using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Gefco.CipQuai.ApiClient
{
    public partial class GefcoCipQuaiWeb
    {
        public GefcoCipQuaiWeb()
        {
            this.Initialize();
        }
        private static GefcoCipQuaiWeb _instance;
        public static GefcoCipQuaiWeb Instance => _instance ?? (_instance = new GefcoCipQuaiWeb());
        static GefcoCipQuaiWeb()
        {
            _instance = new GefcoCipQuaiWeb();
        }


        partial void CustomInitialize()
        {
            BaseUri = new Uri(Settings.ServiceUri);
            HttpClient = new HttpClient(new HttpClientHandler(), false)
            {
                BaseAddress = new Uri(Settings.ServiceUri, UriKind.Absolute),
                Timeout = TimeSpan.FromSeconds(30)
            };
            HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

        }

    }
    public static class Settings
    {
#if DEBUG
        public static string DebugServiceUri { get; } = "http://192.168.1.27/CipQuai";
        public static string ServiceUri { get; } = "http://192.168.1.27/CipQuai";
        //public static string DebugServiceUri { get; } = "http://cipquai.waxalica.com/";
        //public static string ServiceUri { get; } = "http://cipquai.waxalica.com/";
        //public static string DebugServiceUri { get; } = "http://r.cipquai.sensor6ty.com/";
        //public static string ServiceUri { get; } = "http://r.cipquai.sensor6ty.com/";
        //public static string DebugServiceUri { get; } = "http://cipquai.sensor6ty.com/";
        //public static string ServiceUri { get; } = "http://cipquai.sensor6ty.com/";
#else
        //public static string DebugServiceUri { get; } = "http://cipquai.waxalica.com/";
        //public static string ServiceUri { get; } = "http://cipquai.waxalica.com/";
        //public static string DebugServiceUri { get; } = "http://r.cipquai.sensor6ty.com/";
        //public static string ServiceUri { get; } = "http://r.cipquai.sensor6ty.com/";
        public static string DebugServiceUri { get; } = "http://cipquai.sensor6ty.com/";
        public static string ServiceUri { get; } = "http://cipquai.sensor6ty.com/";
        //public static string DebugServiceUri { get; } = "https://cipquai.gefco-france.net";
        //public static string ServiceUri { get; } = "https://cipquai.gefco-france.net";
#endif

    }

}
