using System.Collections.Generic;
using System.Linq;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListeDPViewModel : FilterViewModel
    {
        public ListeDPViewModel()
        {

        }
        public ListeDPViewModel(FilterViewModel viewModel, List<DeclarationDoublePlancher> items)
        {
            Items = items.Select(p => new ListItemDPViewModel(p)).ToList();
            Id = viewModel.Id;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            FilterType = viewModel.FilterType;
        }

        public List<ListItemDPViewModel> Items { get; set; }
    }
}