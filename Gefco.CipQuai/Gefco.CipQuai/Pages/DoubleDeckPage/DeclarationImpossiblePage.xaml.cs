using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.DoubleDeckPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeclarationImpossiblePage
	{
	    private FlowState _flowState = FlowState.DeclarationImpossible;
	    public DPViewModel ViewModel { get; }

	    public DeclarationImpossiblePage(DPViewModel dpViewModel)
		{
		    ViewModel = dpViewModel;
		    InitializeComponent();
		    BindingContext = new DeclarationImpossiblePageViewModel(dpViewModel);
		}

	    private void AppOnHomeButtonPressed()
	    {
	        ShowCancelMenu(null, null);
	    }

        protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        App.HomeButtonPressed -= AppOnHomeButtonPressed;
	        (BindingContext as DeclarationImpossiblePageViewModel).Dispose();
	    }

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
	        App.HomeButtonPressed += AppOnHomeButtonPressed;
	        (BindingContext as DeclarationImpossiblePageViewModel).Setup();
	    }

	    protected async void ShowPictureMenu(object sender, EventArgs e)
	    {
	        if (ViewModel.MotifsDP.All(p => !p.IsChecked))
	            return;
	        if (ViewModel.ErrorPicture1 == "Camera.svg")
	        {
	            BusyIndicator.IsVisible = true;
	            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
	            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
	            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
	        }
	        else
	        {
	            IsLoading = true;
	            var page = new TakeErrorPicturesPage(ViewModel);
	            await NavigationService.PushAsync(page);
	            IsLoading = false;
	        }
        }

        protected override async void TakePicture(object sender, EventArgs e)
	    {
	        CancelPictureMenu(null, null);
	        ViewModel.IsLoading = true;
	        IsLoading = true;

            await base.TakePictureAsync();
	        
	        if (IsPictureAvailable)
	        {
	            ViewModel.ErrorPicture1 = Picture;
	            await ViewModel.SaveDeclarationAsync(FlowState.TakeErrorPictures);
	            var page = new TakeErrorPicturesPage(ViewModel);
	            await NavigationService.PushAsync(page);
	        }

	        IsLoading = false;
            ViewModel.IsLoading = false;
	    }

	    protected override async void ChoosePicture(object sender, EventArgs e)
	    {
	        CancelPictureMenu(null, null);
	        ViewModel.IsLoading = true;
	        IsLoading = true;

            await base.ChoosePictureAsync();

            if (IsPictureAvailable)
            {
                ViewModel.ErrorPicture1 = Picture;
                await ViewModel.SaveDeclarationAsync(FlowState.TakeErrorPictures);
                var page = new TakeErrorPicturesPage(ViewModel);
                await NavigationService.PushAsync(page);
            }

	        IsLoading = false;
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

	    private async void GotoNextStep(object sender, EventArgs e)
	    {
	        if (!(BindingContext is DeclarationImpossiblePageViewModel viewModel))
	            return;
            if (ViewModel.MotifsDP.All(p => !p.IsChecked))
                return;
	        if (!ViewModel.NeedsPicture)
	        {
	            IsLoading = true;
	            await ViewModel.SaveDeclarationAsync(FlowState.TakeErrorPictures);
	            var page = new DPNotUsedSummaryPage(ViewModel);
	            await NavigationService.PushAsync(page);
                IsLoading = false;
            }
            else if ((!viewModel.HasPicture1 && !viewModel.HasPicture2 && !viewModel.HasPicture3))
	        {
	            ShowPictureMenu(null, null);
	        }
	        else
	        {
	            IsLoading = true;
	            await ViewModel.SaveDeclarationAsync(FlowState.TakeErrorPictures);
	            var page = new TakeErrorPicturesPage(ViewModel);
	            await NavigationService.PushAsync(page);
	            IsLoading = false;
	        }
        }
	}

    public class DeclarationImpossiblePageViewModel : ViewModelBaseExt
    {
        public DPViewModel ViewModel { get; }

        public void Setup()
        {
            MotifDPViewModel.Checked -= MotifDPViewModelOnChecked;
            MotifDPViewModel.Checked += MotifDPViewModelOnChecked;
        }
        public void Dispose()
        {
            MotifDPViewModel.Checked -= MotifDPViewModelOnChecked;
        }
        public DeclarationImpossiblePageViewModel()
        {
            
        }
        public DeclarationImpossiblePageViewModel(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            Setup();
            DeletePictureCommand = new RelayCommand(DeletePicture, CanDeletePicture);
        }

        private void DeletePicture()
        {
            
        }

        private bool CanDeletePicture()
        {
            return true;
        }

        public bool HasPicture1 => ViewModel.ErrorPicture1 != "Camera.svg";
        public bool HasPicture2 => ViewModel.ErrorPicture2 != "Camera.svg";
        public bool HasPicture3 => ViewModel.ErrorPicture3 != "Camera.svg";

        private void MotifDPViewModelOnChecked(MotifDPViewModel viewModel)
        {
            if (viewModel.MotifDP.IsNbDP)
            {
                if (!IsLoading)
                {
                    if (viewModel.IsChecked)
                    {
                        GotoSaisieNbBarresDPCassees(viewModel);
                    }
                }
            }
            else if (viewModel.MotifDP.IsOther)
            {
                if (!IsLoading)
                {
                    if (viewModel.IsChecked)
                    {
                        GotoSaisieAutreMotifDP(viewModel);
                    }
                    //else
                    //{
                    //    AutreMotifDP = "";
                    //}
                }
            }
            RaisePropertyChanged(nameof(ViewModel.NeedsPicture));
        }
        private async void GotoSaisieNbBarresDPCassees(MotifDPViewModel viewModel)
        {
            IsLoading = true;
            ViewModel.IsSuccessInput = false;

            var page = new SaisieNbBarresDPCasseesPage(this.ViewModel, viewModel);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }
        private async void GotoSaisieAutreMotifDP(MotifDPViewModel viewModel)
        {
            IsLoading = true;
            ViewModel.IsSuccessInput = false;

            var page = new SaisieAutreMotifDPPage(this.ViewModel, viewModel);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }

        public RelayCommand DeletePictureCommand { get; }
    }
}