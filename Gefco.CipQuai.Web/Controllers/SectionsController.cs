using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class SectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sections
        public ActionResult Index()
        {
            IQueryable<Section> entities = db.Sections;
            if (!User.IsInRole("Super Admin"))
                entities = entities.Where(p => !p.IsDeleted);
            return View(entities.OrderBy(p => p.SortOrder).ToList());
        }

        // GET: Sections/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section Section = db.Sections.Find(id);
            if (Section == null)
            {
                return HttpNotFound();
            }
            return View(Section);
        }

        // POST: Sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Section section)
        {
            ModelState.Clear();
            section.Id = Guid.NewGuid().ToString();
            section.CreationDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Created);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section Section = db.Sections.Find(id);
            if (Section == null)
            {
                return HttpNotFound();
            }
            return View(Section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Section section)
        {
            if (ModelState.IsValid)
            {
                var dbItem = db.Sections.Find(section.Id);
                section.UpdateModel(dbItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var entity = db.Sections.Find(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                db.SaveChanges();
            }
            
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
    }
}
