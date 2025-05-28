using System;
using System.Linq;
using System.Threading.Tasks;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Extensions;
using Xamarin.Forms;

namespace Gefco.CipQuai.DoubleDeckPage
{
    public partial class DoubleDeckPage
    {
        public DoubleDeckPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.DeclarationResuming += ViewModelOnDeclarationResuming;
            await SetupAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.DeclarationResuming -= ViewModelOnDeclarationResuming;
        }

        private void ViewModelOnDeclarationResuming()
        {
            ShowMenu();
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

        public DPViewModel ViewModel => BindingContext as DPViewModel;

        private void AutoComplete_OnCompleted(object sender, EventArgs e)
        {
            if (ViewModel.ValidateTractionCommand.CanExecute(null))
                ViewModel.ValidateTractionCommand.Execute(null);
        }

        #region Bottom Menu

        protected void ShowMenu()
        {
            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => CancelButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(CancelButtonsFrame, nameof(CancelButtonsFrame));
        }
        private void CancelBottomMenu(object sender, EventArgs e)
        {
            var animate = new Animation(d => CancelButtonsFrame.TranslationY = d, 0, 250, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 1, 0, Easing.SpringIn);
            animate.Commit(CancelButtonsFrame, nameof(CancelButtonsFrame), finished: (d, b) => BusyIndicator.IsVisible = false);
        }

        private void ResumeDeclaration(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
            ViewModel.ResumeDeclaration();
        }

        private void RestartDeclaration(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
            ViewModel.RestartDeclaration();
        }


        #endregion
    }
}
