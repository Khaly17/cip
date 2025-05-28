using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Gefco.CipQuai.Services;
using Xamarin.Forms;

namespace Gefco.CipQuai.Droid.Services
{
    /// <summary>
    ///     Service culture.
    /// </summary>
    public class ServiceDeviceInfo : IServiceDeviceInfo
    {
        public ServiceDeviceInfo()
        {
        }

        /// <summary>
        ///     Gets the device notification token.
        /// </summary>
        /// <returns>The device token.</returns>
        public void GetDeviceToken(Action<string> callback)
        {
            if (string.IsNullOrEmpty(App.DeviceToken))
            {
                App.DeviceTokenAvailable += callback;
                if (MainActivity.Instance.IsPlayServicesAvailable())
                {
                    App.DeviceToken = FirebaseInstanceId.Instance.Token;
                    callback?.Invoke(App.DeviceToken);
                }
            }
            else
                callback?.Invoke(App.DeviceToken);
        }


    }
}