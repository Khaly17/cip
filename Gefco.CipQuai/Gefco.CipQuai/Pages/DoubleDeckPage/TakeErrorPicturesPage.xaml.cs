
using System;
using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.Controls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.DoubleDeckPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakeErrorPicturesPage
    {
        private FlowState _flowState = FlowState.TakeErrorPictures;
        public DPViewModel ViewModel { get; }

        public TakeErrorPicturesPage(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new TakeErrorPicturesPageViewmodel(dpViewModel);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.HomeButtonPressed += AppOnHomeButtonPressed;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.HomeButtonPressed -= AppOnHomeButtonPressed;
        }

        private void AppOnHomeButtonPressed()
        {
            ShowCancelMenu(null, null);
        }

        private async void EndDeclaration(object sender, EventArgs e)
        {
            IsLoading = true;
            if (!ViewModel.NeedsPicture)
            {
                await ViewModel.SaveDeclarationAsync(FlowState.DPNotUsedSummary);
                var page = new DPNotUsedSummaryPage(ViewModel);
                await NavigationService.PushAsync(page);
            }
            else if (ViewModel.ErrorPicture1 != "Camera.svg" || ViewModel.ErrorPicture2 != "Camera.svg" || ViewModel.ErrorPicture3 != "Camera.svg")
            {
                await ViewModel.SaveDeclarationAsync(FlowState.DPNotUsedSummary);
                var page = new DPNotUsedSummaryPage(this.ViewModel);
                await NavigationService.PushAsync(page);
            }
            IsLoading = false;
        }

        private ActionTypes ActionType { get; set; }
        private enum ActionTypes
        {
            SelectPictureAndShowMenu
        }
        protected void SelectPictureAndShowMenu(object sender, EventArgs e)
        {
            var @enum = (ErrorPictureEnum)(sender as ExtImage).CommandParameter;
            if (!(BindingContext is TakeErrorPicturesPageViewmodel viewModel))
                return;
            var previousItem = viewModel.SelectedPicture;
            viewModel.SelectedPicture = @enum;
            switch (@enum)
            {
                case ErrorPictureEnum.ErrorPicture1:
                    viewModel.CurrentPicture = ViewModel.ErrorPicture1;
                    break;
                case ErrorPictureEnum.ErrorPicture2:
                    viewModel.CurrentPicture = ViewModel.ErrorPicture2;
                    break;
                case ErrorPictureEnum.ErrorPicture3:
                    viewModel.CurrentPicture = ViewModel.ErrorPicture3;
                    break;
            }
            if (viewModel.CurrentPicture != "Camera.svg" && previousItem != @enum)
                return;
            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
        }
        protected void ShowPictureMenu(object sender, EventArgs e)
        {
            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
        }
        protected void TakeNextPicture(object sender, EventArgs e)
        {
            //todo keep
            if (!(BindingContext is TakeErrorPicturesPageViewmodel viewModel))
                return;
            if (ViewModel.ErrorPicture1 == "Camera.svg")
                viewModel.SelectedPicture = ErrorPictureEnum.ErrorPicture1;
            else if (ViewModel.ErrorPicture2 == "Camera.svg")
                viewModel.SelectedPicture = ErrorPictureEnum.ErrorPicture2;
            else if (ViewModel.ErrorPicture3 == "Camera.svg")
                viewModel.SelectedPicture = ErrorPictureEnum.ErrorPicture3;

            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
        }

        protected override async void TakePicture(object sender, EventArgs e)
        {
            CancelPictureMenu(null, null);
            ViewModel.IsLoading = true;

            await base.TakePictureAsync();

            if (IsPictureAvailable)
            {
                if (!(BindingContext is TakeErrorPicturesPageViewmodel viewModel))
                    return;
                switch (viewModel.SelectedPicture)
                {
                    case ErrorPictureEnum.ErrorPicture1:
                        ViewModel.ErrorPicture1 = Picture;
                        break;
                    case ErrorPictureEnum.ErrorPicture2:
                        ViewModel.ErrorPicture2 = Picture;
                        break;
                    case ErrorPictureEnum.ErrorPicture3:
                        ViewModel.ErrorPicture3 = Picture;
                        break;
                }
                viewModel.CurrentPicture = Picture;
                viewModel.Refresh();
            }
            ViewModel.IsLoading = false;
        }

        protected override async void ChoosePicture(object sender, EventArgs e)
        {
            CancelPictureMenu(null, null);
            ViewModel.IsLoading = true;

            await base.ChoosePictureAsync();

            if (IsPictureAvailable)
            {
                if (!(BindingContext is TakeErrorPicturesPageViewmodel viewModel))
                    return;
                switch (viewModel.SelectedPicture)
                {
                    case ErrorPictureEnum.ErrorPicture1:
                        ViewModel.ErrorPicture1 = Picture;
                        break;
                    case ErrorPictureEnum.ErrorPicture2:
                        ViewModel.ErrorPicture2 = Picture;
                        break;
                    case ErrorPictureEnum.ErrorPicture3:
                        ViewModel.ErrorPicture3 = Picture;
                        break;
                }
                await ViewModel.SaveDeclarationAsync(_flowState);
                viewModel.CurrentPicture = Picture;
                viewModel.Refresh();
            }

            ViewModel.IsLoading = false;
        }

        private void CancelPictureMenu(object sender, EventArgs e)
        {
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 0, 250, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 1, 0, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame), finished: (d, b) => BusyIndicator.IsVisible = false);
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
            ViewModel.SuspendDeclaration(_flowState);
            GoHome();
        }

        private void StopAndFreeDeclaration(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
            ViewModel.StopAndFreeDeclaration(_flowState);
            GoHome();
        }


        #endregion
    }
    public class TakeErrorPicturesPageViewmodel : ViewModelBaseExt
    {
        public DPViewModel ViewModel { get; }

        private string _currentPicture = "Camera.svg";

        public string CurrentPicture
        {
            get { return _currentPicture; }
            set
            {
                if (value == _currentPicture)
                    return;
                _currentPicture = value;
                OnPropertyChanged();
            }
        }
        private string _nextButtonInvite = "Suivant >";

        public string NextButtonLabel
        {
            get { return _nextButtonInvite; }
            set
            {
                if (value == _nextButtonInvite)
                    return;
                _nextButtonInvite = value;
                OnPropertyChanged();
            }
        }

        public TakeErrorPicturesPageViewmodel()
        {
        }
        public TakeErrorPicturesPageViewmodel(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            CurrentPicture = ViewModel.ErrorPicture1;
        }

        public bool CanRetakePicture
        {
            get
            {
                switch (SelectedPicture)
                {
                    case ErrorPictureEnum.ErrorPicture1:
                        return ViewModel != null && ViewModel?.ErrorPicture1 != "Camera.svg";
                    case ErrorPictureEnum.ErrorPicture2:
                        return ViewModel != null && ViewModel?.ErrorPicture2 != "Camera.svg";
                    case ErrorPictureEnum.ErrorPicture3:
                        return ViewModel != null && ViewModel?.ErrorPicture3 != "Camera.svg";
                }
                return false;
            }
        }

        public bool CanTakeNextPicture => ViewModel != null && (ViewModel.ErrorPicture1 == "Camera.svg" || ViewModel.ErrorPicture2 == "Camera.svg" || ViewModel.ErrorPicture3 == "Camera.svg");
        

        private ErrorPictureEnum _selectedPicture = ErrorPictureEnum.ErrorPicture1;

        public ErrorPictureEnum SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                if (value == _selectedPicture)
                    return;
                _selectedPicture = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRetakePicture));
                OnPropertyChanged(nameof(CanTakeNextPicture));
                
            }
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(CanRetakePicture));
            OnPropertyChanged(nameof(CanTakeNextPicture));
        }
    }

    public enum ErrorPictureEnum
    {
        ErrorPicture1,
        ErrorPicture2,
        ErrorPicture3,
    }
}