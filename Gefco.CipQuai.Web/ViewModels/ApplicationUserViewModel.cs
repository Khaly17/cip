using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gefco.CipQuai.Web.Models
{
    public class ApplicationUserViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AppVersion { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Access failed count")]
        public int AccessFailedCount { get; set; }
        [Display(Name = "Agence Mobile")]
        public string MobileUserAgence { get; set; }
        public string MobileUserAgence_Id { get; set; }
        [Display(Name = "Agence Web")]
        public string WebUserAgence { get; set; }
        public string WebUserAgence_Id { get; set; }

        public IEnumerable<string> Roles { get; } = new List<string>();

        public ApplicationUserViewModel()
        {

        }
        public ApplicationUserViewModel(ApplicationUser user)
        {
            UserId = user.Id;
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            AccessFailedCount = user.AccessFailedCount;
            Roles = user.Roles?.Select(p => p.RoleId);
            MobileUserAgence = user.MobileUserAgence?.Name;
            MobileUserAgence_Id = user.MobileUserAgence_Id;
            WebUserAgence = user.WebUserAgence?.Name;
            WebUserAgence_Id = user.WebUserAgence_Id;
            AppVersion = user.AppVersion;
        }

        public void UpdateModel(ApplicationUser applicationUser)
        {
            applicationUser.UserName = UserName;
            applicationUser.FirstName = FirstName;
            applicationUser.LastName = LastName;
            applicationUser.Email = Email;
            applicationUser.PhoneNumber = PhoneNumber;
            applicationUser.AccessFailedCount = AccessFailedCount;
            applicationUser.MobileUserAgence_Id = MobileUserAgence_Id;
            applicationUser.WebUserAgence_Id = WebUserAgence_Id;
            applicationUser.AppVersion = AppVersion;
        }
    }
}