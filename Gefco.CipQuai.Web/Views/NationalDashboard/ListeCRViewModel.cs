using System.Collections.Generic;
using System.Linq;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class ListeCRViewModel : FilterViewModel
    {
        public ListeCRViewModel()
        {

        }
        public ListeCRViewModel(FilterViewModel viewModel, List<DeclarationControleReception> items)
        {
            Items = items.Select(p => new ListItemCRViewModel(p)).ToList();
            Id = viewModel.Id;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            FilterType = viewModel.FilterType;
        }

        public List<ListItemCRViewModel> Items { get; set; }
    }
}
