using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class StatNCViewModel : FilterViewModel
    {
        [JsonProperty("Data")]
        public List<DataValue> Data { get; set; }

        public StatNCViewModel()
        {
            
        }
        public StatNCViewModel(FilterViewModel viewModel)
        {
            Id = viewModel.Id;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            FilterType = viewModel.FilterType;
        }
    }
}