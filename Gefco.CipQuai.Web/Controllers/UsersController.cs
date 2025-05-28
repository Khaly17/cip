using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Gefco.CipQuai.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Exchange.WebServices.Data;
using Syncfusion.EJ2.Base;
using WebGrease.Css.Extensions;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexData(DataManagerRequest dm)
        {
            IEnumerable<ApplicationUserViewModel> DataSource;
            if (User.IsInRole("Admin"))
            {
                var role = db.Roles.SingleOrDefault(p => p.Name == "Super Admin");
                DataSource = db.Users.Where(p => p.Roles.All(q => q.RoleId != role.Id)).ToList().Select(p => new ApplicationUserViewModel(p));
            }
            else
            {
                DataSource = db.Users.ToList().Select(p => new ApplicationUserViewModel(p));
            }
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

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(applicationUser);
                    db.SaveChanges();
                    return Json(applicationUser.Id);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["MobileUserAgence_Id"] = db.Agences.Where(p => p.AgenceType.Value == "Gefco France").OrderBy(p => p.Name).Select(p => new SelectListItem(){ Value = p.Id, Text=p.Name}).ToList();
            ViewData["WebUserAgence_Id"] = db.Agences.Where(p => p.AgenceType.Value == "Gefco France").OrderBy(p => p.Name).Select(p => new SelectListItem(){ Value = p.Id, Text=p.Name}).ToList();
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(new ApplicationUserViewModel(applicationUser));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = db.Users.SingleOrDefault(p => p.Id == viewModel.UserId);
                if (applicationUser != null)
                {
                    viewModel.UpdateModel(applicationUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewData["MobileUserAgence_Id"] = db.Agences.Where(p => p.AgenceType.Value == "Gefco France").OrderBy(p => p.Name).Select(p => new SelectListItem() { Value = p.Id, Text = p.Name }).ToList();
            ViewData["WebUserAgence_Id"] = db.Agences.Where(p => p.AgenceType.Value == "Gefco France").OrderBy(p => p.Name).Select(p => new SelectListItem() { Value = p.Id, Text = p.Name }).ToList();
            return View(viewModel);
        }


        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var entity = db.Users.Find(id);
            entity.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            private set => _userManager = value;
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ApplicationUser admin = db.Users.Find(User.Identity.GetUserId());
            if (user == null || admin == null)
                return Json(new { isSuccess = false});
            if (user.SecurityStamp.IsNullOrWhiteSpace())
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var newCode = Tools.GenerateRandomNumber(4);

            var result = await UserManager.ResetPasswordAsync(user.Id, code, newCode);
            if (result.Succeeded)
            using (var dal = new Dal())
            {
                user = dal.FindUser(id);
                user.NeedsChangePin = true;
                await dal.ObjectContext.SaveChangesAsync();
                var template = dal.FindEmailTemplate("WebAdminSetPassword");
                if (template != null)
                    Instances.SendEmail(template.Object, ReplaceTemplate(), new EmailAddress(user.Email));

                string ReplaceTemplate()
                {
                    var body = template.Content;
                    body = body.Replace("@@FullName@@", user.FirstName + " " + user.LastName);
                    body = body.Replace("@@AdminEmail@@", admin.Email);
                    body = body.Replace("@@CodePIN@@", newCode);
                    return body;
                }
                return Json(new { isSuccess = true });
            }

            return Json(new { isSuccess = false });
        }

        [HttpPost]
        public ActionResult ToggleRole(string userId, string roleId, bool isInRole)
        {
            ApplicationUser user = db.Users.Find(userId);
            IdentityRole role = db.Roles.Find(roleId);
            if (user == null || role == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            if (isInRole)
            {
                user.Roles.Add(new IdentityUserRole() { UserId = userId, RoleId = roleId });
            }
            else
            {
                var userRole = user.Roles.SingleOrDefault(p => p.UserId == userId && p.RoleId == roleId);
                if (userRole != null)
                    user.Roles.Remove(userRole);
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
