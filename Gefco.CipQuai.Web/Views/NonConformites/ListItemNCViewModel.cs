using System.Linq;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListItemNCViewModel
    {
        public ListItemNCViewModel()
        {
            
        }

        public ListItemNCViewModel(DeclarationNonConformite nonConformite)
        {
            Date = (nonConformite.CreationDate).ToLocalTime().ToString("dd/MM/yyyy");
            Heure = (nonConformite.CreationDate).ToLocalTime().ToString("HH:mm");
            Emetteur = nonConformite.CreatedBy.FirstName + " " + nonConformite.CreatedBy.LastName;
            Origine = nonConformite.CreatedBy?.MobileUserAgence?.Name;
            Destination = nonConformite.AgenceConcernée.Name;
            NumVoyage = nonConformite.NumVoyage;
            var motifs = nonConformite.MotifNCs.OrderBy(p => p.DisplayOrder).Select(p => p.Name).ToList();
            if (!nonConformite.AutreMotifNC.IsNullOrWhiteSpace())
                motifs.Add(nonConformite.AutreMotifNC);
            MotifNC = motifs.FirstOrDefault();

            var joins = nonConformite.MotifNCs.Where(p => (p.IsOther && !nonConformite.AutreMotifNC.IsNullOrWhiteSpace()) || !p.IsOther).Select(p => p.IsOther ? "Autre motif : " + nonConformite.AutreMotifNC.Trim() : p.Name).ToList();
            MotifsNC = string.Join(",", joins);
            Id = nonConformite.Id;
            Status = nonConformite.IsDeleted ? "Supprimée" : nonConformite.CurrentStatus?.Description ?? "A valider";
        }


        public string Status { get; set; }
        public string Date { get; set; }
        public string Heure { get; set; }
        public string Emetteur { get; set; }
        public string Origine { get; set; }
        public string Destination { get; set; }
        public string NumVoyage { get; set; }
        public string MotifNC { get; set; }
        public string MotifsNC { get; set; }
        public string Id { get; set; }
        public string DetailsLink { get; set; }
    }
}