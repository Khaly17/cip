using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Exceptions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Results;
using Gefco.CipQuai.Web.Views;
using Microsoft.AspNet.Identity;
using Microsoft.Exchange.WebServices.Data;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize()]
    public class DoublesPlanchersController : Controller
    {
        private readonly ApplicationDbContext _ctx;

        public DoublesPlanchersController()
        {
            _ctx = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Detail(string id, string rUrl)
        {
            var declaration = _ctx.DeclarationDoublePlanchers.SingleOrDefault(p => p.Id == id);
            var tempPictures = _ctx.TempPictures.Where(p => p.DeclarationBonnePratique_Id == id).ToList();
            var viewModel = new DetailDPViewModel(declaration, tempPictures);
            if (declaration.Pictures != null)
                foreach (var picture in declaration.Pictures)
                {
                    if (System.IO.File.Exists(picture.PicturePath))
                        picture.PicturePath = Url.Action("GetPicture", "Pictures", new { id = picture.Id });
                }
            ViewBag.rUrl = rUrl;
            return View(viewModel);
        }
        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "AQHSE,Alternant QHSE,RQHSE", InputType = InputType.DP)]
        public ActionResult Validate(string id, string rUrl)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationDoublePlancher(id);
                declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "Valid");
                dal.UpdateDeclarationDoublePlancher(declaration, dal.FindUser(User.Identity.GetUserId()));
            }
            if (!string.IsNullOrWhiteSpace(rUrl))
                return RedirectToLocal(rUrl);
            return RedirectToAction("Detail", new {id});
        }

        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "AQHSE,Alternant QHSE,RQHSE", InputType = InputType.DP)]
        public ActionResult Invalidate(string id, string rUrl)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationDoublePlancher(id);
                declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "NotValid");
                dal.UpdateDeclarationDoublePlancher(declaration, dal.FindUser(User.Identity.GetUserId()));
                var href = Url.Action("Detail", "DoublesPlanchers", new { id = declaration.Id });
                var requestUrl = Properties.Settings.Default.HostUrl;
                new TaskFactory().StartNew(() => SendEmail(requestUrl + href, declaration.Id));
            }

            if (!string.IsNullOrWhiteSpace(rUrl))
                return RedirectToLocal(rUrl);
            return RedirectToAction("Detail", new { id });
        }

        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "AQHSE,Alternant QHSE,RQHSE", InputType = InputType.DP)]
        private void SendEmail(string href, string declarationId)
        {
            var recipients = new List<EmailAddress>();
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationDoublePlancher(declarationId);
                var origine = declaration.Traction.AgenceDepart;
                var destination = declaration.Traction.AgenceArrivee;

                var daRole = dal.ObjectContext.AgenceRoles.Single(p => p.Value == "Directeur Agence");
                var da = dal.ObjectContext.Users.Where(p => p.AgenceRoles.Any(r => r.AgenceRole_Id == daRole.Key && r.Agence_Id == origine.Id));
                recipients.AddRange(da.ToList().Select(p => new EmailAddress(p.FirstName + " " + p.LastName, p.Email)));

                foreach (var recipient in recipients)
                {
                    var template = dal.FindEmailTemplate("InvalidateDeclarationDoublePlancher");
                    if (template != null)
                        Instances.SendEmail(template.Object, ReplaceTemplate(), recipient);

                    string ReplaceTemplate()
                    {
                        var body = template.Content;
                        body = body.Replace("@@CallbackUrl@@", href);
                        body = body.Replace("@@DestinationName@@", destination.Name.ToUpper());
                        body = body.Replace("@@OrigineName@@", origine.Name.ToUpper());
                        body = body.Replace("@@DeclarationDueDate@@", declaration.Traction.DueDate.ToString("dd/MM/yyyy"));
                        body = body.Replace("@@FullName@@", recipient.Name);
                        return body;
                    }
                }

            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "AQHSE,Alternant QHSE,RQHSE", InputType = InputType.DP)]
        public ActionResult ToJustify(string id, string rUrl)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationDoublePlancher(id);
                declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "ToJustify");
                dal.UpdateDeclarationDoublePlancher(declaration, dal.FindUser(User.Identity.GetUserId()));
            }
            if (!string.IsNullOrWhiteSpace(rUrl))
                return RedirectToLocal(rUrl);
            return RedirectToAction("Detail", new { id });
        }

        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "RQCO", InputType = InputType.DP)]
        public ActionResult ChangeStatus(string id, string rUrl, string status)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationDoublePlancher(id);
                switch (status)
                {
                    case "Suspend":
                        declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "PausedAndFree");
                        break;
                    case "Free":
                        declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "PausedAndFree");
                        break;
                    case "AskPics":
                        declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "InProgress");
                        break;
                    case "Close":
                        declaration.CurrentStatus = dal.GetRemorqueStatuses().SingleOrDefault(p => p.Name == "ToBeValidated");
                        break;
                    case "Delete":
                        declaration.IsDeleted = !declaration.IsDeleted;
                        break;
                }
                dal.UpdateDeclarationDoublePlancher(declaration, dal.FindUser(User.Identity.GetUserId()));
                switch (status)
                {
                    case "Suspend":
                    case "Free":
                    case "AskPics":
                    case "Close":
                        if (!string.IsNullOrWhiteSpace(rUrl))
                            return RedirectToLocal(rUrl);
                        break;
                }
                if (declaration.IsDeleted)
                    if (!string.IsNullOrWhiteSpace(rUrl))
                        return RedirectToLocal(rUrl);
            }
            return RedirectToAction("Detail", new { id });
        }

        public void MergeChunkFile(string fullPath, Stream chunkContent)
        {
            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (chunkContent)
                    {
                        chunkContent.CopyTo(stream);
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        [AcceptVerbs("Post")]
        public void Save(string id, PictureType pictureType)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Length > 0)
                {
                    var httpPostedChunkFile = System.Web.HttpContext.Current.Request.Files["chunkFile"];
                    if (httpPostedChunkFile != null)
                    {
                        var saveFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Pictures");
                        var SaveFilePath = Path.Combine(saveFile, httpPostedChunkFile.FileName + ".part");
                        var chunkIndex = System.Web.HttpContext.Current.Request.Form["chunk-index"];
                        if (chunkIndex == "0")
                        {
                            httpPostedChunkFile.SaveAs(SaveFilePath);
                        }
                        else
                        {
                            MergeChunkFile(SaveFilePath, httpPostedChunkFile.InputStream);
                            var totalChunk = System.Web.HttpContext.Current.Request.Form["total-chunk"];
                            if (Convert.ToInt32(chunkIndex) == (Convert.ToInt32(totalChunk) - 1))
                            {
                                var savedFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Pictures");
                                var originalFilePath = Path.Combine(savedFile, httpPostedChunkFile.FileName);
                                if (System.IO.File.Exists(originalFilePath))
                                    System.IO.File.Delete(originalFilePath);
                                System.IO.File.Move(SaveFilePath, originalFilePath);
                                SaveDeclaration(User.Identity.GetUserId(), id, originalFilePath, pictureType);
                            }
                        }
                        HttpResponse ChunkResponse = System.Web.HttpContext.Current.Response;
                        ChunkResponse.Clear();
                        ChunkResponse.ContentType = "application/json; charset=utf-8";
                        ChunkResponse.StatusDescription = "File uploaded succesfully";
                        ChunkResponse.End();
                    }
                    var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadFiles"];

                    if (httpPostedFile != null)
                    {
                        var fileSave = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Pictures");
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            httpPostedFile.SaveAs(fileSavePath);
                            HttpResponse Response = System.Web.HttpContext.Current.Response;
                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.StatusDescription = "File uploaded succesfully";
                            Response.End();
                            SaveDeclaration(User.Identity.GetUserId(), id, fileSavePath, pictureType);
                        }
                        else
                        {
                            HttpResponse Response = System.Web.HttpContext.Current.Response;
                            Response.Clear();
                            Response.Status = "204 File already exists";
                            Response.StatusCode = 204;
                            Response.StatusDescription = "File already exists";
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.Status = "204 No Content";
                Response.StatusDescription = e.Message;
                Response.End();
            }
        }
        
        private void SaveDeclaration(string userId, string id, string originalFilePath, PictureType pictureType)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);

                var picture = dal.GetPictures().SingleOrDefault(p => p.PictureType == pictureType && p.DeclarationRemorque_Id == id && p.CreatedBy.Id == id);
                if (picture != null) //Same user already has a picture
                {
                    dal.ObjectContext.Pictures.Remove(picture);
                }
                picture = new Picture()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedBy = user,
                    CreationDate = DateTime.UtcNow,
                    PictureType = pictureType,
                    PicturePath = originalFilePath,
                    DeclarationRemorque_Id = id
                };
                dal.InsertPicture(picture);
            }
        }

        [AcceptVerbs("Post")]
        public void Remove()
        {
            try
            {
                //var fileSave = "";
                //if (System.Web.HttpContext.Current.Request.Form["cancel-uploading"] != null)
                //{
                //    fileSave = System.Web.HttpContext.Current.Server.MapPath("UploadingFiles");
                //}
                //else
                //{
                //    fileSave = System.Web.HttpContext.Current.Server.MapPath("UploadedFiles");
                //}
                //var fileName = System.Web.HttpContext.Current.Request.Form["UploadFiles"];
                //var fileSavePath = Path.Combine(fileSave, fileName);
                // if (System.IO.File.Exists(fileSavePath))
                // {
                //     System.IO.File.Delete(fileSavePath);
                // }
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.Status = "200 OK";
                Response.StatusCode = 200;
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusDescription = "File removed succesfully";
                Response.End();
            }
            catch (Exception e)
            {
                HttpResponse Response = System.Web.HttpContext.Current.Response;
                Response.Clear();
                Response.Status = "200 OK";
                Response.StatusCode = 200;
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusDescription = "File removed succesfully";
                Response.End();
            }
        }
    }
}