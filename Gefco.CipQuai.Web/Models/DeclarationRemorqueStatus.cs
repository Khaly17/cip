using System.ComponentModel.DataAnnotations.Schema;

namespace Gefco.CipQuai.Web.Models
{
    public class DeclarationRemorqueStatus : BaseModel
    {
        public DeclarationRemorque DeclarationRemorque { get; set; }
        [ForeignKey(nameof(RemorqueStatus_Id))]
        public RemorqueStatus RemorqueStatus { get; set; }
        public string RemorqueStatus_Id { get; set; }
        public override BaseModel Clone()
        {
            return new DeclarationRemorqueStatus()
            {
                DeclarationRemorque = DeclarationRemorque,
                RemorqueStatus = RemorqueStatus,
                CreationDate = CreationDate,
                Id = Id
            };
        }
    }
}