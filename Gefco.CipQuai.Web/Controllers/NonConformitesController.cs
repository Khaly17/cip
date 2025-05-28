using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Views;
using Microsoft.AspNet.Identity;
using Microsoft.Exchange.WebServices.Data;
using Task = System.Threading.Tasks.Task;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize]
    public class NonConformitesController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public NonConformitesController()
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
            var declaration = _ctx.DeclarationNonConformites.SingleOrDefault(p => p.Id == id);
            var tempPictures = _ctx.TempPictures.Where(p => p.DeclarationBonnePratique_Id == id).ToList();
            var viewModel = new DetailNCViewModel(declaration, tempPictures);
            if (declaration.Pictures != null)
                foreach (var picture in declaration.Pictures)
                {
                    if (System.IO.File.Exists(picture.PicturePath))
                        picture.PicturePath = Url.Action("GetPicture", "Pictures", new { id = picture.Id });
                }
            ViewBag.rUrl = rUrl;
            var agences = _ctx.Agences.Where(p => !p.IsDeleted).OrderBy(p => p.Name).ToList();
            ViewBag.AgenceConcernee_Id = new SelectList(agences, "Id", "Name", Guid.Empty.ToString());
            return View(viewModel);
        }

        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "RQCO", InputType = InputType.NC)]
        public ActionResult Validate(string id)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationNonConformite(id);
                declaration.CurrentStatus = dal.GetDeclarationNcStatuses().SingleOrDefault(p => p.Name == DeclarationNcStatus.Validated);
                dal.ObjectContext.SaveChanges();
            }
            var callbackUrl = Url.Action("Detail", "NonConformites", new { id });
            var requestUrl = Properties.Settings.Default.HostUrl;
            Task.Factory.StartNew(() => SendEmail(requestUrl + callbackUrl, id));
            return RedirectToAction("Detail", new { id });
        }
        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "RQCO", InputType = InputType.NC)]
        public ActionResult Delete(string id)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationNonConformite(id);
                declaration.IsDeleted = !declaration.IsDeleted;
                //declaration.CurrentStatus = dal.GetDeclarationNcStatuses().SingleOrDefault(p => p.Name == DeclarationNcStatus.Deleted);
                dal.ObjectContext.SaveChanges();
            }
            return RedirectToAction("Detail", new { id });
        }
        [MyAuthorizeAttribute(Roles = "Super Admin,Admin", RegionRoles = "RQCO", InputType = InputType.NC)]
        public ActionResult Reaffect(string id, string agenceConcernee_Id)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationNonConformite(id);
                var agence = dal.FindAgence(agenceConcernee_Id);
                if (agence != null)
                {
                    declaration.AgenceConcernée_Id = agenceConcernee_Id;
                    dal.ObjectContext.SaveChanges();
                }
            }
            return RedirectToAction("Detail", new { id });
        }

        private void SendEmail(string href, string declarationId)
        {
            var recipients = new List<RoleEmailAddress>();
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationNonConformite(declarationId);
                var origine = declaration.AgenceConcernée;
                var destination = declaration.Agence;

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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
    }

    public class RoleEmailAddress : EmailAddress
    {
        public RoleEmailAddress(string name, string email, string role) : base(name, email)
        {
            Role = role;
        }

        public RoleEmailAddress(string email, string role) : base(email)
        {
            Role = role;
        }

        public string Role { get; set; }
    }
}