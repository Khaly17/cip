
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.DoubleDeckPage;
using Gefco.CipQuai.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.DoubleDeckPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChoixDestinationPage
    {
        public DPViewModel ViewModel { get; }

        public ChoixDestinationPage(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            InitializeComponent();
            BindingContext = new ChoixDestinationPageViewModel(dpViewModel);
        }

        private void AutoComplete_OnCompleted(object sender, EventArgs e)
        {
            if (ViewModel.ValidateDestinationCommand.CanExecute(null))
                ViewModel.ValidateDestinationCommand.Execute(null);
        }

        private void SfButton_Clicked(object sender, EventArgs e)
        {
            base.GoHome();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetupAsync();
        }

        private async Task SetupAsync()
        {
            await SetupConfigurationAsync();
            if (ViewModel != null && !ViewModel.IsLoaded)
                await ViewModel.LoadViewModel();
        }

        private async Task SetupConfigurationAsync()
        {
            if (App.Settings.Configurations.IsNullOrEmpty())
                await App.LoadConfigurationsAsync();

            var config = App.Settings.Configurations.SingleOrDefault(p => p.Name == "Mobile.MinimumPrefixCharacters");

            if (config == null)
                config = new Configuration()
                {
                    Name = "Mobile.MinimumPrefixCharacters",
                    Value = "0"
                };
            if (config.Value == "0")
            {
                SetDefault();
            }
            else
            {
                try
                {
                    var min = int.Parse(config.Value);
                    AutoComplete.MinimumPrefixCharacters = min;
                    AutoComplete.ShowSuggestionsOnFocus = false;
                }
                catch (Exception)
                {
                    SetDefault();
                }
            }
        }

        private void SetDefault()
        {
            AutoComplete.MinimumPrefixCharacters = 0;
            AutoComplete.ShowSuggestionsOnFocus = true;
        }

    }

    public class ChoixDestinationPageViewModel : ViewModelBaseExt
    {
        public DPViewModel ViewModel { get; }

        public string StartPageInvite { get; }

        public ChoixDestinationPageViewModel(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;

        }

        public ChoixDestinationPageViewModel()
        {
        }
    }
}