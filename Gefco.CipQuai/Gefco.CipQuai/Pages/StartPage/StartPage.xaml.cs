using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Pages;
using Gefco.CipQuai.ProfilePage;
using Gefco.CipQuai.Services;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.StartPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage
	{
	    private ModifierCodePinPage _page;
        StartPageViewModel context;

	    public StartPage()
		{
			InitializeComponent();
            context = (StartPageViewModel)ServiceLocator.Current.GetInstance(typeof(StartPageViewModel));
            BindingContext = context;
		}

	    protected override void OnAppearing()
	    {
            base.OnAppearing();
            DebugFrame.TranslationY = 2500;
            App.HomeButtonPressed += AppOnHomeButtonPressed;
	        if (App.Settings.User.NeedsChangePin ?? false)
	        {
	            if (_page == null)
	            {
	                _page = new ModifierCodePinPage(new ProfilePage.ViewModel()) { ShowBackOrClose = false };
	                ServiceNavigation.Instance.PushModalAsync(_page);
	            }
	        }
            context.SetDebug();

        }
        protected override void OnDisappearing()
        {
            App.HomeButtonPressed -= AppOnHomeButtonPressed;
            base.OnDisappearing();
        }
        private void AppOnHomeButtonPressed()
        {
            context.SetDebug();
            ShowDebugMenu();
        }
        protected async void ShowDebugMenu()
        {
            BusyIndicator.IsVisible = true;
            var animate = new Animation(d => DebugFrame.TranslationY = d, DebugFrame.Height - 5, 0, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 0, 1, Easing.SpringIn);
            animate.Commit(DebugFrame, nameof(DebugFrame));
            foreach (var item in context.Items.ToList())
            {
                await item.SyncAsync();
            }
            context.SetDebug();
            if (context.Items.Any())
                return;
            CancelBottomMenu(null, null);
        }
        private void CancelBottomMenu(object sender, EventArgs e)
        {
            var animate = new Animation(d => DebugFrame.TranslationY = d, 0, DebugFrame.Height + 10, Easing.SpringIn);
            animate.WithConcurrent(d => BusyIndicator.Opacity = d, 1, 0, Easing.SpringIn);
            animate.Commit(DebugFrame, nameof(DebugFrame), finished: (d, b) => BusyIndicator.IsVisible = false);
        }
        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            CancelBottomMenu(null, null);
        }
        private void TapGestureRecognizer2_OnTapped(object sender, EventArgs e)
        {

        }
        private async void SfButton_OnClicked(object sender, EventArgs e)
        {
            //var context2 = ((SfButton)sender).BindingContext as DebugListItem;
            //await context2.SyncAsync();
            //context.HasDebug2 = context.Items.Any();
            //context.RaisePropertyChanged(nameof(context.HasDebug2));
        }
    }

}