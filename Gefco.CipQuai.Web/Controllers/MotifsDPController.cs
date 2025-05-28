using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Syncfusion.EJ2.Base;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    public class MotifsDPController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            IQueryable<MotifDP> entities = db.MotifDPs;
            if (!User.IsInRole("Super Admin"))
                entities = entities.Where(p => !p.IsDeleted);
            return View(entities.OrderBy(p => p.Name));
        }
        public ActionResult IndexData(DataManagerRequest dm)
        {
            var entities = db.MotifDPs.AsQueryable();
            if (!User.IsInRole("Super Admin"))
                entities = entities.Where(p => !p.IsDeleted);

            DataOperations operation = new DataOperations();
            List<string> str = new List<string>();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                entities = operation.PerformSearching(entities, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                entities = operation.PerformSorting(entities, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                entities = operation.PerformFiltering(entities, dm.Where, dm.Where[0].Operator);
            }
            int count = entities.Count();
            if (dm.Skip != 0)
            {
                entities = operation.PerformSkip(entities, dm.Skip);         //Paging
            }
            if (dm.Take != 0)
            {
                entities = operation.PerformTake(entities, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = entities.CloneList(), count = count }, JsonRequestBehavior.AllowGet) : Json(entities.CloneList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotifDP MotifDP = db.MotifDPs.Find(id);
            if (MotifDP == null)
            {
                return HttpNotFound();
            }
            return View(MotifDP);
        }

        [HttpPost]
        public ActionResult Create(MotifDP MotifDP)
        {
            ModelState.Clear();
            MotifDP.Id = Guid.NewGuid().ToString();
            MotifDP.CreationDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                db.MotifDPs.Add(MotifDP);
                db.SaveChanges();
                return Json(MotifDP.Id);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotifDP MotifDP = db.MotifDPs.Find(id);
            if (MotifDP == null)
            {
                return HttpNotFound();
            }
            return View(MotifDP);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MotifDP model)
        {
            if (ModelState.IsValid)
            {
                var dbItem = db.MotifDPs.Find(model.Id);
                dbItem.DisplayOrder = model.DisplayOrder;
                dbItem.IsNbDP = model.IsNbDP;
                dbItem.IsOther = model.IsOther;
                dbItem.NeedPicture = model.NeedPicture;
                dbItem.Name = model.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // POST: MotifsDP/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var entity = db.MotifDPs.Find(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
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
