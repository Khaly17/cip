using System.Collections.Generic;
using Gefco.CipQuai.Web.Models;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Views
{
    public class NCNationalStatViewModel : FilterViewModel
    {
        public List<CategoryDataValue> StackedRegionCategoriesData { get; set; }
        public List<CategoryDataValue> StackedAgenceCategoriesData { get; set; }
        public List<string> Categories { get; set; }
        public List<DataValue> WeekData { get; set; }
        public List<DataValue> ContributorsData { get; set; }
        public List<DataValue> PieData { get; set; }

        public NCNationalStatViewModel()
        {
            StackedRegionCategoriesData = new List<CategoryDataValue>();
            StackedAgenceCategoriesData = new List<CategoryDataValue>();
            WeekData = new List<DataValue>();
            ContributorsData = new List<DataValue>();
            PieData = new List<DataValue>();
        }
    }
}