using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class StatBPViewModel
    {
        [JsonProperty("Déclarées")]
        public int BpDeclarées { get; set; }

        [JsonProperty("Attendues")]
        public int BpAttendues { get; set; }

        public List<DataValue> Data { get; set; }
        public string PercentData => BpAttendues == 0 ? "100%" : (BpDeclarées * 100 / BpAttendues) + "%";
    }
}