using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Gefco.CipQuai.Services.Controllers;
using Gefco.CipQuai.Web.Exceptions;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Results;

namespace Gefco.CipQuai.Web.Controllers.Api
{
    public class DeclarationDoublePlancherController : ApiController
    {
        [Route(nameof(GetActiveDeclarationDoublePlanchers))]
        public DeclarationDoublePlancherListServiceResult GetActiveDeclarationDoublePlanchers(string appVersion, string userId)
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
                var declarationDoublePlanchers = dal.GetActiveDeclarationDoublePlanchers(user).CloneList();
                foreach (var declaration in declarationDoublePlanchers)
                {
                    if (declaration.Pictures != null)
                        foreach (var picture in declaration.Pictures)
                        {
                            if (File.Exists(picture.PicturePath))
                                picture.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = picture.Id });
                        }
                }
                return new DeclarationDoublePlancherListServiceResult(declarationDoublePlanchers);
                
            }
        }

        [Route(nameof(AddDeclarationDoublePlancher))]
        public StringServiceResult AddDeclarationDoublePlancher(string appVersion, string userId, DeclarationDoublePlancher declaration, string tractionId)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                var result = new StringServiceResult();
                if (user == null)
                {
                    var exception = new InvalidUserException(userId);
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }
                if (tractionId.IsNullOrWhiteSpace())
                {
                    var exception = new ArgumentException("Invalid argument", nameof(tractionId));
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }
                if (declaration.Traction.AgenceArrivee.Id != null && declaration.Traction.AgenceArrivee.Id == Guid.Empty.ToString())
                {
                    var exception = new ArgumentException("Invalid argument", "declaration.Traction.AgenceArrivee.Id");
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }

                var decl = dal.FindDeclarationDoublePlancher(declaration.Id);
                if (decl != null)
                {
                    var exception = new DeclarationExistsException();
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    result.IsSuccess = true;
                    return result;
                }
                decl = dal.ObjectContext.DeclarationDoublePlanchers.SingleOrDefault(p => p.Traction_Id == tractionId);
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
                    declaration.CreatedBy = user;
                    declaration.CreationDate = DateTime.UtcNow;
                    declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "InProgress");
                    if (tractionId != Guid.Empty.ToString())
                    {
                        declaration.Traction_Id = tractionId;
                        var traction = dal.FindTraction(tractionId);
                        traction.IsCreated = true;
                    }
                    else
                    {
                        var traction = new Traction()
                        {
                            Id = Guid.NewGuid().ToString(),
                            AgenceDepart = user.MobileUserAgence,
                            Name = declaration.Traction.Name,
                            CreatedBy = user,
                            CreationDate = DateTime.UtcNow,
                            DueDate = DateTime.Now.Date,
                            IsCreated = true
                        };
                        if (declaration.Traction.AgenceArrivee.Id != null && declaration.Traction.AgenceArrivee.Id != Guid.Empty.ToString())
                            traction.AgenceArrivee = dal.FindAgence(declaration.Traction.AgenceArrivee.Id);
                        declaration.Traction_Id = traction.Id;
                        dal.InsertTraction(traction);
                    }
                    declaration.Traction = null;
                    dal.InsertDeclarationDoublePlancher(declaration);
                    result.Value = declaration.Traction_Id;
                    result.IsSuccess = true;
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

        [Route(nameof(UpdateDeclarationDoublePlancher))]
        public BooleanServiceResult UpdateDeclarationDoublePlancher(string appVersion, string userId, DeclarationDoublePlancher declaration, string statusId)
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
                try
                {
                    user.AppVersion = appVersion;
                    var decl = dal.FindDeclarationDoublePlancher(declaration.Id);
                    if (decl == null)
                    {
                        var exception = new InvalidDeclarationException(userId);
                        SimpleLogger.GetOne().Error(exception);
                        result.SetError(exception);
                        return result;
                    }
                    if (!decl.IsMineActive(user) && !decl.IsOthersActive(user))
                    {
                        var exception = new DeclarationInProgressException();
                        SimpleLogger.GetOne().Error(exception);
                        result.SetError(exception);
                        return result;
                    }

                    if (decl.CreatedBy_Id != userId)
                    {
                        var bypass = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "PausedAndFree");
                        if (decl.CurrentStatus_Id == bypass.Id)
                        {
                            decl.CreatedBy_Id = userId;
                        }
                    }
                    declaration.AgenceId = user.MobileUserAgence_Id;

                    decl.UpdateWith(declaration, dal.GetMotifDPs());
                    var status = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "ToBeValidated");
                    if (statusId == status.Id) //nettoyage en fonction de DP Used
                    {
                        switch (declaration.IsDPUsed)
                        {
                            case true:
                                decl.Pictures?.RemoveAll(p => p.CreatedBy_Id == userId && DeclarationDoublePlancher.DpNotUsedPictureTypes.Contains(p.PictureType));
                                decl.MotifDps.Clear();
                                decl.NbDPCassées = 0;
                                decl.AutreMotifDP = null;
                                break;
                            case false:
                                decl.Pictures?.RemoveAll(p => p.CreatedBy_Id == userId && DeclarationDoublePlancher.DpUsedPictureTypes.Contains(p.PictureType));
                                break;
                        }
                        decl.CompletionDate = DateTime.UtcNow;
                    }

                    dal.UpdateDeclarationDoublePlancher(decl, user);
                    dal.ObjectContext.SaveChanges();
                    result.Value = true;
                    result.IsSuccess = true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    result.SetError(exception);
                }
                return result;
                
            }
        }

        [Route(nameof(GetTractions))]
        public TractionListServiceResult GetTractions(string appVersion, string userId)
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
                var tractions = dal.GetTractions(user).CloneList();
                return new TractionListServiceResult(tractions);
                
            }
        }

        [Route(nameof(UploadPictureDP))]
        [HttpPut]
        [ValidateMimeMultipartContentFilter]
        public async Task<PictureServiceResult> UploadPictureDP(string appVersion, string id, PictureType pictureType, string declarationId, string fileName)
        {
            var dal = new Dal();
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
                    var declaration = dal.FindDeclarationDoublePlancher(declarationId);
                    if (declaration == null)
                    {
                        var exception1 = new ArgumentOutOfRangeException(nameof(declarationId));
                        SimpleLogger.GetOne().Error(exception1);
                        result.SetError(exception1);
                        return result;
                    }
                    if (!declaration.IsMine(user))
                    {
                        var exception2 = new DeclarationInProgressException();
                        SimpleLogger.GetOne().Error(exception2);
                        result.SetError(exception2);
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
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.DeclarationRemorque_Id == declarationId);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
                        File.Delete(fileData.LocalFileName);
                        return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == pictureType && p.DeclarationRemorque_Id == declarationId && p.CreatedBy.Id == id);
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
                        DeclarationRemorque_Id = declarationId
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

        [Route(nameof(UploadPicDP))]
        [HttpPost]
        public async Task<PictureServiceResult> UploadPicDP(PictureParameter parameter)
        {
            var dal = new Dal();
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
                    var declaration = dal.FindDeclarationDoublePlancher(parameter.DeclarationId);
                    if (declaration == null)
                    {
                        var exception1 = new ArgumentOutOfRangeException(nameof(parameter.DeclarationId));
                        SimpleLogger.GetOne().Error(exception1);
                        result.SetError(exception1);
                        return result;
                    }
                    if (!declaration.IsMine(user))
                    {
                        var exception2 = new DeclarationInProgressException();
                        SimpleLogger.GetOne().Error(exception2);
                        result.SetError(exception2);
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
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.DeclarationRemorque_Id == parameter.DeclarationId);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
                        return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == parameter.PictureType && p.DeclarationRemorque_Id == parameter.DeclarationId && p.CreatedBy.Id == parameter.Id);
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
                                  CreatedBy_Id = user.Id,
                                  CreationDate = DateTime.UtcNow,
                                  PictureType = parameter.PictureType,
                                  PicturePath = destFileName,
                                  DeclarationRemorque_Id = parameter.DeclarationId
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

        [Route(nameof(GetMotifDPs))]
        public MotifDPListServiceResult GetMotifDPs(string appVersion, string userId)
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
                return new MotifDPListServiceResult(dal.GetMotifDPs().CloneList());
                
            }
        }
    }
}
