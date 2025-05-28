using System.Collections.Generic;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class NCStatListViewModel : FilterViewModel
    {
        public List<DeclarationNonConformite> Data { get; set; }

        public NCStatListViewModel()
        {
            Data = new List<DeclarationNonConformite>();
        }

        public NCStatListViewModel(FilterViewModel viewModel)
        {
            Id = viewModel.Id;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            FilterType = viewModel.FilterType;
        }
    }
}