using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Services;

namespace Gefco.CipQuai.LoadingPage
{
    public class ViewModel : ObservableObject
    {
        private string _loadingMessage;
        public event Action Loaded;
        public string LoadingMessage
        {
            get => _loadingMessage;
            set
            {
                if (value == _loadingMessage) return;
                _loadingMessage = value;
                OnPropertyChanged();
            }
        }
        public IDialogService ServiceDialog { get; set; }

        private double _progress;

        public double Progress
        {
            get { return _progress; }
            set
            {
                if (value.Equals(_progress))
                    return;
                _progress = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ProgressText));
            }
        }
        public string ProgressText
        {
            get { return (int)_progress + "%"; }
        }

        public async void Load(DependencyRegistratorBase registrator)
        {
            LoadingMessage = "Initialisation des services, merci de patienter...";
            await Task.Factory.StartNew(LoadDataServices);
            Progress = 5;
            await Task.Factory.StartNew(LoadDependencyServices);
            await Task.Delay(150);
            Progress = 10;

            LoadingMessage = "Chargement de la configuration, merci de patienter...";
            await LoadConfig();
            await Task.Delay(150);

            Progress = 90;
            LoadingMessage = "Synchronisation en cours, merci de patienter...";
            await Task.Factory.StartNew(Sync);
            await Task.Factory.StartNew(PreLoad);
            await Task.Delay(150);

            Progress = 95;
            LoadingMessage = "Initialisation des pages, merci de patienter...";
            await Task.Factory.StartNew(LoadViewModels);
            await Task.Factory.StartNew(LoadPages);
            await Task.Delay(150);

            Progress = 100;
            LoadingMessage = "Initialisation en cours, merci de patienter...";
            await Task.Delay(50);

            Loaded?.Invoke();
        }
        private void LoadDataServices()
        {
        }
        private void LoadDependencyServices()
        {
            ServiceDialog = ServiceLocator.Current.GetInstance<IDialogService>();
            ServiceLocator.Current.GetInstance<IMyNavigation>();
            ServiceLocator.Current.GetInstance<IServiceNetwork>();
            ServiceLocator.Current.GetInstance<ILoginService>();
            ServiceLocator.Current.GetInstance<IServiceDeviceInfo>();
        }
        private async Task LoadConfig()
        {
            if (App.Services.ServiceNetwork.IsConnected())
            {
                if (App.Settings.UserToSave != null)
                    await App.UpdateUser(App.Settings.UserToSave, 2);
                Progress = 15;
                await App.LoadConfigurationsAsync();
                Progress = 25;
                await App.LoadResourcesAsync();
                Progress = 40;
                await App.LoadMotifDPsAsync();
                Progress = 50;
                await App.LoadRemorqueStatusesAsync();
                Progress = 60;
                await App.LoadTractionsAsync();
                Progress = 70;
                await App.LoadActiveDeclarationDoublePlanchersAsync();
            }
            //try
            //{
            //    GetDeviceToken();
            //}
            //catch (Exception)
            //{
            //}
        }
        private async void GetDeviceToken()
        {
            var deviceInfo = ServiceLocator.Current.GetInstance<IServiceDeviceInfo>();
            if (string.IsNullOrEmpty(App.DeviceToken))
                deviceInfo.GetDeviceToken(async token => await SendDeviceInfo(token));
            else
                await SendDeviceInfo(App.DeviceToken);
        }
        private async Task SendDeviceInfo(string deviceToken)
        {
            var sendDeviceInfo = await DataService.SetDevice(deviceToken);
        }

        private async Task Sync()
        {
            await App.CheckToSyncAsync();
        }

        private async Task PreLoad()
        {
        }
        private void LoadViewModels()
        {
            //ServiceLocator.Current.GetInstance<StartPage.ViewModel>();
        }
        private void LoadPages()
        {
            //ServiceLocator.Current.GetInstance<StartPage.StartPage>();
        }

    }
}
