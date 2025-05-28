using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gefco.CipQuai.Web.Models
{
    public class AgenceViewModel
    {
        public AgenceViewModel()
        {

        }
        public AgenceViewModel(Agence agence)
        {
            Name = agence.Name;
            OtherName = agence.OtherName;
            Id = agence.Id;
            Region = agence.Region?.Name ?? "Autre";
            AgenceType = agence.AgenceType.Value;
            AgenceType_Id = agence.AgenceType_Id;
            Region_Id = agence.Region_Id;
            IsStart = agence.IsStart;
            IsEnd = agence.IsEnd;
            IsProvenanceNC = agence.IsProvenanceNC;
            IsUnderWatch = agence.IsUnderWatch;
        }
        public int AgenceType_Id { get; set; }
        public string Region_Id { get; set; }

        public string Name { get; set; }
        public string Region { get; set; }
        public string OtherName { get; set; }
        public string AgenceType { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsProvenanceNC { get; set; }
        public bool IsUnderWatch { get; set; }

        public string Id { get; set; }

        public void UpdateModel(Agence agence)
        {
            agence.Name = Name;
            agence.OtherName = OtherName;
            agence.IsStart = IsStart;
            agence.IsEnd = IsEnd;
            agence.IsProvenanceNC = IsProvenanceNC;
            agence.IsUnderWatch = IsUnderWatch;
        }

    }
}