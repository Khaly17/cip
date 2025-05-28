using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Gefco.CipQuai.Services;

namespace Gefco.CipQuai.Droid.Services
{
    /// <summary>
    ///     Service network.
    /// </summary>
    public class ServiceNetwork : IServiceNetwork
    {
        /// <summary>
        ///     The connectivity.
        /// </summary>
        private ConnectivityManager connectivity;

        private int _timeout;

        /// <summary>
        ///     Gets the connectivity.
        /// </summary>
        /// <value>The connectivity.</value>
        protected ConnectivityManager Connectivity => connectivity ?? (connectivity = Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager);

        /// <summary>
        ///     Determines whether the device is connected.
        /// </summary>
        /// <returns>true if the device is connected. Otherwise, false.</returns>
        public bool IsConnected()
        {
            return true;
            //var toto = Connectivity.IsDefaultNetworkActive;
            //var titi = Connectivity.ActiveNetwork;
        }

    }
}