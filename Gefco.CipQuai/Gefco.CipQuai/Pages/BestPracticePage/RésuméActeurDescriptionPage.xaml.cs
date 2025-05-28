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

namespace Gefco.CipQuai.BestPracticePage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RésuméActeurDescriptionPage
	{
	    private FlowState _flowState = FlowState.RésuméActeurDescription;
	    public BPViewModel ViewModel { get; }

	    public RésuméActeurDescriptionPage(BPViewModel bpViewModel)
		{
		    ViewModel = bpViewModel;
		    InitializeComponent();
		    BindingContext = new RésuméActeurDescriptionPageViewModel(bpViewModel);
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

        protected async void ShowPictureMenu(object sender, EventArgs e)
	    {
	        if (ViewModel.Picture1 == "Camera.svg")
	        {
	            BusyIndicator.IsVisible = true;
	            var animate = new Animation(d => BottomButtonsFrame.TranslationY = d, 250, 0, Easing.SpringIn);
	            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
	            animate.Commit(BottomButtonsFrame, nameof(BottomButtonsFrame));
	        }
	        else
	        {
	            IsLoading = true;
	            var page = new TakePicturesPage(ViewModel);
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
	            ViewModel.Picture1 = Picture;
	            var page = new TakePicturesPage(ViewModel);
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
                ViewModel.Picture1 = Picture;
                var page = new TakePicturesPage(ViewModel);
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

	    private void StopAndFreeDeclaration(object sender, EventArgs e)
	    {
	        CancelBottomMenu(null, null);
	        GoHome();
	    }


        #endregion

	    private async void GotoNextStep(object sender, EventArgs e)
	    {
	        if (!(BindingContext is RésuméActeurDescriptionPageViewModel viewModel))
	            return;

	        if (!viewModel.HasPicture1 && !viewModel.HasPicture2 && !viewModel.HasPicture3)
	        {
	            ShowPictureMenu(null, null);
	        }
	        else
	        {
	            IsLoading = true;
	            var page = new TakePicturesPage(ViewModel);
	            await NavigationService.PushAsync(page);
	            IsLoading = false;
	        }
        }
	}

    public class RésuméActeurDescriptionPageViewModel : ViewModelBaseExt
    {
        public BPViewModel ViewModel { get; }

        public RésuméActeurDescriptionPageViewModel()
        {
            
        }
        public RésuméActeurDescriptionPageViewModel(BPViewModel bpViewModel)
        {
            ViewModel = bpViewModel;
            DeletePictureCommand = new RelayCommand(DeletePicture, CanDeletePicture);
        }

        private void DeletePicture()
        {
            
        }

        private bool CanDeletePicture()
        {
            return true;
        }

        public bool HasPicture1 => ViewModel.Picture1 != "Camera.svg";
        public bool HasPicture2 => ViewModel.Picture2 != "Camera.svg";
        public bool HasPicture3 => ViewModel.Picture3 != "Camera.svg";

        public RelayCommand DeletePictureCommand { get; }
    }
}