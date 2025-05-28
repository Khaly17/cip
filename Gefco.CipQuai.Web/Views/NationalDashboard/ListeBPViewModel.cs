using System.Collections.Generic;
using System.Linq;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListeBPViewModel : FilterViewModel
    {
        public ListeBPViewModel()
        {

        }
        public ListeBPViewModel(FilterViewModel viewModel, List<DeclarationBonnePratique> items)
        {
            Items = items.Select(p => new ListItemBPViewModel(p)).ToList();
            Id = viewModel.Id;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            FilterType = viewModel.FilterType;
        }

        public List<ListItemBPViewModel> Items { get; set; }
    }
}