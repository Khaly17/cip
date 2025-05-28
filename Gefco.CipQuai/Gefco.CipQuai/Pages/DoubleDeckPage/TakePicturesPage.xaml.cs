
using System;
using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.Controls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.DoubleDeckPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakePicturesPage
    {
        private FlowState _flowState = FlowState.TakePictures;
        public DPViewModel ViewModel { get; }
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

        public TakePicturesPage(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            InitializeComponent();
            BindingContext = new TakePicturesPageViewmodel(dpViewModel);
        }
        private void GotoNextStep(object sender, EventArgs e)
        {
            if (!(BindingContext is TakePicturesPageViewmodel viewModel))
                return;

            if (!viewModel.HasPicture1)
            {
                TakeNextPicture(null, null);
            }
            else if (!viewModel.HasPicture2)
            {
                TakeNextPicture(null, null);
            }
            else
            {
                EndDeclaration(null, null);
            }
        }
        private async void EndDeclaration(object sender, EventArgs e)
        {
            if (ViewModel.Picture1 != "Camera.svg" || ViewModel.Picture2 != "Camera.svg")
            {
                IsLoading = true;
                await ViewModel.SaveDeclarationAsync(FlowState.DPUsedSummary);
                var page = new DPUsedSummaryPage(this.ViewModel);
                await NavigationService.PushAsync(page);
                IsLoading = false;
            }
        }

        protected void SelectPictureAndShowMenu(object sender, EventArgs e)
        {
            var @enum = (PictureEnum)(sender as SfButton).CommandParameter;
            if (!(BindingContext is TakePicturesPageViewmodel viewModel))
                return;
            
            if (@enum == PictureEnum.Picture2 && !viewModel.HasPicture1)
                return;
                
            viewModel.SelectedPicture = @enum;
            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
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
                }
                await ViewModel.SaveDeclarationAsync(_flowState);
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
                }
                await ViewModel.SaveDeclarationAsync(_flowState);
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

        private void DeletePicture(object sender, EventArgs e)
        {
            CancelPictureMenu(null, null);
            if (!(BindingContext is TakePicturesPageViewmodel viewModel))
                return;
            if (viewModel.SelectedPicture == PictureEnum.Picture1)
                ViewModel.Picture1 = "Camera.svg";//todo delete ?
            else
                ViewModel.Picture2 = "Camera.svg";//todo delete ?
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
    public class TakePicturesPageViewmodel : ViewModelBaseExt
    {
        public DPViewModel ViewModel { get; }

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
        public TakePicturesPageViewmodel(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
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
                }
                return false;
            }
        }

        public bool CanTakeNextPicture => ViewModel != null && (ViewModel.Picture1 == "Camera.svg" || ViewModel.Picture2 == "Camera.svg");
        

        private PictureEnum _selectedPicture = PictureEnum.Picture1;

        public PictureEnum SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                _selectedPicture = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRetakePicture));
                OnPropertyChanged(nameof(CanTakeNextPicture));
                OnPropertyChanged(nameof(CanDeletePicture));
                
            }
        }

        public bool HasPicture1 => ViewModel.Picture1 != "Camera.svg";
        public bool HasPicture2 => ViewModel.Picture2 != "Camera.svg";

        public bool CanDeletePicture => ViewModel != null && (SelectedPicture == PictureEnum.Picture1 ? ViewModel.Picture1 != "Camera.svg" : ViewModel.Picture2 != "Camera.svg");

        public void Refresh()
        {
            OnPropertyChanged(nameof(CanRetakePicture));
            OnPropertyChanged(nameof(CanTakeNextPicture));
        }
    }

    public enum PictureEnum
    {
        Picture1,
        Picture2
    }
}