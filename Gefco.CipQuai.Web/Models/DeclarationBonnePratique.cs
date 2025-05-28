using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Gefco.CipQuai.Web.Extensions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class DeclarationBonnePratique : BaseModel
    {
        [JsonIgnore]
        public string AgenceId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(AgenceId))]
        public virtual Agence Agence { get; set; }

        [ForeignKey(nameof(AgentConcerné_Id))]
        public virtual ApplicationUser AgentConcerné { get; set; }
        [JsonIgnore]
        public string AgentConcerné_Id { get; set; }
        public string AutreAgentConcerné { get; set; }

        public int CurrentWorkflowStep { get; set; }
        public string Description { get; set; }
        public virtual List<Picture> Pictures { get; set; }

        public override BaseModel Clone()
        {
            var item = new DeclarationBonnePratique()
            {
                CreationDate = CreationDate,
                Id = Id,
                AgentConcerné = AgentConcerné.Clone(),
                CurrentWorkflowStep = CurrentWorkflowStep,
                AutreAgentConcerné = AutreAgentConcerné, 
                Description = Description
            };
            if (Pictures != null)
            {
                var pictures = Pictures.OrderByDescending(p => p.CreationDate).DistinctBy(p => p.PictureType);
                item.Pictures = pictures.CloneList();
            }
            return item;
        }
        public void UpdateWith(DeclarationBonnePratique declaration)
        {
            Description = declaration.Description;
            AutreAgentConcerné = declaration.AutreAgentConcerné;
            AgentConcerné_Id = declaration.AgentConcerné_Id;
            CurrentWorkflowStep = declaration.CurrentWorkflowStep;
        }
    }

    public enum DeclarationBPStatus
    {
        Created = 0,
        InProgress,
        ToBeValidated,
        Valid,
        Invalid
    }
}