
using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.DoubleDeckPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeclarationPage : ExtContentPage
    {
        public DPViewModel ViewModel { get; }

        public DeclarationPage(DPViewModel dpViewModel)
        {
            InitializeComponent();
            var model = new DeclarationPageViewModel(dpViewModel);
            model.CancelGoBack += ModelOnCancelGoBack;
            BindingContext = model;
            ViewModel = dpViewModel;
        }

        private void ModelOnCancelGoBack()
        {
            ShowCancelMenu(null, null);
        }

        private void AppOnHomeButtonPressed()
        {
            ShowCancelMenu(null, null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.BackButtonPressed -= AppOnBackButtonPressed;
            App.HomeButtonPressed += AppOnHomeButtonPressed;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.BackButtonPressed -= AppOnBackButtonPressed;
            App.HomeButtonPressed += AppOnHomeButtonPressed;
        }

        private void AppOnBackButtonPressed(CancelEventArgs args)
        {
            args.Cancel = true;
            ShowCancelMenu(null, null);
        }

        private void GotoNextAction(object sender, EventArgs e)
        {
            if (ViewModel.IsDpUsed ?? true)
                GotoTakePictures(sender, e);
            else
                GotoCantTakePictures(sender, e);
        }
        private async void GotoTakePictures(object sender, EventArgs e)
        {
            IsLoading = true;
            ViewModel.IsLoading = true;
            ViewModel.IsDpUsed = true;
            //if (ViewModel.State >= FlowState.DeclarationImpossible)
            //{
            //    ViewModel.Picture1
            //}
            var page = new TakePicturesPage(ViewModel);
            await NavigationService.PushAsync(page);
            ViewModel.IsLoading = false;
            IsLoading = false;
        }
        private async void GotoCantTakePictures(object sender, EventArgs e)
        {
            IsLoading = true;
            ViewModel.IsLoading = true;
            ViewModel.IsDpUsed = false;
            //if (ViewModel.State >= FlowState.TakePictures && ViewModel.State < FlowState.DeclarationImpossible)
            //{

            //}
            var page = new DeclarationImpossiblePage(ViewModel);
            await NavigationService.PushAsync(page);
            ViewModel.IsLoading = false;
            IsLoading = false;
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

        private void SuspendDeclaration(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
            ViewModel.SuspendDeclaration(FlowState.Declaration);
            GoHome();
        }

        private void StopAndFreeDeclaration(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
            ViewModel.StopAndFreeDeclaration(FlowState.Declaration);
            GoHome();
        }


        #endregion
    }

    public class DeclarationPageViewModel : ViewModelBaseExt
    {
        public DPViewModel ViewModel { get; }

        public RelayCommand CanGoHomeCommand { get; }

        public DeclarationPageViewModel()
        {
            
        }
        public DeclarationPageViewModel(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            CanGoHomeCommand = new RelayCommand(GoHome);
        }

        public event Action CancelGoBack;
        private void GoHome()
        {
            CancelGoBack?.Invoke();
        }
    }
}