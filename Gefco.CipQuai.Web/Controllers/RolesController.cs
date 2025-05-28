using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Gefco.CipQuai.Web.Models;
using Syncfusion.EJ2.Base;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles
        public ActionResult Index()
        {
            var entities = db.Roles;
            return View(entities.ToList().Select(p => new IdentityRoleViewModel(p)));
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")] IdentityRole identityRole)
        {
            if (ModelState.IsValid)
            {
                identityRole.Id = Guid.NewGuid().ToString();
                db.Roles.Add(identityRole);
                db.SaveChanges();
                return Json(identityRole.Id);
            }

            return View(identityRole);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole identityRole = db.Roles.Find(id);
            if (identityRole == null)
            {
                return HttpNotFound();
            }
            return View(new IdentityRoleViewModel(identityRole));
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var identityRole = db.Roles.SingleOrDefault(p => p.Id == viewModel.RoleId);
                if (identityRole != null)
                {
                    viewModel.UpdateModel(identityRole);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(viewModel);
        }


        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var entity = db.Roles.Find(id);
            db.Roles.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult IndexData(DataManagerRequest dm)
        {
            var DataSource = db.Roles.ToList().Select(p => new IdentityRoleViewModel(p));

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
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }, JsonRequestBehavior.AllowGet) : Json(DataSource, JsonRequestBehavior.AllowGet);
        }
    }
}