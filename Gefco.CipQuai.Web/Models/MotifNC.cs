using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class MotifNC : NameBaseModel
    {
        public bool IsOther { get; set; }
        public int DisplayOrder { get; set; }
        [JsonIgnore]
        public virtual IList<DeclarationNonConformite> DeclarationNonConformites { get; set; }

        public string Color { get; set; }

        public override BaseModel Clone()
        {
            return new MotifNC()
            {
                Id = Id,
                CreationDate = CreationDate,
                Name = Name,
                DisplayOrder = DisplayOrder,
                IsOther = IsOther
            };
        }
    }
}