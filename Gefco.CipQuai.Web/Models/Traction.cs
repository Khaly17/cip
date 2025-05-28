using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class Traction : NameBaseModel
    {
        public virtual Agence AgenceDepart { get; set; }
        public virtual Agence AgenceArrivee { get; set; }
        [JsonIgnore]
        public TractionDefinition TractionDefinition { get; set; }
        public string NumeroBorderau { get; set; }
        public string IdVoyage { get; set; }
        public DateTime DueDate { get; set; }
        [JsonIgnore]
        public virtual List<DeclarationRemorque> Declarations { get; set; }

        public bool IsCreated { get; set; }
        public bool IsCancelled { get; set; }
        public string CancelReason { get; set; }

        public override BaseModel Clone()
        {
            var item = new Traction()
            {
                Id = Id,
                CreationDate = CreationDate,
                Name = Name,
                NumeroBorderau = NumeroBorderau,
                IdVoyage = IdVoyage,
                DueDate = DueDate,
            };
            if (AgenceDepart != null)
                item.AgenceDepart = new Agence() { Id = AgenceDepart.Id, Name = AgenceDepart.Name };
            if (AgenceArrivee != null)
                item.AgenceArrivee = new Agence() { Id = AgenceArrivee.Id, Name = AgenceArrivee.Name };
            return item;
        }
    }
}