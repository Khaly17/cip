
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

namespace Gefco.CipQuai.NonConformityPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChoixProvenancePage
    {
        public NCViewModel ViewModel { get; }

        public ChoixProvenancePage(NCViewModel ncViewModel)
        {
            ViewModel = ncViewModel;
            InitializeComponent();
            BindingContext = new ChoixProvenancePageViewModel(ncViewModel);
        }

        private void AutoComplete_OnCompleted(object sender, EventArgs e)
        {
            if (ViewModel.ValidateProvenanceCommand.CanExecute(null))
                ViewModel.ValidateProvenanceCommand.Execute(null);
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

    public class ChoixProvenancePageViewModel : ViewModelBaseExt
    {
        public NCViewModel ViewModel { get; }

        public string StartPageInvite { get; }

        public List<Agence> Provenances { get; } = new List<Agence>();


        public ChoixProvenancePageViewModel(NCViewModel ncViewModel)
        {
            ViewModel = ncViewModel;
            switch (ncViewModel.ProvenanceType)
            {
                case ProvenanceType.France:
                    StartPageInvite = ViewModel.GefcoButtonLabel;
                    Provenances = ViewModel.Provenances.Where(p => p.IsGefcoFrance).ToList();
                    break;
                case ProvenanceType.International:
                    StartPageInvite = ViewModel.InternationalButtonLabel;
                    Provenances = ViewModel.Provenances.Where(p => p.IsInternational).ToList();
                    break;
                case ProvenanceType.Confrères:
                    StartPageInvite = ViewModel.ConfrèresButtonLabel;
                    Provenances = ViewModel.Provenances.Where(p => p.IsClient).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


        }

        public ChoixProvenancePageViewModel()
        {
        }
    }
}