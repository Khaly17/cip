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
    public class MotifsNCController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            IQueryable<MotifNC> entities = db.MotifNCs;
            if (!User.IsInRole("Super Admin"))
                entities = entities.Where(p => !p.IsDeleted);
            return View(entities.OrderBy(p => p.Name));
        }
        public ActionResult IndexData(DataManagerRequest dm)
        {
            var entities = db.MotifNCs.AsQueryable();
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
            MotifNC MotifNC = db.MotifNCs.Find(id);
            if (MotifNC == null)
            {
                return HttpNotFound();
            }
            return View(MotifNC);
        }

        [HttpPost]
        public ActionResult Create(MotifNC MotifNC)
        {
            ModelState.Clear();
            MotifNC.Id = Guid.NewGuid().ToString();
            MotifNC.CreationDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                db.MotifNCs.Add(MotifNC);
                db.SaveChanges();
                return Json(MotifNC.Id);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotifNC MotifNC = db.MotifNCs.Find(id);
            if (MotifNC == null)
            {
                return HttpNotFound();
            }
            return View(MotifNC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MotifNC model)
        {
            if (ModelState.IsValid)
            {
                var dbItem = db.MotifNCs.Find(model.Id);
                dbItem.DisplayOrder = model.DisplayOrder;
                dbItem.IsOther = model.IsOther;
                dbItem.Name = model.Name;
                dbItem.Color = model.Color;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // POST: MotifsNC/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var entity = db.MotifNCs.Find(id);
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
