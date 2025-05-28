using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gefco.CipQuai.Web.Models
{
    public class Configuration : NameBaseModel
    {
        public string Description { get; set; }
        public string Value { get; set; }

        public override BaseModel Clone()
        {
            return new Configuration()
            {
                CreationDate = CreationDate,
                Id = Id,
                Name = Name,
                Value = Value,
                Description = Description
            };
        }
    }
}