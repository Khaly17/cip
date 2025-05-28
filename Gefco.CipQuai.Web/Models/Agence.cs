using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class Agence : NameBaseModel
    {
        [JsonIgnore]
        public bool IsStart { get; set; }
        [JsonIgnore]
        public bool IsEnd { get; set; }
        [JsonIgnore]
        public bool IsProvenanceNC { get; set; }
        
        public bool IsUnderWatch { get; set; }
        [ForeignKey(nameof(AgenceType_Id))]
        public virtual AgenceType AgenceType { get; set; }
        [JsonIgnore]
        public int AgenceType_Id { get; set; }
        public string OtherName { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(Region_Id))]
        public virtual Region Region { get; set; }
        [JsonIgnore]
        public string Region_Id { get; set; }
        [JsonIgnore]
        public bool UserCreated { get; set; }

        public List<DeclarationBonnePratique> DeclarationBonnePratiques { get; set; }

        public override BaseModel Clone()
        {
            var obj = new Agence
            {
                CreationDate = CreationDate,
                Id = Id,
                Name = Name,
                IsStart = IsStart,
                IsEnd = IsEnd,
                Region = Region,
                AgenceType = AgenceType
            };
            return obj;
        }
    }

    public class AgenceType : EnumType
    {
        
    }
}