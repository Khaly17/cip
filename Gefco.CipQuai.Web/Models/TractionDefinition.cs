using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class TractionDefinition : NameBaseModel
    {
        [ForeignKey(nameof(AgenceDepart_Id))]
        public virtual Agence AgenceDepart { get; set; }
        public string AgenceDepart_Id { get; set; }

        [ForeignKey(nameof(AgenceArrivee_Id))]
        public virtual Agence AgenceArrivee { get; set; }
        public string AgenceArrivee_Id { get; set; }
        public string DaysOfWeekValue { get; set; }
        [NotMapped]
        public List<Days> DaysOfWeek
        {
            get
            {
                var strs = DaysOfWeekValue?.Split(',');
                var res = new List<Days>();
                if (strs != null)
                    foreach (var str in strs)
                    {
                        if (int.TryParse(str, out int day))
                        res.Add((Days) day);
                        else if (Enum.TryParse(str, out Days days))
                        res.Add(days);
                    }
                return res;
            }
            set => DaysOfWeekValue = string.Join(",", value);
        }
        [JsonIgnore]
        public virtual List<Traction> Tractions { get; set; }

        public override BaseModel Clone()
        {
            return new TractionDefinition()
            {
                AgenceArrivee = AgenceArrivee,
                AgenceDepart = AgenceDepart,
                DaysOfWeek = DaysOfWeek,
                CreationDate = CreationDate,
                Id = Id,
                Name = Name,
            };
        }
    }
    public enum Days
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 0,
    }

}