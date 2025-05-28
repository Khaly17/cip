using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Syncfusion.EJ2.Base;

namespace Gefco.CipQuai.Web.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Super Admin,Admin")]
    public class TractionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            IQueryable<TractionDefinition> entities = db.TractionDefinitions;
            if (!User.IsInRole("Super Admin"))
                entities = entities.Where(p => !p.IsDeleted);
            return View(entities.OrderBy(p => p.Name).ThenBy(p => p.AgenceDepart.Name).ThenBy(p => p.AgenceArrivee.Name).ToList().Select(p => new TractionViewModel(p)));
        }
        public ActionResult AnnulationTractions(DateTime startDate, DateTime endDate)
        {
            var entities = db.TractionDefinitions.OrderBy(p => p.Name).ThenBy(p => p.AgenceDepart.Name).ThenBy(p => p.AgenceArrivee.Name).ToList();
            return View(new JoursFeriesViewModel(entities, startDate, endDate));
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult AnnulationTractions(string selection, string type, DateTime startDate, DateTime endDate)
        {
            var ids = selection.Split(',').ToList();
            foreach (var id in ids)
            {
                var definition = db.TractionDefinitions.SingleOrDefault(p => p.Id == id);
                if (definition == null)
                    continue;
                var start = startDate.Date;
                var end = endDate.Date.AddDays(1);
                while (start < end)
                {
                    var traction = db.Tractions.SingleOrDefault(p => p.TractionDefinition.Id == id && p.DueDate == start);
                    if (traction != null)
                    {
                        traction.IsCancelled = type == "Cancel";
                        traction.IsDeleted = type == "Cancel";
                        traction.CancelReason = type == "Cancel" ? "Traction annulée" : null;
                    }
                    else
                    {
                        if (type == "Cancel" && definition.DaysOfWeek.Contains((Days)start.DayOfWeek))
                        {
                            traction = new Traction()
                            {
                                AgenceDepart = definition.AgenceDepart,
                                AgenceArrivee = definition.AgenceArrivee,
                                //CreatedBy = user,
                                TractionDefinition = definition,
                                Id = Guid.NewGuid().ToString(),
                                Name = definition.Name,
                                IsDeleted = true,
                                IsCancelled = true,
                                CreationDate = DateTime.UtcNow,
                                DueDate = start,
                                CancelReason = "Traction annulée"
                            };
                            db.Tractions.Add(traction);
                        }

                    }
                    start = start.AddDays(1);
                }
            }
            db.SaveChanges();
            var entities = db.TractionDefinitions.OrderBy(p => p.Name).ThenBy(p => p.AgenceDepart.Name).ThenBy(p => p.AgenceArrivee.Name).ToList();
            var model = new JoursFeriesViewModel(entities, startDate, endDate);
            return View(model);
        }
        public ActionResult IndexData(DataManagerRequest dm)
        {
            IQueryable<TractionDefinition> set = db.TractionDefinitions.CloneList().AsQueryable();
            if (!User.IsInRole("Super Admin"))
                set = set.Where(p => !p.IsDeleted);
            var entities = set.ToList().Select(p => new TractionViewModel(p));

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
            return dm.RequiresCounts ? Json(new { result = entities, count = count }, JsonRequestBehavior.AllowGet) : Json(entities, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TractionDefinition model = db.TractionDefinitions.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Create(TractionDefinition model)
        {
            ModelState.Clear();
            model.Id = Guid.NewGuid().ToString();
            model.CreationDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                db.TractionDefinitions.Add(model);
                db.SaveChanges();
                return Json(model.Id);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TractionDefinition model = db.TractionDefinitions.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var agencesDepart = db.Agences.Where(p => p.IsStart).OrderBy(p => p.Name).ToList();
            agencesDepart.Insert(0, new Agence { Name = "--- Aucune ---", Id = Guid.Empty.ToString() });
            ViewBag.AgenceDepart_Id = new SelectList(agencesDepart, "Id", "Name", Guid.Empty.ToString());
            var agencesArrivee = db.Agences.Where(p => p.IsEnd).OrderBy(p => p.Name).ToList();
            agencesArrivee.Insert(0, new Agence { Name = "--- Aucune ---", Id = Guid.Empty.ToString() });
            ViewBag.AgenceArrivee_Id = new SelectList(agencesArrivee, "Id", "Name", Guid.Empty.ToString());

            return View(new TractionViewModel(model));
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TractionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dbItem = db.TractionDefinitions.Find(model.Id);
                model.UpdateModel(dbItem);
                if (model.AgenceDepart_Id != Guid.Empty.ToString())
                    dbItem.AgenceDepart = db.Agences.Find(model.AgenceDepart_Id);
                else
                    dbItem.AgenceDepart = null;
                if (model.AgenceArrivee_Id != Guid.Empty.ToString())
                    dbItem.AgenceArrivee = db.Agences.Find(model.AgenceArrivee_Id);
                else
                    dbItem.AgenceArrivee = null;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var agencesDepart = db.Agences.Where(p => p.IsStart).OrderBy(p => p.Name).ToList();
            agencesDepart.Insert(0, new Agence { Name = "--- Aucune ---", Id = Guid.Empty.ToString() });
            ViewBag.AgenceDepart_Id = new SelectList(agencesDepart, "Id", "Name", Guid.Empty.ToString());
            var agencesArrivee = db.Agences.Where(p => p.IsEnd).OrderBy(p => p.Name).ToList();
            agencesArrivee.Insert(0, new Agence { Name = "--- Aucune ---", Id = Guid.Empty.ToString() });
            ViewBag.AgenceArrivee_Id = new SelectList(agencesArrivee, "Id", "Name", Guid.Empty.ToString());
            return View(model);
        }

        // POST: Tractions/Delete/5
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var entity = db.TractionDefinitions.Find(id);
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
