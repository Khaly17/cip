using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Super Admin") || User.IsInRole("Admin") || User.IsInRole("National Users"))
                return RedirectToAction("Index", "NationalDashboard");
            if (User.IsInRole("Regional Users"))
            {
                using (var dal = new Dal())
                {
                    var usr = dal.FindUser(User.Identity.GetUserId());
                    if (usr.WebUserAgence?.Region_Id != null)
                        return RedirectToAction("Index", "RegionDashboard", new RouteValueDictionary(){ { "id", usr.WebUserAgence.Region_Id } });
                }
            }
            if (User.IsInRole("Agence Users"))
            {
                using (var dal = new Dal())
                {
                    var usr = dal.FindUser(User.Identity.GetUserId());
                    if (usr.WebUserAgence_Id != null)
                        return RedirectToAction("Index", "AgenceDashboard", new RouteValueDictionary(){ { "id", usr.WebUserAgence_Id } });
                }
            }
            return View();
        }
        [Authorize(Roles = "Super Admin")]
        public ActionResult Dashboard()
        {
            return View();
        }
        [Authorize(Roles = "Super Admin")]
        public ActionResult DashboardNational()
        {
            return View();
        }
        [Authorize(Roles = "Super Admin")]
        public ActionResult DashboardCrm()
        {
            return View();
        }
        [Authorize(Roles = "Super Admin")]
        public ActionResult DashboardAnalytics()
        {
            return View();
        }
        [Authorize(Roles = "Super Admin")]
        public ActionResult Widgets()
        {
            return View();
        }


    }
}