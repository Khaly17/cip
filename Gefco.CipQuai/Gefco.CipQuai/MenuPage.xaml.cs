using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gefco.CipQuai.Pages;
using Gefco.CipQuai.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		public MenuPage()
		{
			InitializeComponent ();
		}

	    private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
	    {
	        ServiceNavigation.Instance.PushAsync(new DebugPage());
	        MainPage.Instance.IsPresented = false;
	    }
	}
}