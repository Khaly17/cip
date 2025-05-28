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
    public class PagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pages
        public ActionResult Index()
        {
            return View(db.Pages.OrderBy(p => p.Section.SortOrder).ThenBy(p => p.SortOrder).ThenBy(p => p.SortOrder).ToList());
        }

        // GET: Pages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Page page)
        {
            ModelState.Clear();
            page.Id = Guid.NewGuid().ToString();
            page.CreationDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                db.Pages.Add(page);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Created);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        // GET: Pages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            var dbSections = db.Sections.ToList();
            dbSections.Insert(0, new Section { Name = "--- None ---", Id = Guid.Empty.ToString() });
            ViewBag.SectionId = new SelectList(dbSections, "Id", "Name", Guid.Empty.ToString());

            return View(new PageViewModel(page));
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PageViewModel page)
        {
            var dbSections = db.Sections.ToList();
            dbSections.Insert(0, new Section { Name = "--- None ---", Id = Guid.Empty.ToString() });
            ViewBag.SectionId = new SelectList(dbSections, "Id", "Name", Guid.Empty.ToString());
            if (ModelState.IsValid)
            {
                var dbItem = db.Pages.Find(page.Id);
                page.UpdateModel(dbItem);
                if (page.SectionId != Guid.Empty)
                    dbItem.Section = db.Sections.Find(page.SectionId.ToString());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var page = db.Pages.Find(id);
            if (page != null)
                db.Pages.Remove(page);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleRole(string pageId, string roleId, bool isInRole)
        {
            var item = db.Pages.Find(pageId);
            var role = db.Roles.Find(roleId);
            if (item == null || role == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            if (isInRole)
            {
                item.Roles.Add(new PageRole(item, role) { Id = Guid.NewGuid().ToString(), CreationDate = DateTime.UtcNow });
            }
            else
            {
                var pageRole = item.Roles.SingleOrDefault(p => p.Page.Id == pageId && p.Role.Id == roleId);
                if (pageRole != null)
                    item.Roles.Remove(pageRole);
            }
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
    }
}
