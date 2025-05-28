using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Models
{
    public class StatDPViewModel
    {
        [JsonProperty("Utilisées")]
        public int DpUtilisées { get; set; }

        [JsonProperty("Attendues")]
        public int DpAttendues { get; set; }
        [JsonProperty("Data")]
        public List<DataValue> Data { get; set; }

        public string PercentData => DpAttendues == 0 ? "100%" : (DpUtilisées * 100 / DpAttendues) + "%";
        [JsonProperty("Non utilisées")]
        public int DpNonUtilisées { get; set; }
        public int T1 { get; set; }
        public int T2 { get; set; }

        [JsonProperty("A déclarer")]
        public int ADéclarer => DpAttendues - (DpUtilisées + DpNonUtilisées);
    }
}