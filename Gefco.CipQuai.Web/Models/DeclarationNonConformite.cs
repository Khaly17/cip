using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Gefco.CipQuai.Web.Extensions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class DeclarationNonConformite : BaseModel
    {
        [ForeignKey(nameof(AgenceConcernée_Id))]
        public virtual Agence AgenceConcernée { get; set; }
        [JsonIgnore]
        public string AgenceConcernée_Id { get; set; }

        public int CurrentWorkflowStep { get; set; }
        public virtual List<MotifNC> MotifNCs { get; set; }
        public string AutreMotifNC { get; set; }
        public string NumVoyage { get; set; }
        public virtual List<Picture> Pictures { get; set; }
        [ForeignKey(nameof(CurrentStatus_Id))]
        public virtual DeclarationNcStatus CurrentStatus { get; set; }
        public string CurrentStatus_Id { get; set; }

        [JsonIgnore]
        public string AgenceId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(AgenceId))]
        public virtual Agence Agence { get; set; }

        public override BaseModel Clone()
        {
            var item = new DeclarationNonConformite()
            {
                CreationDate = CreationDate,
                Id = Id,
                AgenceConcernée = (Agence) AgenceConcernée.Clone(),
                CurrentWorkflowStep = CurrentWorkflowStep,
                AgenceConcernée_Id = AgenceConcernée_Id,
                AutreMotifNC = AutreMotifNC,
                NumVoyage = NumVoyage,
            };
            if (Pictures != null)
            {
                var pictures = Pictures.OrderByDescending(p => p.CreationDate).DistinctBy(p => p.PictureType);
                item.Pictures = pictures.CloneList();
            }
            return item;
        }
        public void UpdateWith(DeclarationNonConformite declaration, IQueryable<MotifNC> motifNCs)
        {
            if (declaration.MotifNCs != null)
            {
                if (MotifNCs == null)
                    MotifNCs = new List<MotifNC>();
                MotifNCs.Clear();
                foreach (var motifNC in declaration.MotifNCs)
                {
                    MotifNCs.Add(motifNCs.SingleOrDefault(p => p.Id == motifNC.Id));
                }
            }
            else
            {
                MotifNCs?.Clear();
            }
            AutreMotifNC = declaration.AutreMotifNC;
            NumVoyage = declaration.NumVoyage;
            CurrentWorkflowStep = declaration.CurrentWorkflowStep;
            AgenceConcernée_Id = declaration.AgenceConcernée.Id;
        }

    }
    public enum DeclarationNCStatus
    {
        Created = 0,
        InProgress,
        ToBeValidated,
        Valid,
        Invalid
    }

}