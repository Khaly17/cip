using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.StartPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdatePage
    {
        private readonly StartPageViewModel _startPageViewModel;

	    public UpdatePage(StartPageViewModel startPageViewModel)
		{
		    InitializeComponent();
            _startPageViewModel = startPageViewModel;
		    BindingContext = startPageViewModel;
		}
        private void OnAppearing(object sender, EventArgs e)
	    {
	        MyEntry.Focus();
	        App.BackButtonPressed += AppOnBackButtonPressed;
	    }

	    protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        App.BackButtonPressed -= AppOnBackButtonPressed;
	    }

        private void AppOnBackButtonPressed(CancelEventArgs cancelEventArgs)
	    {
	        cancelEventArgs.Cancel = App.Settings.User.NeedsChangePin ?? false;
	    }
	}
}