using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.DoubleDeckPage;
using Gefco.CipQuai.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gefco.CipQuai.Extensions;
using Microsoft.AppCenter.Distribute;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;

namespace Gefco.CipQuai
{
    public partial class App : Application
    {
        private static ServicesWrapper _services;
        private static string _deviceToken;
        public DependencyRegistratorBase DependencyRegistrator { get; set; }
        public ILoginService LoginService => ServiceLocator.Current.GetInstance<ILoginService>();

        public static double Height { get; set; }
        public static double Width { get; set; }
        public static double ScreenRatio { get; set; }
        public static double Width10 => Width - 10;
        public static double Width20 => Width - 20;
        public static double Width50 => Width - 50;

        public static void SetScreenDimensions(int height, int width)
        {
            Height = height;
            Width = width;
            ScreenRatio = Math.Max(width / 400.0, height / 800.0);
        }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDM5ODUyQDMxMzkyZTMxMmUzMGhwV2FZZlpJbW5vVEVmRFVwM05zRWROdWZDZmV5UFhDbnl4eHVxNUI4UVU9");
            InitializeComponent();
        }
        public App(DependencyRegistratorBase dependencyRegistrator) : this()
        {
            DependencyRegistrator = dependencyRegistrator;
            try
            {
                MainPage = new LoginPage.LoginPage();
                new TaskFactory().StartNew(async () =>
                                           {
                                               var page = await GetStartPageAsync();
                                               InvokeOnUIThread(() => MainPage = page);
                                           });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Crashes.TrackError(ex);
            }
        }
        private void ServiceNavigationOnNavigated(object o, EventArgs eventArgs)
        {
            CheckToSync();
        }
        public static bool IsSynching { get; set; }

        internal static void CheckToSync()
        {
            if (IsSynching)
                return;

            Task.Run(async () =>
            {
                IsSynching = true;
                await SyncAddRemorque();
                await SyncAddControleReception();
                await SyncAddDoublePlancher();
                await SyncAddNonConformite();
                await SyncAddBonnePratique();
                await SyncUpdateDoublePlancher();
                await SyncAddPictures();

                IsSynching = false;
            });
        }
        internal static async Task CheckToSyncAsync()
        {
            if (IsSynching)
                return;

            IsSynching = true;
            await SyncAddRemorque();
            await SyncAddControleReception();
            await SyncAddDoublePlancher();
            await SyncAddNonConformite();
            await SyncAddBonnePratique();
            await SyncUpdateDoublePlancher();
            await SyncAddPictures();

            IsSynching = false;
        }

        internal static async Task SyncAddPictures(bool warn = false)
        {
            Settings.UploadPicture = Settings.UploadPicture.DistinctBy(p => p.Picture.DeclarationId + p.Picture.PicturePath).ToList();
            var items = Settings.UploadPicture.DistinctBy(p => p.Picture.DeclarationId + p.Picture.PicturePath).ToList();
            if (items.Any())
                try
                {
                    PictureServiceResult result = null;
                    foreach (var item in items)
                    {
                        var filePath = item.Picture.PicturePath;
                        var bytes = File.ReadAllBytes(filePath);
                        var fileContent = Convert.ToBase64String(bytes);
                        switch (item.PictureType)
                        {
                            case Settings.UploadPictureType.SP:
                                result = await DataService.UploadPicSP(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.CR:
                                result = await DataService.UploadPicCR(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.DP:
                                result = await DataService.UploadPicDP(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.NC:
                                result = await DataService.UploadPicNC(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.BP:
                                result = await DataService.UploadPicBP(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.Profile:
                                result = await DataService.UploadProfilePic(fileContent, Path.GetFileName(filePath));
                                break;
                        }
                        if (result?.IsSuccess ?? false)
                            Settings.UploadPicture.Remove(item);
                        else if (warn && result?.ErrorMessage != null)
                            await ServiceDialog.ShowError(result.ErrorMessage, "Exception", "OK", null);

                    }
                }
                catch (Exception exception)
                {
                    if (warn)
                        await ServiceDialog.ShowError(exception, "Exception", "OK", null);
                    else
                        Console.WriteLine(exception);
                }
                finally
                {
                    Settings.UploadPicture = Settings.UploadPicture;
                    SyncHappened?.Invoke();
                }
        }
        internal static async Task SyncAddRemorque(bool warn = false)
        {
            var items = Settings.AddDeclarationRemorques.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationRemorque(item);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationExistsException"))
                                Settings.AddDeclarationRemorques.Remove(item);
                        }
                        else if (warn && result?.ErrorMessage != null)
                            await ServiceDialog.ShowError(result.ErrorMessage, "Exception", "OK", null);
                    }
                }
                catch (Exception exception)
                {
                    if (warn)
                        await ServiceDialog.ShowError(exception, "Exception", "OK", null);
                    else
                        Console.WriteLine(exception);
                }
                finally
                {
                    Settings.AddDeclarationRemorques = Settings.AddDeclarationRemorques;
                    SyncHappened?.Invoke();
                }
        }
        internal static async Task SyncAddControleReception(bool warn = false)
        {
            var items = Settings.AddDeclarationControleReceptions.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationControleReception(item);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationExistsException"))
                                Settings.AddDeclarationControleReceptions.Remove(item);
                        }
                        else if (warn && result?.ErrorMessage != null)
                            await ServiceDialog.ShowError(result.ErrorMessage, "Exception", "OK", null);
                    }
                }
                catch (Exception exception)
                {
                    if (warn)
                        await ServiceDialog.ShowError(exception, "Exception", "OK", null);
                    else
                        Console.WriteLine(exception);
                }
                finally
                {
                    Settings.AddDeclarationControleReceptions = Settings.AddDeclarationControleReceptions;
                    SyncHappened?.Invoke();
                }
        }
        internal static async Task SyncAddDoublePlancher(bool warn = false)
        {
            var items = Settings.AddDeclarationDoublePlanchers.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationDoublePlancher(item, item.TractionId);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationExistsException"))
                                Settings.AddDeclarationDoublePlanchers.Remove(item);
                        }
                        else if (warn && result?.ErrorMessage != null)
                            await ServiceDialog.ShowError(result.ErrorMessage, "Exception", "OK", null);
                    }
                }
                catch (Exception exception)
                {
                    if (warn)
                        await ServiceDialog.ShowError(exception, "Exception", "OK", null);
                    else
                        Console.WriteLine(exception);
                }
                finally
                {
                    Settings.AddDeclarationDoublePlanchers = Settings.AddDeclarationDoublePlanchers;
                    SyncHappened?.Invoke();
                }
        }
        internal static async Task SyncUpdateDoublePlancher(bool warn = false)
        {
            var items = Settings.UpdateDeclarationDoublePlanchers.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.UpdateDeclarationDoublePlancherAsync(item, item.CurrentStatus.Id);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationInProgressException") || result.ErrorMessage.Contains("InvalidDeclarationException"))
                                Settings.UpdateDeclarationDoublePlanchers.Remove(item);
                        }
                        else if (warn && result?.ErrorMessage != null)
                            await ServiceDialog.ShowError(result.ErrorMessage, "Exception", "OK", null);
                    }
                }
                catch (Exception exception)
                {
                    if (warn)
                        await ServiceDialog.ShowError(exception, "Exception", "OK", null);
                    else
                        Console.WriteLine(exception);
                }
                finally
                {
                    Settings.UpdateDeclarationDoublePlanchers = Settings.UpdateDeclarationDoublePlanchers;
                    SyncHappened?.Invoke();
                }
        }
        internal static async Task SyncAddNonConformite(bool warn = false)
        {
            var items = Settings.AddDeclarationNonConformites.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationNonConformite(item);
                        if (result?.IsSuccess ?? false)
                        {
                            Settings.AddDeclarationNonConformites.Remove(item);
                        }
                        else if (warn && result?.ErrorMessage != null)
                            await ServiceDialog.ShowError(result.ErrorMessage, "Exception", "OK", null);
                    }
                }
                catch (Exception exception)
                {
                    if (warn)
                        await ServiceDialog.ShowError(exception, "Exception", "OK", null);
                    else
                        Console.WriteLine(exception);
                }
                finally
                {
                    Settings.AddDeclarationNonConformites = Settings.AddDeclarationNonConformites;
                    SyncHappened?.Invoke();
                }
        }
        internal static async Task SyncAddBonnePratique(bool warn = false)
        {
            var items = Settings.AddDeclarationBonnePratiques.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationBonnePratique(item);
                        if (result?.IsSuccess ?? false)
                        {
                            Settings.AddDeclarationBonnePratiques.Remove(item);
                        }
                        else if (warn && result?.ErrorMessage != null)
                            await ServiceDialog.ShowError(result.ErrorMessage, "Exception", "OK", null);
                    }
                }
                catch (Exception exception)
                {
                    if (warn)
                        await ServiceDialog.ShowError(exception, "Exception", "OK", null);
                    else
                        Console.WriteLine(exception);
                }
                finally
                {
                    Settings.AddDeclarationBonnePratiques = Settings.AddDeclarationBonnePratiques; 
                    SyncHappened?.Invoke();
                }
        }

        public async Task<Page> GetStartPageAsync()
        {
            var user = await LoginService.GetUser();
            if (user != null)
            {
                Settings.User = user;
                if (await LoginService.IsLoggedIn())
                {
                    ServiceNavigation.Navigated -= ServiceNavigationOnNavigated;
                    ServiceNavigation.Navigated += ServiceNavigationOnNavigated;

                    var page = new LoadingPage.LoadingPage(DependencyRegistrator);
                    return page;
                }
                return new LoginPage.LoginPage();
            }
            //#if DEBUG
            //            return new NavigationPage(new DoubleDeckPage.DoubleDeckPage());
            //#else            
            return new LoginPage.LoginPage();
            //#endif
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=8cd8284b-a2f3-44d0-bc48-cfbbbd978cc0;", typeof(Analytics), typeof(Crashes), typeof(Distribute));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void InvokeOnUIThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public static IServiceNetwork ServiceNetwork => ServiceLocator.Current.GetInstance<IServiceNetwork>();

        public static IDialogService ServiceDialog => ServiceLocator.Current.GetInstance<IDialogService>();

        public static Settings Settings { get; } = new Settings();
        public static string DeviceToken
        {
            get => _deviceToken;
            set
            {
                if (_deviceToken == value)
                    return;

                _deviceToken = value;
                DeviceTokenAvailable?.Invoke(value);
            }
        }
        public static event Action<string> DeviceTokenAvailable;

        public static ServicesWrapper Services => _services ??= new ServicesWrapper();

        public static event Action MenuPressed;
        public static void InvokeMenuPressed()
        {
            MenuPressed?.Invoke();
        }

        public const string AppId = "{3E4436A3-206C-491D-81B4-FD5F7A82CA68}";

        public static async Task UpdateUser(Settings.UpdateUserParameters parameters, int retryCount = 0, string planId = null)
        {
            var result = await DataService.UpdateUser(parameters);
            if (!result.IsSuccess ?? false)
            {
                Settings.UserToSave = parameters;
                if (retryCount < 2)
                    await UpdateUser(parameters, retryCount + 1);
            }
        }

        public static async Task<IList<DeclarationDoublePlancher>> LoadActiveDeclarationDoublePlanchersAsync()
        {
            var getDeclarationDoublePlanchers = await DataService.GetActiveDeclarationDoublePlanchers();
            if (getDeclarationDoublePlanchers.IsSuccess ?? false)
                Settings.DeclarationDoublePlanchers = getDeclarationDoublePlanchers.Values;
            return getDeclarationDoublePlanchers.Values;
        }

        public static async Task<IList<Traction>> LoadTractionsAsync()
        {
            var getTractions = await DataService.GetTractions();
            if (getTractions.IsSuccess ?? false)
                Settings.Tractions = getTractions.Values;
            return getTractions.Values;
        }
        public static async Task<IList<Agence>> LoadProvenancesAsync()
        {
            var getProvenances = await DataService.GetAgences();
            if (getProvenances.IsSuccess ?? false)
                Settings.Agences = getProvenances.Values;
            return getProvenances.Values;
        }
        public static async Task<IList<Agence>> LoadAgencesCRAsync()
        {
            var getProvenances = await DataService.GetAgencesCR();
            if (getProvenances.IsSuccess ?? false)
                Settings.AgencesCR = getProvenances.Values;
            return getProvenances.Values;
        }

        public static async Task<IList<Configuration>> LoadConfigurationsAsync()
        {
            var getConfigurations = await DataService.GetConfigurations();
            if (getConfigurations.IsSuccess ?? false)
                Settings.Configurations = getConfigurations.Values;
            return getConfigurations.Values;
        }
        public static async Task<IList<MotifDP>> LoadMotifDPsAsync()
        {
            var getMotifDPs = await DataService.GetMotifDPs();
            if (getMotifDPs.IsSuccess ?? false)
                Settings.MotifDPs = getMotifDPs.Values;
            return getMotifDPs.Values;
        }
        public static async Task<IList<MotifNC>> LoadMotifNCsAsync()
        {
            var getMotifNCs = await DataService.GetMotifNCs();
            if (getMotifNCs.IsSuccess ?? false)
                Settings.MotifNCs = getMotifNCs.Values;
            return getMotifNCs.Values;
        }
        public static async Task<IList<Resource>> LoadResourcesAsync()
        {
            var getResources = await DataService.GetResources();
            if (getResources.IsSuccess ?? false)
                Settings.Resources = getResources.Values;
            return getResources.Values;
        }
        public static async Task<IList<RemorqueStatus>> LoadRemorqueStatusesAsync()
        {
            var getRemorqueStatuses = await DataService.GetRemorqueStatuses();
            if (getRemorqueStatuses.IsSuccess ?? false)
                Settings.RemorqueStatuses = getRemorqueStatuses.Values;
            return getRemorqueStatuses.Values;
        }

        public static event Action<CancelEventArgs> BackButtonPressed;
        public static void InvokeOnBackButtonPressed(CancelEventArgs args)
        {
            BackButtonPressed?.Invoke(args);
        }

        public static event Action HomeButtonPressed;
        public static event Action SyncHappened;
        public static void InvokeSyncHappened()
        {
            SyncHappened?.Invoke();
        }
        private void HomeButton_Tapped(object sender, EventArgs e)
        {
            HomeButtonPressed?.Invoke();
        }
    }
}
