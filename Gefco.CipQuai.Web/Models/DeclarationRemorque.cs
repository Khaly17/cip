using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Gefco.CipQuai.Web.Extensions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public abstract class DeclarationRemorque : BaseModel
    {
        public bool IsCr { get; set; }
        [ForeignKey(nameof(Traction_Id))]
        public virtual Traction Traction { get; set; }
        public string Traction_Id { get; set; }
        public Remorque Remorque { get; set; }
        [ForeignKey(nameof(CurrentStatus_Id))]
        public virtual RemorqueStatus CurrentStatus { get; set; }
        public string CurrentStatus_Id { get; set; }
        [JsonIgnore]
        public virtual List<DeclarationRemorqueStatus> StatusHistory { get; set; }
        [JsonIgnore]
        public DateTime? CompletionDate { get; set; }
        public int CurrentWorkflowStep { get; set; }
        public string AutreAgenceArrivee { get; set; }
        public virtual List<Picture> Pictures { get; set; }

        [JsonIgnore]
        public string AgenceId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(AgenceId))]
        public virtual Agence Agence { get; set; }

        public bool IsMine(ApplicationUser user)
        {
            return CreatedBy.Id == user.Id;
        }
        public bool IsMineActive(ApplicationUser user)
        {
            return IsMine(user) && RemorqueStatus.OwnerActiveStatuses.Contains(CurrentStatus.Name);
        }
        public bool IsOthersActive(ApplicationUser user)
        {
            return !IsMine(user) && RemorqueStatus.OthersActiveStatuses.Contains(CurrentStatus.Name);
        }

    }
    public class DeclarationSimplePlancher : DeclarationRemorque
    {
        public override BaseModel Clone()
        {
            var item = new DeclarationSimplePlancher()
            {
                CreationDate = CreationDate,
                Id = Id,
                Traction = Traction,
                Remorque = Remorque,
                CurrentStatus = CurrentStatus,
                CompletionDate = CompletionDate,
                CurrentWorkflowStep = CurrentWorkflowStep,
                AutreAgenceArrivee = AutreAgenceArrivee,
                Traction_Id = Traction_Id,
                AgenceId = AgenceId
            };
            return item;
        }
    }

    public class DeclarationControleReception : DeclarationRemorque
    {
        public DeclarationControleReception()
        {
            IsCr = true;
        }
        public override BaseModel Clone() { return new DeclarationControleReception()
                                                   {
                                                     AgenceId = AgenceId,
                                                     IsCr = IsCr,
                                                     Traction_Id = Traction_Id,
                                                     AutreAgenceArrivee = AutreAgenceArrivee,
                                                     CompletionDate = CompletionDate,
                                                     CreatedBy_Id = CreatedBy_Id,
                                                     CreationDate = CreationDate,
                                                     CurrentStatus_Id = CurrentStatus_Id,
                                                     CurrentWorkflowStep = CurrentWorkflowStep,
                                                     Id = Id,
                                                   };
        }
    }
    public class DeclarationDoublePlancher : DeclarationRemorque
    {
        public bool IsDPUsed { get; set; }
        public virtual List<MotifDP> MotifDps { get; set; }
        public string AutreMotifDP { get; set; }
        public int NbDPCassées { get; set; }
        public override BaseModel Clone()
        {
            var item = new DeclarationDoublePlancher()
            {
                CreationDate = CreationDate,
                Id = Id,
                Remorque = Remorque,
                MotifDps = MotifDps,
                IsDPUsed = IsDPUsed,
                NbDPCassées = NbDPCassées,
                CurrentStatus = (RemorqueStatus) CurrentStatus?.Clone(),
                AutreMotifDP = AutreMotifDP,
                AutreAgenceArrivee = AutreAgenceArrivee,
                CurrentWorkflowStep = CurrentWorkflowStep,
                AgenceId = AgenceId
            };
            if (Traction != null)
            {
                item.Traction = new Traction(){Id = Traction.Id, Name = Traction.Name, DueDate = Traction.DueDate };
                if (Traction.AgenceDepart != null)
                    item.Traction.AgenceDepart = new Agence() { Id = Traction.AgenceDepart.Id, Name = Traction.AgenceDepart.Name };
                if (Traction.AgenceArrivee != null)
                    item.Traction.AgenceArrivee = new Agence() { Id = Traction.AgenceArrivee.Id, Name = Traction.AgenceArrivee.Name };
            }
            if (Pictures != null)
            {
                var pictures = Pictures.OrderByDescending(p => p.CreationDate).DistinctBy(p => p.PictureType);
                item.Pictures = pictures.CloneList();
            }
            return item;
        }

        public void UpdateWith(DeclarationDoublePlancher declaration, IQueryable<MotifDP> motifDps)
        {
            //todo declaration.Remorque = Remorque;
            if (declaration.MotifDps != null)
            {
                if (MotifDps == null)
                    MotifDps = new List<MotifDP>();
                MotifDps.Clear();
                foreach (var motifDp in declaration.MotifDps)
                {
                    MotifDps.Add(motifDps.SingleOrDefault(p => p.Id == motifDp.Id));
                }
            }
            else
            {
                MotifDps?.Clear();
            }
            IsDPUsed = declaration.IsDPUsed;
            NbDPCassées = declaration.NbDPCassées;
            CurrentStatus_Id = declaration.CurrentStatus?.Id ?? declaration.CurrentStatus_Id;
            //todo declaration.CurrentStatus = (RemorqueStatus)CurrentStatus?.Clone();
            AutreMotifDP = declaration.AutreMotifDP;
            AutreAgenceArrivee = declaration.AutreAgenceArrivee;
            CurrentWorkflowStep = declaration.CurrentWorkflowStep;
            AgenceId = declaration.AgenceId;
        }
        
        [NotMapped]
        public static List<PictureType> DpUsedPictureTypes = new List<PictureType>()
        {
            PictureType.HalfLoadPicture,
            PictureType.FullLoadPicture,
        };
        [NotMapped]
        public static List<PictureType> DpNotUsedPictureTypes = new List<PictureType>()
        {
            PictureType.ErrorPicture1,
            PictureType.ErrorPicture2,
            PictureType.ErrorPicture3,
        };
    }

}