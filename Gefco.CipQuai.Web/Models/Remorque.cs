namespace Gefco.CipQuai.Web.Models
{
    public class Remorque : BaseModel
    {
        public string NuméroRemorque { get; set; }
        public bool IsDoublePlancher { get; set; }
        public override BaseModel Clone()
        {
            var item = new Remorque
            {
                CreationDate = CreationDate,
                Id = Id,
                NuméroRemorque = NuméroRemorque,
                IsDoublePlancher = IsDoublePlancher
            };
            return item;
        }
    }
}