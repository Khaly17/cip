using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListItemBPViewModel
    {
        public ListItemBPViewModel()
        {
            
        }

        public ListItemBPViewModel(DeclarationBonnePratique bonnePratique)
        {
            Date = (bonnePratique.CreationDate).ToLocalTime().ToString("dd/MM/yyyy");
            Heure = (bonnePratique.CreationDate).ToLocalTime().ToString("HH:mm");
            Origine = bonnePratique.CreatedBy.FirstName + " " + bonnePratique.CreatedBy.LastName;
            Destination = bonnePratique.AutreAgentConcerné;
            Description = bonnePratique.Description;
            Id = bonnePratique.Id;
        }


        public string Date { get; set; }
        public string Heure { get; set; }
        public string Origine { get; set; }
        public string Destination { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string DetailsLink { get; set; }
    }
}