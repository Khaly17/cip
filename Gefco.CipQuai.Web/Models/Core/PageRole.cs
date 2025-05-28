using Microsoft.AspNet.Identity.EntityFramework;

namespace Gefco.CipQuai.Web.Models
{
    public class PageRole : BaseModel
    {
        public PageRole()
        {

        }
        public PageRole(Page page, IdentityRole role)
        {
            Page = page;
            Role = role;
        }

        public virtual IdentityRole Role { get; set; }

        public virtual Page Page { get; set; }

        public override BaseModel Clone()
        {
            var obj = new PageRole
            {
                Id = Id,
                CreationDate = CreationDate,
                Page = Page,
                Role = Role
            };
            return obj;
        }
    }
}