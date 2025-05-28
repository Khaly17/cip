using System;
using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Views;
using Android.OS;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using Firebase;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: UsesFeature("android.hardware.camera", Required = true)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
namespace Gefco.CipQuai.Droid
{
    [Activity(Label = "Cip Quai", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public MainActivity()
        {
            Instance = this;
            var ignore = typeof(FFImageLoading.Svg.Forms.SvgCachedImage);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            var args = new CancelEventArgs();
            App.InvokeOnBackButtonPressed(args);
            if (!args.Cancel)
                base.OnBackPressed();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                FirebaseApp.InitializeApp(this);

                CachedImageRenderer.Init(true);
                FFImageLoading.Svg.Forms.SvgCachedImage.Init();
                var config = new FFImageLoading.Config.Configuration()
                {
                    VerboseLogging = false,
                    VerbosePerformanceLogging = false,
                    VerboseMemoryCacheLogging = false,
                    VerboseLoadingCancelledLogging = false,
                    Logger = new CustomLogger(),
                };
                ImageService.Instance.Initialize(config);
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                AppCenter.Start("8cd8284b-a2f3-44d0-bc48-cfbbbd978cc0", typeof(Analytics), typeof(Crashes), typeof(Distribute));

                //var tmp = new RadEntry();
                var metricsHeightPixels = Resources.DisplayMetrics.HeightPixels;
                var metricsWidthPixels = Resources.DisplayMetrics.WidthPixels;
                var metricsDensity = Resources.DisplayMetrics.Density;
                App.SetScreenDimensions((int)(metricsHeightPixels / metricsDensity), (int)(metricsWidthPixels / metricsDensity));
                
                base.OnCreate(savedInstanceState);
                Forms.Init(this, savedInstanceState);
                VersionTracking.Track();
                //UpdateManager.Register(this, App.Settings.HockeyAppApiKey);
                LoadApplication(new App(new DependencyRegistrator()));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Crashes.TrackError(e);
                throw;
            }
        }
        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Console.WriteLine(errorMessage);
                Crashes.TrackError(new Exception(errorMessage));
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }

        public static MainActivity Instance { get; private set; }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Menu)
            {
                App.InvokeMenuPressed();

                return true;
            }

            Console.WriteLine("Non menukey");

            return base.OnKeyUp(keyCode, e);
        }
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    ;//msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    //msgText.Text = "This device is not supported";
                    //Finish();
                }
                return false;
            }
            else
            {
                //msgText.Text = "Google Play Services is available.";
                return true;
            }
        }

    }
}