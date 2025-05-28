using System.Collections.Generic;
using System.Linq;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListeSPViewModel : FilterViewModel
    {
        public ListeSPViewModel()
        {

        }
        public ListeSPViewModel(FilterViewModel viewModel, List<DeclarationSimplePlancher> items)
        {
            Items = items.Select(p => new ListItemSPViewModel(p)).ToList();
            Id = viewModel.Id;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            FilterType = viewModel.FilterType;
        }

        public List<ListItemSPViewModel> Items { get; set; }
    }
}