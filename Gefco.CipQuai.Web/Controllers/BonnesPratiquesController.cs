using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Views;
using Microsoft.AspNet.Identity;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize]
    public class BonnesPratiquesController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public BonnesPratiquesController()
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
            var declaration = _ctx.DeclarationBonnePratiques.SingleOrDefault(p => p.Id == id);
            var tempPictures = _ctx.TempPictures.Where(p => p.DeclarationBonnePratique_Id == id).ToList();
            var viewModel = new DetailBPViewModel(declaration, tempPictures);
            if (declaration.Pictures != null)
                foreach (var picture in declaration.Pictures)
                {
                    if (System.IO.File.Exists(picture.PicturePath))
                        picture.PicturePath = Url.Action("GetPicture", "Pictures", new { id = picture.Id });
                }
            ViewBag.rUrl = rUrl;
            return View(viewModel);
        }
    }
}