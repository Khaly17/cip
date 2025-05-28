using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gefco.CipQuai.Web.Models
{
    public class IdentityRoleViewModel
    {
        public string RoleId { get; set; }
        [Display(Name = "Role name")]
        public string Name { get; set; }
        [Display(Name = "Users")]
        public int Users { get; set; }
        public IdentityRoleViewModel()
        {

        }
        public IdentityRoleViewModel(IdentityRole role)
        {
            RoleId = role.Id;
            Name = role.Name;
            Users = role.Users.Count;
        }

        public void UpdateModel(IdentityRole identityRole)
        {
            identityRole.Name = Name;
        }
    }
}