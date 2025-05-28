using System.Collections.Generic;
using System.Linq;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListeNCViewModel : FilterViewModel
    {
        public ListeNCViewModel()
        {
            
        }
        public ListeNCViewModel(FilterViewModel viewModel, List<DeclarationNonConformite> items)
        {
            Items = items.Select(p => new ListItemNCViewModel(p)).ToList();
            Id = viewModel.Id;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            FilterType = viewModel.FilterType;
        }

        public List<ListItemNCViewModel> Items { get; set; }
    }
}