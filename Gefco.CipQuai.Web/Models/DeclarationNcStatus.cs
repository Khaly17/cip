namespace Gefco.CipQuai.Web.Models
{
    public class DeclarationNcStatus : NameBaseModel
    {
        public string Description { get; set; }
        public override BaseModel Clone()
        {
            var item = new DeclarationNcStatus()
            {
                CreationDate = CreationDate,
                Id = Id,
                Name = Name,
                Description = Description,
            };
            return item;
        }

        public static readonly string ToBeValidated = nameof(ToBeValidated);
        public static readonly string Validated = nameof(Validated);
        public static readonly string Deleted = nameof(Deleted);
    }
}