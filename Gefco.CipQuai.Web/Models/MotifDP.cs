using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class MotifDP : NameBaseModel
    {
        public int DisplayOrder { get; set; }
        public bool NeedPicture { get; set; }
        public bool IsNbDP { get; set; }
        public bool IsOther { get; set; }
        public override BaseModel Clone()
        {
            return new MotifDP()
            {
                Id = Id,
                CreationDate = CreationDate,
                Name = Name,
                DisplayOrder = DisplayOrder,
                NeedPicture = NeedPicture,
                IsNbDP = IsNbDP,
                IsOther = IsOther
            };
        }

        [JsonIgnore]
        public virtual IList<DeclarationDoublePlancher> DeclarationDoublePlanchers { get; set; }
    }
}