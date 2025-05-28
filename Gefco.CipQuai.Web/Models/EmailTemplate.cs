using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gefco.CipQuai.Web.Models
{
    public class EmailTemplate : NameBaseModel
    {
        public EmailTemplate()
        {
        }

        public string Object { get; set; }
        public string Content { get; set; }
        public override BaseModel Clone()
        {
            return null;
        }
    }
}