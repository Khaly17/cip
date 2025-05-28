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
    public class DeclarationRemorqueController : ApiController
    {
        [Route(nameof(AddDeclarationRemorque))]
        public BooleanServiceResult AddDeclarationRemorque(string appVersion, string userId, DeclarationSimplePlancher declaration)
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
                if (declaration.AutreAgenceArrivee.IsNullOrWhiteSpace())
                {
                    var exception = new ArgumentException("Invalid argument", "declaration.AutreAgenceArrivee");
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }

                var decl = dal.FindDeclarationSimplePlancher(declaration.Id);
                if (decl != null)
                {
                    var exception = new DeclarationExistsException();
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    result.IsSuccess = true;
                    return result;
                }
                decl = dal.ObjectContext.DeclarationSimplePlanchers.SingleOrDefault(p => p.Id == declaration.Id);
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
                    declaration.AgenceId = user.MobileUserAgence_Id;
                    user.AppVersion = appVersion;
                    declaration.CreatedBy = user;
                    declaration.CreationDate = DateTime.UtcNow;
                    declaration.CompletionDate = DateTime.UtcNow;
                    declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "ToBeValidated");
                    dal.InsertDeclarationRemorque(declaration);
                    result.Value = true;
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

        [Route(nameof(UploadPictureSP))]
        [HttpPut]
        [ValidateMimeMultipartContentFilter]
        public async Task<PictureServiceResult> UploadPictureSP(string appVersion, string id, PictureType pictureType, string declarationId, string fileName)
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
                    var declaration = dal.FindDeclarationSimplePlancher(declarationId);
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

        [Route(nameof(UploadPicSP))]
        [HttpPost]
        public async Task<PictureServiceResult> UploadPicSP(PictureParameter parameter)
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
                    var declaration = dal.FindDeclarationSimplePlancher(parameter.DeclarationId);
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
                                  CreatedBy = user,
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
    }
}
