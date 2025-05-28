using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class Page : NameBaseModel
    {
        public Page()
        {
            Roles = new List<PageRole>();
        }

        public virtual List<PageRole> Roles { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual string Icon { get; set; }
        public virtual string Link { get; set; }
        public virtual string MenuTag { get; set; }
        [ForeignKey("Section_Id")]
        public virtual Section Section { get; set; }
        [JsonIgnore]
        public string Section_Id { get; set; }

        public override BaseModel Clone()
        {
            var obj = new Page
            {
                Id = Id,
                CreationDate = CreationDate,
                Roles = Roles?.Select(p => (PageRole)p.Clone()).ToList(),
                Icon = Icon,
                Link = Link,
                MenuTag = MenuTag,
                Name = Name,
                Section = (Section) Section.Clone(),
                SortOrder = SortOrder
            };
            return obj;
        }

    }
}