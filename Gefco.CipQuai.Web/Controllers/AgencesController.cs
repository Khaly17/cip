using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Models;
using Syncfusion.EJ2.Base;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    public class AgencesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? agenceType)
        {
            ViewBag.Regions = db.Regions.ToList();
            IEnumerable<Agence> entities = db.Agences;
            if (!User.IsInRole("Super Admin"))
                entities = entities.Where(p => !p.IsDeleted);
            if (agenceType != null)
            {
                entities = entities.Where(p => p.AgenceType_Id == agenceType);
                ViewBag.AgenceType = agenceType;
            }
            return View(entities.OrderBy(p => p.Region?.Name).ThenBy(p => p.Name).ToList().Select(p => new AgenceViewModel(p)));
        }
        public ActionResult IndexData(DataManagerRequest dm, int? agenceType)
        {
            var entities = db.Agences.AsQueryable();
            if (!User.IsInRole("Super Admin"))
                entities = entities.Where(p => !p.IsDeleted);
            if (agenceType != null)
                entities = entities.Where(p => p.AgenceType_Id == agenceType);
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
            return dm.RequiresCounts ? Json(new { result = entities.ToList().Select(p => new AgenceViewModel(p)), count = count }, JsonRequestBehavior.AllowGet) : Json(entities.ToList().Select(p => new AgenceViewModel(p)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agence Agence = db.Agences.Find(id);
            if (Agence == null)
            {
                return HttpNotFound();
            }
            return View(Agence);
        }

        [HttpPost]
        public ActionResult Create(Agence agence)
        {
            ModelState.Clear();
            agence.Id = Guid.NewGuid().ToString();
            agence.CreationDate = DateTime.UtcNow;
            agence.AgenceType_Id = 1;
            if (ModelState.IsValid)
            {
                db.Agences.Add(agence);
                db.SaveChanges();
                return Json(agence.Id);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agence Agence = db.Agences.Find(id);
            if (Agence == null)
            {
                return HttpNotFound();
            }
            var dbAgenceTypes = db.AgenceTypes.ToList();
            dbAgenceTypes.Insert(0, new AgenceType { Value = "--- Aucune ---", Key = -1 });
            ViewBag.AgenceType_Id = new SelectList(dbAgenceTypes, "Key", "Value", -1);
            var dbRegions = db.Regions.ToList();
            dbRegions.Insert(0, new Region { Name = "--- Autre ---", Id = Guid.Empty.ToString() });
            ViewBag.Region_Id = new SelectList(dbRegions, "Id", "Name", Guid.Empty.ToString());

            return View(new AgenceViewModel(Agence));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AgenceViewModel agence)
        {
            if (ModelState.IsValid)
            {
                var dbItem = db.Agences.Find(agence.Id);
                agence.UpdateModel(dbItem);
                if (agence.AgenceType_Id != -1)
                    dbItem.AgenceType_Id = agence.AgenceType_Id;
                if (agence.Region_Id != Guid.Empty.ToString())
                    dbItem.Region_Id = agence.Region_Id;
                else
                    dbItem.Region_Id = null;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var dbAgenceTypes = db.AgenceTypes.ToList();
            dbAgenceTypes.Insert(0, new AgenceType { Value = "--- Aucune ---", Key = -1 });
            ViewBag.AgenceType_Id = new SelectList(dbAgenceTypes, "Key", "Value", -1);
            var dbRegions = db.Regions.ToList();
            dbRegions.Insert(0, new Region { Name = "--- Autre ---", Id = Guid.Empty.ToString() });
            ViewBag.Region_Id = new SelectList(dbRegions, "Id", "Name", Guid.Empty.ToString());
            return View(agence);
        }

        // POST: Agences/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var entity = db.Agences.Find(id);
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
