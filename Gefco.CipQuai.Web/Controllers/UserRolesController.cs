using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.ViewModels;
using Syncfusion.EJ2.Base;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    public class UserRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.RegionRoles.OrderBy(p => p.Value).ToList().Select(p => new BusinessRoleViewModel(p)).Union(db.AgenceRoles.OrderBy(p => p.Value).ToList().Select(p => new BusinessRoleViewModel(p))).Union(db.NationalRoles.OrderBy(p => p.Value).ToList().Select(p => new BusinessRoleViewModel(p))));
        }
        public ActionResult IndexData(DataManagerRequest dm)
        {
            var DataSource = db.RegionRoles.OrderBy(p => p.Value).ToList().Select(p => new BusinessRoleViewModel(p)).Union(db.AgenceRoles.OrderBy(p => p.Value).ToList().Select(p => new BusinessRoleViewModel(p))).Union(db.NationalRoles.OrderBy(p => p.Value).ToList().Select(p => new BusinessRoleViewModel(p)));

            DataOperations operation = new DataOperations();
            List<string> str = new List<string>();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);         //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource.ToList(), count = count }, JsonRequestBehavior.AllowGet) : Json(DataSource.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateAgenceRole(AgenceRole item)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                db.AgenceRoles.Add(item);
                db.SaveChanges();
                return Json(item.Key);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public ActionResult CreateNationalRole(NationalRole item)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                db.NationalRoles.Add(item);
                db.SaveChanges();
                return Json(item.Key);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public ActionResult CreateRegionRole(RegionRole item)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                db.RegionRoles.Add(item);
                db.SaveChanges();
                return Json(item.Key);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessRole BusinessRole = db.BusinessRoles.Find(id);
            if (BusinessRole == null)
            {
                return HttpNotFound();
            }
            var users = db.Users.ToList().Select(p => new { Id = p.Id, Name = p.FirstName + " " + p.LastName }).OrderBy(p => p.Name).ToList();
            ViewBag.User_Id = new SelectList(users, "Id", "Name");
            if (BusinessRole is RegionRole)
            {
                var regions = db.Regions.OrderBy(p => p.Name).CloneList();
                ViewBag.Region_Id = new SelectList(regions, "Id", "Name");
            }
            if (BusinessRole is AgenceRole)
            {
                var agences = db.Agences.Where(p => p.AgenceType.Value == "GEFCO France").OrderBy(p => p.Name).CloneList();
                ViewBag.Agence_Id = new SelectList(agences, "Id", "Name");
            }
            return View(BusinessRole);
        }

        public ActionResult UserNationalRoleData(DataManagerRequest dm, int roleId)
        {
            var DataSource = db.UserNationalRoles.Where(p => p.NationalRole_Id == roleId).OrderBy(p => p.User.FirstName).ToList().Select(p => new UserNationalRole(){ Id = p.Id, NationalRole_Id = p.NationalRole_Id, User_Id = p.User_Id, User = p.User.Clone() });

            DataOperations operation = new DataOperations();
            List<string> str = new List<string>();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);         //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource.ToList(), count = count }, JsonRequestBehavior.AllowGet) : Json(DataSource.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserRegionRoleData(DataManagerRequest dm, int roleId)
        {
            var DataSource = db.UserRegionRoles.Where(p => p.RegionRole_Id == roleId).ToList().OrderBy(p => p.Region.Name).ThenBy(p => p.User.FirstName).ToList().Select(p => new UserRegionRole(){ Id = p.Id, Region_Id = p.Region_Id, RegionRole_Id = p.RegionRole_Id, User_Id = p.User_Id, Region = (Region) p.Region.Clone(), User = p.User.Clone() });

            DataOperations operation = new DataOperations();
            List<string> str = new List<string>();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);         //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource.ToList(), count = count }, JsonRequestBehavior.AllowGet) : Json(DataSource.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserAgenceRoleData(DataManagerRequest dm, int roleId)
        {
            var DataSource = db.UserAgenceRoles.Where(p => p.AgenceRole_Id == roleId).ToList().OrderBy(p => p.Agence.Name).ThenBy(p => p.User.FirstName).ToList().Select(p => new UserAgenceRole(){ Id = p.Id, Agence_Id = p.Agence_Id, AgenceRole_Id = p.AgenceRole_Id, User_Id = p.User_Id, Agence = (Agence) p.Agence.Clone(), User = p.User.Clone() });

            DataOperations operation = new DataOperations();
            List<string> str = new List<string>();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);         //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource.ToList(), count = count }, JsonRequestBehavior.AllowGet) : Json(DataSource.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BusinessRole model)
        {
            if (ModelState.IsValid)
            {
                var dbItem = db.BusinessRoles.Find(model.Key);
                dbItem.Value = model.Value;
                dbItem.Description = model.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var users = db.Users.ToList().Select(p => new { Id = p.Id, Name = p.FirstName + " " + p.LastName }).ToList();
            ViewBag.User_Id = new SelectList(users, "Id", "Name");
            if (model is RegionRole)
            {
                var regions = db.Regions.CloneList();
                ViewBag.Region_Id = new SelectList(regions, "Id", "Name");
            }
            if (model is AgenceRole)
            {
                var agences = db.Agences.CloneList();
                ViewBag.Agence_Id = new SelectList(agences, "Id", "Name");
            }
            return View(model);
        }

        [HttpPost]
        public HttpStatusCodeResult AddUserNationalRole(UserNationalRole model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                db.UserNationalRoles.AddOrUpdate(model);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(500);
        }
        [HttpPost]
        public HttpStatusCodeResult AddUserRegionRole(UserRegionRole model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                db.UserRegionRoles.AddOrUpdate(model);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(500);
        }
        [HttpPost]
        public HttpStatusCodeResult AddUserAgenceRole(UserAgenceRole model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                db.UserAgenceRoles.AddOrUpdate(model);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(500);
        }



        // POST: BusinessRoles/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            BusinessRole BusinessRole = db.BusinessRoles.Find(id);
            if (BusinessRole != null)
            {
                //db.BusinessRoles.Remove(BusinessRole);
                db.BusinessRoles.Remove(BusinessRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: BusinessRoles/Delete/5
        [HttpPost]
        public ActionResult DeleteUserAgenceRole(string id)
        {
            var role = db.UserAgenceRoles.Find(id);
            if (role != null)
            {
                db.UserAgenceRoles.Remove(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult DeleteUserRegionRole(string id)
        {
            var role = db.UserRegionRoles.Find(id);
            if (role != null)
            {
                db.UserRegionRoles.Remove(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult DeleteUserNationalRole(string id)
        {
            var role = db.UserNationalRoles.Find(id);
            if (role != null)
            {
                db.UserNationalRoles.Remove(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
