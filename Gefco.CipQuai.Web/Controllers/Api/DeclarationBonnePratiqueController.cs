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
    public class DeclarationBonnePratiqueController : ApiController
    {
        [Route(nameof(GetActeurs))]
        public ApplicationUserListServiceResult GetActeurs(string appVersion, string userId)
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
                var applicationUsers = dal.GetUsers().CloneList();
                return new ApplicationUserListServiceResult(applicationUsers);
                
            }
        }

        [Route(nameof(AddDeclarationBonnePratique))]
        public BooleanServiceResult AddDeclarationBonnePratique(string appVersion, string userId, DeclarationBonnePratique declaration)
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
                var decl = dal.FindDeclarationBonnePratique(declaration.Id);
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
                    //declaration.AgentConcerné = dal.FindUser(declaration.AgentConcerné.Id);
                    dal.InsertDeclarationBonnePratique(declaration);
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


        [Route(nameof(UploadPictureBP))]
        [HttpPut]
        [ValidateMimeMultipartContentFilter]
        public async Task<PictureServiceResult> UploadPictureBP(string appVersion, string id, PictureType pictureType, string declarationId, string fileName)
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
                    var declaration = dal.FindDeclarationBonnePratique(declarationId);
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
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.DeclarationBonnePratique_Id == declarationId);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
						File.Delete(fileData.LocalFileName);
						return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == pictureType && p.DeclarationBonnePratique_Id == declarationId && p.CreatedBy.Id == id);
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
                        DeclarationBonnePratique_Id = declarationId
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

        [Route(nameof(UploadPicBP))]
        [HttpPost]
        public async Task<PictureServiceResult> UploadPicBP(PictureParameter parameter)
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
                    var declaration = dal.FindDeclarationBonnePratique(parameter.DeclarationId);
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
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.DeclarationBonnePratique_Id == parameter.DeclarationId);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
						return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == parameter.PictureType && p.DeclarationBonnePratique_Id == parameter.DeclarationId && p.CreatedBy.Id == parameter.Id);
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
                                  DeclarationBonnePratique_Id = parameter.DeclarationId
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
