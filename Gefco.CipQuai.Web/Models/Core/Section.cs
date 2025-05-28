using System.Collections.Generic;
using System.Linq;

namespace Gefco.CipQuai.Web.Models
{
    public class Section : NameBaseModel
    {
        public int SortOrder { get; set; }
        public virtual List<Page> Pages { get; set; }

        public void UpdateModel(Section dbItem)
        {
            dbItem.Name = Name;
            dbItem.SortOrder = SortOrder;
        }

        public Section()
        {
            Pages = new List<Page>();
        }

        public override BaseModel Clone()
        {
            var obj = new Section
            {
                Id = Id,
                CreationDate = CreationDate,
                Name = Name,
                SortOrder = SortOrder,
                Pages = Pages?.Select(p => (Page) p.Clone()).ToList()
            };
            return obj;
        }

    }
}