using Gefco.CipQuai.Services.Controllers;
using Gefco.CipQuai.Web.Exceptions;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Results;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Gefco.CipQuai.Web.Controllers.Api
{
    public class DeclarationNonConformiteController : ApiController
    {
        [System.Web.Http.Route(nameof(AddDeclarationNonConformite))]
        public BooleanServiceResult AddDeclarationNonConformite(string appVersion, string userId, DeclarationNonConformite declaration)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                var result = new BooleanServiceResult();
                if (user == null)
                {
                    var exception = new InvalidUserException(userId);
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }
                var decl = dal.FindDeclarationNonConformite(declaration.Id);
                if (decl != null)
                {
                    var exception = new DeclarationExistsException();
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    result.IsSuccess = true;
                    return result;
                }
                try
                {
                    user.AppVersion = appVersion;
                    declaration.AgenceId = user.MobileUserAgence_Id;
                    declaration.CreatedBy = user;
                    declaration.CreationDate = DateTime.UtcNow;
                    declaration.AgenceConcernée = dal.FindAgence(declaration.AgenceConcernée.Id);
                    declaration.CurrentStatus_Id = dal.ObjectContext.DeclarationNcStatuses.SingleOrDefault(p => p.Name == DeclarationNcStatus.ToBeValidated)?.Id;
                    var motifs = dal.GetMotifNCs();
                    if (declaration.MotifNCs != null)
                    {
                        var motifNCs = new List<MotifNC>();
                        foreach (var motifDp in declaration.MotifNCs)
                        {
                            motifNCs.Add(motifs.SingleOrDefault(p => p.Id == motifDp.Id));
                        }
                        declaration.MotifNCs = motifNCs;
                    }

                    dal.InsertDeclarationNonConformite(declaration);
                    result.Value = true;
                    result.IsSuccess = true;
                    var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                    var callbackUrl = helper.Action("Detail", "NonConformites", new { declaration.Id });
                    var requestUrl = Properties.Settings.Default.HostUrl;
                    Task.Factory.StartNew(() => SendEmail(requestUrl + callbackUrl, declaration.Id));
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    result.IsSuccess = false;
                    result.SetError(exception, string.Empty, Guid.NewGuid().ToString());
                }
                return result;

            }
        }

        private void SendEmail(string href, string declarationId)
        {
            var recipients = new List<RoleEmailAddress>();
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationNonConformite(declarationId);
                var origine = declaration.AgenceConcernée;
                var destination = declaration.Agence;
                var region = destination.Region;
                if (region.AutoValidateNC)
                {
                    declaration.CurrentStatus = dal.GetDeclarationNcStatuses().SingleOrDefault(p => p.Name == DeclarationNcStatus.Validated);
                    dal.ObjectContext.SaveChanges();
                }
                else
                {
                    return;
                }
                switch (origine.AgenceType.Value)
                {
                    case "Gefco France":
                        {
                            var daRole = dal.ObjectContext.AgenceRoles.Where(p => p.Value == "Directeur Agence").Select(p => p.Key);
                            var da = dal.ObjectContext.Users.Where(p => p.AgenceRoles.Any(r => daRole.Contains(r.AgenceRole_Id) && r.Agence_Id == origine.Id));
                            recipients.AddRange(da.ToList().Select(p => new RoleEmailAddress(p.FirstName + " " + p.LastName, p.Email, $"Directeur d'agence de l'agence {origine.Name}")));

                            var rexRole = dal.ObjectContext.AgenceRoles.Where(p => p.Value == "Responsable Exploitation").Select(p => p.Key);
                            var rex = dal.ObjectContext.Users.Where(p => p.AgenceRoles.Any(r => rexRole.Contains(r.AgenceRole_Id) && r.Agence_Id == origine.Id));
                            recipients.AddRange(rex.ToList().Select(p => new RoleEmailAddress(p.FirstName + " " + p.LastName, p.Email, $"Responsable d'exploitation de l'agence {origine.Name}")));

                            var rqcoRole = dal.ObjectContext.RegionRoles.Where(p => p.Value == "RQCO").Select(p => p.Key);
                            var rqco = dal.ObjectContext.Users.Where(p => p.RegionRoles.Any(r => rqcoRole.Contains(r.RegionRole_Id) && r.Region_Id == origine.Region_Id));
                            recipients.AddRange(rqco.ToList().Select(p => new RoleEmailAddress(p.FirstName + " " + p.LastName, p.Email, $"RQCO de l'agence {origine.Name}")));

                            recipients.Add(new RoleEmailAddress(declaration.CreatedBy.FirstName + " " + declaration.CreatedBy.LastName, declaration.CreatedBy.Email, $"Créateur de la NC"));

                            var config = dal.FindConfiguration("Web.DestinataireNCGefcoFrance");
                            if (config != null)
                            {
                                try
                                {
                                    recipients.Add(new RoleEmailAddress(config.Value, $"Destinataire NC Gefco France"));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                        }
                        break;
                    case "International":
                        {
                            var rqcoRole = dal.ObjectContext.RegionRoles.Where(p => p.Value == "RQCO").Select(p => p.Key);
                            var rqco = dal.ObjectContext.Users.Where(p => p.RegionRoles.Any(r => rqcoRole.Contains(r.RegionRole_Id) && r.Region_Id == destination.Region_Id));
                            recipients.AddRange(rqco.ToList().Select(p => new RoleEmailAddress(p.FirstName + " " + p.LastName, p.Email, $"RQCO de l'agence {origine.Name}")));

                            var config = dal.FindConfiguration("Web.DestinataireNCInternational");
                            if (config != null)
                            {
                                try
                                {
                                    recipients.Add(new RoleEmailAddress(config.Value, $"Destinataire NC International"));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                        }
                        break;
                    case "Confrères":
                        {
                            var rqcoRole = dal.ObjectContext.RegionRoles.Where(p => p.Value == "RQCO").Select(p => p.Key);
                            var rqco = dal.ObjectContext.Users.Where(p => p.RegionRoles.Any(r => rqcoRole.Contains(r.RegionRole_Id) && r.Region_Id == destination.Region_Id));
                            recipients.AddRange(rqco.ToList().Select(p => new RoleEmailAddress(p.FirstName + " " + p.LastName, p.Email, $"RQCO de l'agence {origine.Name}")));

                            var config = dal.FindConfiguration($"Web.DestinataireNCConfrères");
                            if (config != null)
                            {
                                try
                                {
                                    recipients.Add(new RoleEmailAddress(config.Value, $"Destinataire NC Confrères"));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                        }
                        break;
                }
                recipients.Add(new RoleEmailAddress("Waxalica Koba", "sv@sensor6ty.com", $"Copie de vérification"));
                recipients.Add(new RoleEmailAddress("Abdou Diop", "ad@sensor6ty.com", "Copie de vérification"));
                foreach (var recipient in recipients)
                {
                    var template = dal.FindEmailTemplate($"AddDeclarationNonConformite.{origine.AgenceType.Value}");
                    if (template != null)
                        Instances.SendEmail(template.Object, ReplaceTemplate(), recipient);

                    string ReplaceTemplate()
                    {
                        var body = template.Content;
                        body = body.Replace("@@CallbackUrl@@", href);
                        body = body.Replace("@@DestinationName@@", destination.Name);
                        body = body.Replace("@@OrigineName@@", origine.Name);
                        body = body.Replace("@@FullName@@", recipient.Name);
                        body += "Vous recevez ce mail en tant que " + recipient.Role;
                        return body;
                    }
                }
            }
        }


        [System.Web.Http.Route(nameof(UploadPictureNC))]
        [System.Web.Http.HttpPut]
        [ValidateMimeMultipartContentFilter]
        public async Task<PictureServiceResult> UploadPictureNC(string appVersion, string id, PictureType pictureType, string declarationId, string fileName)
        {
            using (var dal = new Dal())
            {
                var result = new PictureServiceResult();
                var user = dal.FindUser(id);
                if (user == null)
                {
                    var exception = new InvalidUserException(id);
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }
                if (declarationId != null)
                {
                    var declaration = dal.FindDeclarationNonConformite(declarationId);
                    if (declaration == null)
                    {
                        var exception = new ArgumentOutOfRangeException(nameof(declarationId));
                        SimpleLogger.GetOne().Error(exception);
                        result.SetError(exception);
                        return result;
                    }
                }
                user.AppVersion = appVersion;
                var rootPath = HttpContext.Current.Server.MapPath("~/App_Data/Pictures");
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);
                var tempDir = Path.Combine(rootPath, "Temp");
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);

                var streamProvider = new MultipartFormDataStreamProvider(tempDir);
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                var fileData = streamProvider.FileData.FirstOrDefault();

                if (fileData != null)
                {
                    string name = fileData.Headers.ContentDisposition.FileName;
                    name = name.Trim("\" ".ToCharArray());
                    var destFileName = Path.Combine(rootPath, name);
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.DeclarationNonConformite_Id == declarationId);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
                        File.Delete(fileData.LocalFileName);
                        return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == pictureType && p.DeclarationNonConformite_Id == declarationId && p.CreatedBy.Id == id);
                    if (picture != null) //Same user already has a picture
                    {
                        if (File.Exists(picture.PicturePath))
                            File.Delete(picture.PicturePath);
                        dal.ObjectContext.Pictures.Remove(picture);
                    }
                    if (!File.Exists(destFileName))
                        File.Copy(fileData.LocalFileName, destFileName);
                    File.Delete(fileData.LocalFileName);
                    picture = new Picture()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedBy = user,
                        CreationDate = DateTime.UtcNow,
                        PictureType = pictureType,
                        PicturePath = destFileName,
                        DeclarationNonConformite_Id = declarationId
                    };
                    dal.InsertPicture(picture);

                    var clone = picture.Clone() as Picture;
                    clone.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = picture.Id });
                    return new PictureServiceResult(clone);
                }

                var exception3 = new ArgumentOutOfRangeException(nameof(fileData));
                SimpleLogger.GetOne().Error(exception3);
                result.SetError(exception3);
                return result;
            }

        }

        [System.Web.Http.Route(nameof(UploadPicNC))]
        [System.Web.Http.HttpPost]
        public async Task<PictureServiceResult> UploadPicNC(PictureParameter parameter)
        {
            using (var dal = new Dal())
            {
                var result = new PictureServiceResult();
                var user = dal.FindUser(parameter.Id);
                if (user == null)
                {
                    var exception = new InvalidUserException(parameter.Id);
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }
                if (parameter.DeclarationId != null)
                {
                    var declaration = dal.FindDeclarationNonConformite(parameter.DeclarationId);
                    if (declaration == null)
                    {
                        var exception = new ArgumentOutOfRangeException(nameof(parameter.DeclarationId));
                        SimpleLogger.GetOne().Error(exception);
                        result.SetError(exception);
                        return result;
                    }
                }
                user.AppVersion = parameter.AppVersion;
                var rootPath = HttpContext.Current.Server.MapPath("~/App_Data/Pictures");
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);
                var tempDir = Path.Combine(rootPath, "Temp");
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);

                var bytes = Convert.FromBase64String(parameter.FileContent);
                if (bytes.Length > 0)
                {
                    string name = parameter.FileName;
                    name = name.Trim("\" ".ToCharArray());
                    var destFileName = Path.Combine(rootPath, name);
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.DeclarationNonConformite_Id == parameter.DeclarationId);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
                        return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == parameter.PictureType && p.DeclarationNonConformite_Id == parameter.DeclarationId && p.CreatedBy.Id == parameter.Id);
                    if (picture != null) //Same user already has a picture
                    {
                        if (File.Exists(picture.PicturePath))
                            File.Delete(picture.PicturePath);
                        dal.ObjectContext.Pictures.Remove(picture);
                    }
                    if (!File.Exists(destFileName))
                        File.WriteAllBytes(destFileName, bytes);
                    picture = new Picture()
                              {
                                  Id = Guid.NewGuid().ToString(),
                                  CreatedBy = user,
                                  CreationDate = DateTime.UtcNow,
                                  PictureType = parameter.PictureType,
                                  PicturePath = destFileName,
                                  DeclarationNonConformite_Id = parameter.DeclarationId
                              };
                    dal.InsertPicture(picture);

                    var clone = picture.Clone() as Picture;
                    clone.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = picture.Id });
                    return new PictureServiceResult(clone);
                }

                var exception3 = new ArgumentOutOfRangeException(nameof(parameter.FileContent));
                SimpleLogger.GetOne().Error(exception3);
                result.SetError(exception3);
                return result;
            }

        }

        [System.Web.Http.Route(nameof(GetMotifNCs))]
        public MotifNCListServiceResult GetMotifNCs(string appVersion, string userId)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                if (user == null)
                {
                    SimpleLogger.GetOne().Error(new InvalidUserException(userId));
                    return null;
                }
                user.AppVersion = appVersion;
                dal.ObjectContext.SaveChanges();
                return new MotifNCListServiceResult(dal.GetMotifNCs().CloneList());

            }
        }

        [System.Web.Http.Route(nameof(GetAgences))]
        public AgenceListServiceResult GetAgences(string appVersion, string userId)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                if (user == null)
                {
                    SimpleLogger.GetOne().Error(new InvalidUserException(userId));
                    return null;
                }
                user.AppVersion = appVersion;
                dal.ObjectContext.SaveChanges();
                return new AgenceListServiceResult(dal.GetAgences(userId).CloneList());

            }
        }

    }

}
