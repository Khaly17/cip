using System.Linq;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListItemDPViewModel
    {
        public ListItemDPViewModel()
        {
            
        }

        public ListItemDPViewModel(DeclarationDoublePlancher doublePlancher)
        {
            Date = doublePlancher.Traction.DueDate.ToString("dd/MM/yyyy");
            Heure = (doublePlancher.CompletionDate ?? doublePlancher.CreationDate).ToString("HH:mm");
            Origine = doublePlancher.Traction?.AgenceDepart?.Name;
            Destination = doublePlancher.Traction?.AgenceArrivee?.Name;
            var motifs = doublePlancher.MotifDps.OrderBy(p => p.DisplayOrder).Select(p => p.Name).ToList();
            if (!doublePlancher.AutreMotifDP.IsNullOrWhiteSpace())
                motifs.Add(doublePlancher.AutreMotifDP);
            MotifNC = motifs.FirstOrDefault();
            var joins = doublePlancher.MotifDps.Where(p => (p.IsOther && !doublePlancher.AutreMotifDP.IsNullOrWhiteSpace()) || !p.IsOther).Select(p => p.IsOther ? "Autre motif : " + doublePlancher.AutreMotifDP.Trim() : p.Name).ToList();
            MotifsNC = string.Join(", ", joins);
            DpUtilisé = doublePlancher.IsDPUsed;
            Id = doublePlancher.Id;
            Status = !doublePlancher.IsDeleted ? doublePlancher.CurrentStatus?.Description ?? "To be validated" : "Supprimée";
        }


        public string Status { get; set; }
        public string Date { get; set; }
        public string Heure { get; set; }
        public string Origine { get; set; }
        public string Destination { get; set; }
        public bool DpUtilisé { get; set; }
        public string MotifNC { get; set; }
        public string MotifsNC { get; set; }
        public string Id { get; set; }
        public string DetailsLink { get; set; }
    }
}