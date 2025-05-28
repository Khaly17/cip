using System.ComponentModel.DataAnnotations;

namespace Gefco.CipQuai.Web.Models
{
    public abstract class NameBaseModel : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }
    }
}