using System.Collections.Generic;

namespace Gefco.CipQuai.Web.Models
{
    public class RemorqueStatus : NameBaseModel
    {
        public string Description { get; set; }
        public override BaseModel Clone()
        {
            var item = new RemorqueStatus()
            {
                CreationDate = CreationDate,
                Id = Id,
                Name = Name,
                Description = Description,
            };
            return item;
        }
        public static readonly List<string> OwnerActiveStatuses = new List<string>()
        {
            "PausedAndLocked",
            "PausedAndFree",
            "InProgress",
            "ToJustify",
        };
        public static readonly List<string> OthersActiveStatuses = new List<string>()
        {
            "PausedAndFree",
        };
        public static readonly List<string> DashboardStatuses = new List<string>()
        {
            "ToBeValidated",
            "Valid",
            "NotValid",
            "ToJustify",
        };
    }
}