using System.Collections.Generic;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Views
{
    public class NCRegionalStatViewModel : FilterViewModel
    {
        public List<CategoryDataValue> StackedWeekCategoriesData { get; set; }
        public List<CategoryDataValue> StackedAgenceCategoriesData { get; set; }
        public List<CategoryDataValue> StackedTopAgenceCategoriesData { get; set; }
        public List<string> Categories { get; set; }

        public NCRegionalStatViewModel()
        {
            StackedWeekCategoriesData = new List<CategoryDataValue>();
            StackedAgenceCategoriesData = new List<CategoryDataValue>();
            StackedTopAgenceCategoriesData = new List<CategoryDataValue>();
        }
    }
}