using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.ViewModels
{
    public class BusinessRoleViewModel
    {
        public BusinessRoleViewModel(BusinessRole role)
        {
            Key = role.Key;
            Value = role.Value;
            Description = role.Description;
            Type = role is AgenceRole ? "Rôle agence" : role is NationalRole ? "Rôle national" : "Rôle région";
        }

        public string Type { get; set; }
        public string Description { get; set; }

        public string Value { get; set; }

        public int Key { get; set; }
    }
}