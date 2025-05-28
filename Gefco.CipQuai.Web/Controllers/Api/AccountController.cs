using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Gefco.CipQuai.Services.Controllers;
using Gefco.CipQuai.Web.Exceptions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Owin.Security;
using Task = System.Threading.Tasks.Task;

namespace Gefco.CipQuai.Web.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            _userManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; }

        // POST api/Account/LoginApplicationUser
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route(nameof(Login))]
        public async Task<ApplicationUser> Login(string appVersion, string userName, string password)
        {
            try
            {
                var user = await UserManager.FindAsync(userName, password);

                if (!UserManager.IsInRole(user.Id, "Mobile Users"))
                    return null;
                using (var dal = new Dal())
                {
                    var usr = dal.FindUser(user.Id);
                    var result = usr.Clone();
                    if (usr.ProfilePicture != null && File.Exists(usr.ProfilePicture.PicturePath))
                        result.ProfilePicture.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = usr.ProfilePicture.Id });
                    usr.AppVersion = appVersion;
                    await dal.ObjectContext.SaveChangesAsync();
                    return result;
                }
            }
            catch (Exception e)
            {
                SimpleLogger.GetOne().Error(e);
                return null;
            }
        }

        // POST api/Account/ChangePassword
        [System.Web.Http.Route(nameof(ChangePassword))]
        public async Task<IHttpActionResult> ChangePassword(string appVersion, string userId, string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var dal = new Dal())
            {
                var usr = dal.FindUser(userId);
                if (usr.NeedsChangePin)
                    usr.NeedsChangePin = false;
                usr.AppVersion = appVersion;
                await dal.ObjectContext.SaveChangesAsync();
            }

            var result = await UserManager.ChangePasswordAsync(userId, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/ChangePassword
        [System.Web.Http.Route(nameof(ForgotPassword))]
        public async Task<IHttpActionResult> ForgotPassword(string appVersion, string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await UserManager.FindByEmailAsync(email);
                using (var dal = new Dal())
                {
                    var usr = dal.FindUser(user.Id);
                    usr.AppVersion = appVersion;
                    if (usr.SecurityStamp.IsNullOrWhiteSpace())
                        usr.SecurityStamp = Guid.NewGuid().ToString();

                    await dal.ObjectContext.SaveChangesAsync();
                }
                var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = helper.Action("ResetPassword", "Account", new { userId = user.Id, code = code });

                var requestUrl = Properties.Settings.Default.HostUrl;
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                using (var dal = new Dal())
                {
                    var template = dal.FindEmailTemplate("WebResetPassword");
                    if (template != null)
                        Instances.SendEmail(template.Object, ReplaceTemplate(), new EmailAddress(user.Email));

                    string ReplaceTemplate()
                    {
                        var body = template.Content;
                        body = body.Replace("@@CallbackUrl@@", requestUrl + callbackUrl);
                        body = body.Replace("@@FullName@@", user.FirstName + " " + user.LastName);
                        return body;
                    }
                }

                return Ok();
            }
            catch (Exception e)
            {
                SimpleLogger.GetOne().Error(e);
                return BadRequest(e.ToString());
            }
        }

        // POST api/Account/RegisterApplicationUser
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route(nameof(Register))]
        public async Task<UserServiceResult> Register(string appVersion, string fgu, string password, string firstName, string lastName, string phoneNumber)
        {
            var resultM = new UserServiceResult();
            if (!ModelState.IsValid)
            {
                resultM.IsSuccess = false;
                return resultM;
            }

            try
            {
                var user = new ApplicationUser() { UserName = fgu, Email = fgu + "@gefco.net", FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber };

                IdentityResult result = await UserManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    resultM.IsSuccess = false;
                    return resultM;
                }


                resultM.Value = user.Clone();
                resultM.IsSuccess = true;
            }
            catch (Exception ex)
            {
                resultM.SetError(ex, "Error", "0x001");
            }
            return resultM;
        }
        
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route(nameof(RegisterSeed))]
        public async Task<UserServiceResult> RegisterSeed(string fgu, string password, string firstName, string lastName, string phoneNumber, string email)
        {
            var resultM = new UserServiceResult();
            if (!ModelState.IsValid)
            {
                resultM.IsSuccess = false;
                return resultM;
            }

            try
            {
                var user = new ApplicationUser() { UserName = fgu, Email = email, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber };

                IdentityResult result = await UserManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    resultM.IsSuccess = false;
                    return resultM;
                }


                resultM.Value = user.Clone();
                resultM.IsSuccess = true;
            }
            catch (Exception ex)
            {
                resultM.SetError(ex, "Error", "0x001");
            }
            return resultM;
        }

        [System.Web.Http.Route(nameof(SeedMe))]
        public async Task<string> SeedMe()
        {
            var sb = new StringBuilder();
            using (var dal = new Dal())
            {
                var users = dal.ObjectContext.Users.Where(p => p.PasswordHash == null).ToList();
                foreach (var user in users)
                {
                    dal.ObjectContext.Users.Remove(user);
                    dal.ObjectContext.SaveChanges();
                    var pin = Tools.GenerateRandomNumber(4);
                    await RegisterSeed(user.UserName, pin, user.FirstName, user.LastName, null, user.Email);
                    sb.AppendLine($"{user.UserName}\t{pin}");
                }
            }
            return sb.ToString();

        }

        [System.Web.Http.Route(nameof(Update))]
        [System.Web.Http.HttpPost]
        public UserServiceResult Update(string appVersion, string userId, string email, string firstName, string lastName, string phoneNumber)
        {
            var result = new UserServiceResult();
            try
            {
                using (var dal = new Dal())
                {
                    var user = dal.FindUser(userId);
                    if (user != null)
                    {
                        user.AppVersion = appVersion;
                        if (!string.IsNullOrEmpty(email))
                        {
                            user.Email = email.ToLower();
                        }
                        if (!string.IsNullOrEmpty(firstName))
                            user.FirstName = firstName;
                        if (!string.IsNullOrEmpty(lastName))
                            user.LastName = lastName;

                        if (!string.IsNullOrEmpty(phoneNumber))
                        {
                            var number = Tools.GetE164Number(Tools.CleanPhoneNumber(phoneNumber));
                            user.PhoneNumber = number;
                        }

                        //ctx.SaveChanges();
                        dal.UpdateUser(user);
                        result.Value = user.Clone();
                        return result;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex, "An error occured. Log follows", $"{nameof(AccountController)}.{nameof(Update)}");
            }
            return result;
        }

        [System.Web.Http.Route(nameof(SetDevice))]
        [System.Web.Http.HttpPost]
        public BooleanServiceResult SetDevice(string appVersion, string userId, DeviceType deviceType, string deviceToken)
        {
            var result = new BooleanServiceResult();
            try
            {
                using (var dal = new Dal())
                {
                    var user = dal.FindUser(userId);
                    if (user != null)
                    {
                        user.AppVersion = appVersion;
                        var device = dal.GetDevices().FirstOrDefault(p => p.DeviceType == deviceType && p.DeviceToken == deviceToken);
                        if (device == null)
                            dal.InsertDevice(new Device
                            {
                                Id = Guid.NewGuid().ToString(),
                                DeviceToken = deviceToken,
                                DeviceType = deviceType,
                                UserId = userId,
                                CreationDate = DateTime.UtcNow,
                            });
                        else
                        {
                            device.DeviceToken = deviceToken;
                            dal.UpdateDevice(device);
                        }

                        result.Value = true;
                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex, "An error occured. Log follows", $"{nameof(AccountController)}.{nameof(SetDevice)}");
            }
            return result;
        }

        [System.Web.Http.Route(nameof(HasVersion))]
        [System.Web.Http.HttpPost]
        public BooleanServiceResult HasVersion(string appVersion, string userId)
        {
            var result = new BooleanServiceResult();
            try
            {
                using (var dal = new Dal())
                {
                    var user = dal.FindUser(userId);
                    if (user != null)
                    {
                        user.AppVersion = appVersion;
                        var version = int.Parse(dal.GetUsers().Max(p => p.AppVersion).Replace(".", ""));
                        var uVersion = int.Parse(appVersion.Replace(".", ""));
                        result.Value = uVersion >= version;
                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                result.SetError(ex, "An error occured. Log follows", $"{nameof(AccountController)}.{nameof(SetDevice)}");
            }
            return result;
        }

        [System.Web.Http.Route(nameof(UploadProfilePicture))]
        [System.Web.Http.HttpPut]
        [ValidateMimeMultipartContentFilter]
        public async Task<PictureServiceResult> UploadProfilePicture(string appVersion, string id, string fileName)
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
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.ApplicationUser_Id == id);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
                        return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == PictureType.Profile && p.ApplicationUser_Id == id && p.CreatedBy.Id == id);
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
                        PictureType = PictureType.Profile,
                        PicturePath = destFileName,
                        ApplicationUser_Id = id
                    };
                    dal.InsertPicture(picture);
                    user.ProfilePicture_Id = picture.Id;
                    await dal.ObjectContext.SaveChangesAsync().ConfigureAwait(false);
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
        
        [System.Web.Http.Route(nameof(UploadProfilePic))]
        [System.Web.Http.HttpPost]
        public async Task<PictureServiceResult> UploadProfilePic(PictureParameter parameter)
        {
            var start = DateTime.Now;
            var before = DateTime.Now;
            var now = DateTime.Now;
            SimpleLogger.GetOne().Debug(nameof(UploadProfilePic) + " Entered");
            using (var dal = new Dal())
            {
                var result = new PictureServiceResult();
                var user = dal.FindUser(parameter.Id);
                now = DateTime.Now;
                SimpleLogger.GetOne().Debug(nameof(UploadProfilePic) + $" Fetch user in {(now-before).TotalMilliseconds} ms");
                before = now;
                if (user == null)
                {
                    var exception = new InvalidUserException(parameter.Id);
                    SimpleLogger.GetOne().Error(exception);
                    result.SetError(exception);
                    return result;
                }
                user.AppVersion = parameter.AppVersion;
                var rootPath = HttpContext.Current.Server.MapPath("~/App_Data/Pictures");
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);
                var tempDir = Path.Combine(rootPath, "Temp");
                if (!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);
                now = DateTime.Now;
                SimpleLogger.GetOne().Debug(nameof(UploadProfilePic) + $" Verified directories in {(now-before).TotalMilliseconds} ms");
                before = now;

                var bytes = Convert.FromBase64String(parameter.FileContent);
                if (bytes.Length > 0)
                {
                    string name = parameter.FileName;
                    name = name.Trim("\" ".ToCharArray());
                    var destFileName = Path.Combine(rootPath, name);
                    var samePicture = dal.GetPictures().SingleOrDefault(p => p.PicturePath == destFileName && p.ApplicationUser_Id == parameter.Id);
                    if (samePicture != null) //Already the same picture exists
                    {
                        var clone0 = samePicture.Clone() as Picture;
                        clone0.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = samePicture.Id });
                        return new PictureServiceResult(clone0);
                    }
                    var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == PictureType.Profile && p.ApplicationUser_Id == parameter.Id && p.CreatedBy.Id == parameter.Id);
                    if (picture != null) //Same user already has a picture
                    {
                        if (File.Exists(picture.PicturePath))
                            File.Delete(picture.PicturePath);
                        dal.ObjectContext.Pictures.Remove(picture);
                    }
                    if (!File.Exists(destFileName))
                        File.WriteAllBytes(destFileName, bytes);
                    now = DateTime.Now;
                    SimpleLogger.GetOne().Debug(nameof(UploadProfilePic) + $" Wrote file in {(now-before).TotalMilliseconds} ms");
                    before = now;

                    picture = new Picture()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedBy = user,
                        CreationDate = DateTime.UtcNow,
                        PictureType = PictureType.Profile,
                        PicturePath = destFileName,
                        ApplicationUser_Id = parameter.Id
                    };
                    try
                    {
                        dal.InsertPicture(picture);
                        new Task(() =>
                                 {
                                     Task.Delay(TimeSpan.FromSeconds(20));
                                     using (var ctx = new ApplicationDbContext())
                                     {
                                         var _user = dal.FindUser(parameter.Id);

                                         _user.ProfilePicture_Id = picture.Id;
                                         ctx.SaveChanges();
                                     }
                                 }).Start();
                        now = DateTime.Now;
                        SimpleLogger.GetOne().Debug(nameof(UploadProfilePic) + $" Saved picture in DB in {(now-before).TotalMilliseconds} ms");
                        SimpleLogger.GetOne().Debug(nameof(UploadProfilePic) + $" Returning clone picture. Total time for {nameof(UploadProfilePic)} : {(now-start).TotalMilliseconds} ms");
                        var clone = picture.Clone() as Picture;
                        clone.PicturePath = Url.Link("Default", new { controller = "Pictures", action = "GetPicture", id = picture.Id });
                        return new PictureServiceResult(clone);
                    }
                    catch (Exception e)
                    {
                        SimpleLogger.GetOne().Error(e);
                        result.SetError(e);
                        return result;
                    }
                }

                var exception3 = new ArgumentOutOfRangeException(nameof(parameter.FileContent));
                SimpleLogger.GetOne().Error(exception3);
                result.SetError(exception3);
                return result;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        //private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


        #endregion
    }
}
