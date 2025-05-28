using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.DoubleDeckPage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.NonConformityPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChoixMotifPage : ExtTakePicturePage
    {
	    private FlowState _flowState = FlowState.ChoixMotif;

        public NCViewModel ViewModel { get; }

	    public ChoixMotifPage(NCViewModel ncViewModel)
		{
		    ViewModel = ncViewModel;
		    InitializeComponent();
		    BindingContext = new ChoixMotifPageViewModel(ncViewModel);
		}

	    private void AppOnHomeButtonPressed()
	    {
	        ShowCancelMenu(null, null);
	    }

        protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
		        App.HomeButtonPressed -= AppOnHomeButtonPressed;
        (BindingContext as ChoixMotifPageViewModel).Dispose();
	    }

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
	        App.HomeButtonPressed += AppOnHomeButtonPressed;
	        (BindingContext as ChoixMotifPageViewModel).Setup();
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

	    protected async void ShowPictureMenu(object sender, EventArgs e)
	    {
	        if (ViewModel.MotifNCs.All(p => !p.IsChecked))
	            return;
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

        private void GotoNextStep(object sender, EventArgs e)
	    {
	        if (!(BindingContext is ChoixMotifPageViewModel viewModel))
	            return;

	        if (ViewModel.MotifNCs.Any(p => p.IsChecked))
	        {
                //var page = new TakePicturesPage(ViewModel);
                //await NavigationService.PushAsync(page);
	            ShowPictureMenu(null, null);
	        }
        }
	}

    public class ChoixMotifPageViewModel : ViewModelBaseExt
    {
        public NCViewModel ViewModel { get; }

        public void Setup()
        {
            MotifNCViewModel.Checked -= MotifNCViewModelOnChecked;
            MotifNCViewModel.Checked += MotifNCViewModelOnChecked;
        }
        public void Dispose()
        {
            MotifNCViewModel.Checked -= MotifNCViewModelOnChecked;
        }
        public ChoixMotifPageViewModel()
        {
            
        }
        public ChoixMotifPageViewModel(NCViewModel ncViewModel)
        {
            ViewModel = ncViewModel;
            Setup();
        }

        private void MotifNCViewModelOnChecked(MotifNCViewModel viewModel)
        {
            switch (viewModel.MotifNC.Name)
            {
                case "Autre":
                    if (!IsLoading)
                    {
                        if (viewModel.IsChecked)
                        {
                            GotoSaisieAutreMotifNC(viewModel);
                        }
                    }
                    break;
            }
        }
        private async void GotoSaisieAutreMotifNC(MotifNCViewModel viewModel)
        {
            IsLoading = true;
            ViewModel.IsSuccessInput = false;

            var page = new SaisieAutreMotifNCPage(this.ViewModel, viewModel);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }

        public RelayCommand DeletePictureCommand { get; }
    }
}