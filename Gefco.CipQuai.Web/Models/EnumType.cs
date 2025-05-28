using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gefco.CipQuai.Web.Models
{
    public class EnumType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}