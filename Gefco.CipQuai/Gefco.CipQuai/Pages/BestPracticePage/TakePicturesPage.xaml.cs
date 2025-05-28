
using System;
using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.Controls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.BestPracticePage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakePicturesPage
    {
        private FlowState _flowState = FlowState.TakePictures;
        public BPViewModel ViewModel { get; }

        public TakePicturesPage(BPViewModel bpViewModel)
        {
            ViewModel = bpViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new TakePicturesPageViewmodel(bpViewModel);
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
            if (ViewModel.Picture1 != "Camera.svg" || ViewModel.Picture2 != "Camera.svg" || ViewModel.Picture3 != "Camera.svg")
            {
                IsLoading = true;
                var page = new SummaryPage(this.ViewModel);
                await NavigationService.PushAsync(page);
                IsLoading = false;
            }
        }

        protected void SelectPictureAndShowMenu(object sender, EventArgs e)
        {
            var @enum = (PictureEnum)(sender as ExtImage).CommandParameter;
            if (!(BindingContext is TakePicturesPageViewmodel viewModel))
                return;
            var previousItem = viewModel.SelectedPicture;
            viewModel.SelectedPicture = @enum;
            switch (@enum)
            {
                case PictureEnum.Picture1:
                    viewModel.CurrentPicture = ViewModel.Picture1;
                    break;
                case PictureEnum.Picture2:
                    viewModel.CurrentPicture = ViewModel.Picture2;
                    break;
                case PictureEnum.Picture3:
                    viewModel.CurrentPicture = ViewModel.Picture3;
                    break;
            }
            if (viewModel.CurrentPicture != "Camera.svg" && previousItem != @enum)
                return;
            ShowMenu();
        }

        private void ShowMenu()
        {
            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
        }

        protected void ShowPictureMenu(object sender, EventArgs e)
        {
            ShowMenu();
        }
        protected void TakeNextPicture(object sender, EventArgs e)
        {
            //todo keep
            if (!(BindingContext is TakePicturesPageViewmodel viewModel))
                return;
            if (ViewModel.Picture1 == "Camera.svg")
                viewModel.SelectedPicture = PictureEnum.Picture1;
            else if (ViewModel.Picture2 == "Camera.svg")
                viewModel.SelectedPicture = PictureEnum.Picture2;
            else if (ViewModel.Picture3 == "Camera.svg")
                viewModel.SelectedPicture = PictureEnum.Picture3;

            ShowMenu();
        }

        protected override async void TakePicture(object sender, EventArgs e)
        {
            CancelPictureMenu(null, null);
            ViewModel.IsLoading = true;

            await base.TakePictureAsync();

            if (IsPictureAvailable)
            {
                if (!(BindingContext is TakePicturesPageViewmodel viewModel))
                    return;
                switch (viewModel.SelectedPicture)
                {
                    case PictureEnum.Picture1:
                        ViewModel.Picture1 = Picture;
                        break;
                    case PictureEnum.Picture2:
                        ViewModel.Picture2 = Picture;
                        break;
                    case PictureEnum.Picture3:
                        ViewModel.Picture3 = Picture;
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
                if (!(BindingContext is TakePicturesPageViewmodel viewModel))
                    return;
                switch (viewModel.SelectedPicture)
                {
                    case PictureEnum.Picture1:
                        ViewModel.Picture1 = Picture;
                        break;
                    case PictureEnum.Picture2:
                        ViewModel.Picture2 = Picture;
                        break;
                    case PictureEnum.Picture3:
                        ViewModel.Picture3 = Picture;
                        break;
                }
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

        private void StopAndFreeDeclaration(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
            GoHome();
        }


        #endregion
    }
    public class TakePicturesPageViewmodel : ViewModelBaseExt
    {
        public BPViewModel ViewModel { get; }

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

        public TakePicturesPageViewmodel()
        {
        }
        public TakePicturesPageViewmodel(BPViewModel bpViewModel)
        {
            ViewModel = bpViewModel;
            CurrentPicture = ViewModel.Picture1;
        }

        public bool CanRetakePicture
        {
            get
            {
                switch (SelectedPicture)
                {
                    case PictureEnum.Picture1:
                        return ViewModel != null && ViewModel?.Picture1 != "Camera.svg";
                    case PictureEnum.Picture2:
                        return ViewModel != null && ViewModel?.Picture2 != "Camera.svg";
                    case PictureEnum.Picture3:
                        return ViewModel != null && ViewModel?.Picture3 != "Camera.svg";
                }
                return false;
            }
        }

        public bool CanTakeNextPicture => ViewModel != null && (ViewModel.Picture1 == "Camera.svg" || ViewModel.Picture2 == "Camera.svg" || ViewModel.Picture3 == "Camera.svg");
        

        private PictureEnum _selectedPicture = PictureEnum.Picture1;

        public PictureEnum SelectedPicture
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

    public enum PictureEnum
    {
        Picture1,
        Picture2,
        Picture3,
    }
}