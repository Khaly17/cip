using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.LoginPage.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : ContentView
	{
		public LoginView ()
		{
			InitializeComponent ();

        }

	    private void UserEntry_OnCompleted(object sender, EventArgs e)
	    {
	        PassEntry.Focus();

	    }
	}
}