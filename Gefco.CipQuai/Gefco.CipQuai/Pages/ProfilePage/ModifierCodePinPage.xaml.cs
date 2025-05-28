using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.ProfilePage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifierCodePinPage
	{
        private readonly ViewModel _viewModel;

	    public ModifierCodePinPage(ViewModel viewModel)
		{
		    InitializeComponent();
            _viewModel = viewModel;
		    BindingContext = viewModel;
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