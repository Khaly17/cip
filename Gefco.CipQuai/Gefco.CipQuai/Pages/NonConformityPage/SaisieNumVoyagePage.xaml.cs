using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.NonConformityPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaisieNumVoyagePage
    {
        private readonly NCViewModel _ncViewModel;

	    public SaisieNumVoyagePage(NCViewModel ncViewModel)
		{
		    InitializeComponent();
            _ncViewModel = ncViewModel;
		    BindingContext = ncViewModel;
		}
        private void OnAppearing(object sender, EventArgs e)
	    {
	        MyEntry.Focus();
	    }


        private void AppOnHomeButtonPressed()
        {
            ShowCancelMenu(null, null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.HomeButtonPressed -= AppOnHomeButtonPressed;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.HomeButtonPressed += AppOnHomeButtonPressed;
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
}