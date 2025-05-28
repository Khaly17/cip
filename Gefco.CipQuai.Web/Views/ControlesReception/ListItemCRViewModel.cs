using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListItemCRViewModel
    {
        public ListItemCRViewModel()
        {
            
        }

        public ListItemCRViewModel(DeclarationControleReception item)
        {
            Date = (item.CreationDate.ToLocalTime()).ToString("dd/MM/yyyy");
            Heure = (item.CreationDate.ToLocalTime()).ToString("HH:mm");
            Acteur = item.CreatedBy.FirstName + " " + item.CreatedBy.LastName;
            Origine = item.Agence.Name;
            Destination = item.AutreAgenceArrivee;
            //Description = item.Description;
            Id = item.Id;
        }


        public string Date { get; set; }
        public string Heure { get; set; }
        public string Acteur { get; set; }
        public string Origine { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string DetailsLink { get; set; }
    }
}
