namespace Gefco.CipQuai.Web.Models
{
    public class ProvenanceDP : NameBaseModel
    {
        public bool IsGefcoFrance { get; set; }
        public bool IsInternational { get; set; }
        public bool IsClient { get; set; }
        public override BaseModel Clone()
        {
            return new ProvenanceDP()
            {
                Id = Id,
                CreationDate = CreationDate,
                Name = Name,
                IsClient = IsClient,
                IsInternational = IsInternational,
                IsGefcoFrance = IsGefcoFrance,
            };
        }
    }
}