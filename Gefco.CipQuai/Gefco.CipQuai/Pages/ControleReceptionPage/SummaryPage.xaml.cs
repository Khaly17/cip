using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;

namespace Gefco.CipQuai.ControleReceptionPage
{
    public partial class SummaryPage
    {
        public CRViewModel ViewModel { get; }

        public SummaryPage(CRViewModel CRViewModel)
        {
            ViewModel = CRViewModel;
            InitializeComponent();
            BindingContext = new SummaryPageViewModel(CRViewModel);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.HomeButtonPressed += AppOnHomeButtonPressed;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.HomeButtonPressed += AppOnHomeButtonPressed;
        }

        private void AppOnHomeButtonPressed()
        {
            ShowCancelMenu(null, null);
        }

        #region Bottom Menu

        protected void ShowCancelMenu(object sender, EventArgs e)
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

        private void StopAndFreeDeclaration(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
            GoHome();
        }


        #endregion    }
    }

    public class SummaryPageViewModel : ViewModelBaseExt
    {
        public CRViewModel ViewModel { get; }

        public SummaryPageViewModel()
        {
            
        }
        public SummaryPageViewModel(CRViewModel CRViewModel)
        {
            ViewModel = CRViewModel;
            EndDeclarationCommand = new RelayCommand(EndDeclaration, CanEndDeclaration);
        }

        public RelayCommand EndDeclarationCommand { get; set; }
        private async void EndDeclaration()
        {
            IsLoading = true;
            ViewModel.EndDeclaration();
            var page = new EndSessionPage(ViewModel);
            await NavigationService.PushAsync(page);
            IsLoading = false;
            NavigationService.ClearAllButFirstAndLast();
        }

        private bool CanEndDeclaration()
        {
            return true;
        }

    }
}
