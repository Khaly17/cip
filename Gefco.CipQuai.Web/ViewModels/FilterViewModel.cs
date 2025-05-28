using System;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;

namespace Gefco.CipQuai.Web.Models
{
    public class FilterViewModel
    {
        public FilterViewModel()
        {
            
        }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public FilterType FilterType { get; set; } = FilterType.Default;

        public string Id { get; set; }
    }

    public enum FilterType
    {
        Default,
        Month,
        Week,
        Other
    }
}