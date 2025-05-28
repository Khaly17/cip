using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool NeedsChangePin { get; set; }
        [JsonIgnore]
        public string AppVersion { get; set; }
        [JsonIgnore]
        public string MobileUserAgence_Id { get; set; }

        [ForeignKey("MobileUserAgence_Id")]
        public virtual Agence MobileUserAgence { get; set; }
        [JsonIgnore]
        public string WebUserAgence_Id { get; set; }

        [ForeignKey("WebUserAgence_Id")]
        [JsonIgnore]
        public virtual Agence WebUserAgence { get; set; }
        [JsonIgnore]
        public string ProfilePicture_Id { get; set; }

        [ForeignKey("ProfilePicture_Id")]
        public virtual Picture ProfilePicture { get; set; }
        public virtual List<Resource> Resources { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser Clone()
        {
            var obj = new ApplicationUser
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Id = Id,
                NeedsChangePin = NeedsChangePin,
            };
            obj.MobileUserAgence = (Agence) MobileUserAgence?.Clone();
            obj.MobileUserAgence_Id = MobileUserAgence?.Id;
            obj.ProfilePicture = (Picture)ProfilePicture?.Clone();
            return obj;
        }

        public virtual List<UserAgenceRole> AgenceRoles { get; set; }
        public virtual List<UserRegionRole> RegionRoles { get; set; }
        public virtual List<UserNationalRole> NationalRoles { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        [JsonIgnore]
        public string FullName => FirstName + " " + LastName;
    }

    public class UserNationalRole
    {
        public UserNationalRole()
        {
            
        }
        public UserNationalRole(NationalRole role)
        {
            NationalRole_Id = role.Key;
        }

        public string Id { get; set; }
        public string User_Id { get; set; }
        public int NationalRole_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(NationalRole_Id))]
        public virtual NationalRole NationalRole { get; set; }
    }
    public class UserRegionRole
    {
        public UserRegionRole()
        {
            
        }
        public UserRegionRole(RegionRole role)
        {
            RegionRole_Id = role.Key;
        }

        public string Id { get; set; }
        public string User_Id { get; set; }
        public int RegionRole_Id { get; set; }
        public string Region_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey(nameof(RegionRole_Id))]
        public virtual RegionRole RegionRole { get; set; }
        [ForeignKey(nameof(Region_Id))]
        public virtual Region Region { get; set; }
    }
    public class UserAgenceRole
    {
        public UserAgenceRole()
        {
            
        }
        public UserAgenceRole(AgenceRole role)
        {
            AgenceRole_Id = role.Key;
        }

        public string Id { get; set; }
        public string User_Id { get; set; }
        public int AgenceRole_Id { get; set; }
        public string Agence_Id { get; set; }
        [ForeignKey(nameof(User_Id))]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey(nameof(AgenceRole_Id))]
        public virtual AgenceRole AgenceRole { get; set; }
        [ForeignKey(nameof(Agence_Id))]
        public virtual Agence Agence { get; set; }

    }
    public class NationalRole : BusinessRole
    {
    }
    public class RegionRole : BusinessRole
    {
    }
    public class AgenceRole : BusinessRole
    {
    }
}