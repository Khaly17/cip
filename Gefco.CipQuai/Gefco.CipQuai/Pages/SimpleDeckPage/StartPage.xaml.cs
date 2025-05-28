using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.SimpleDeckPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage
	{
		public StartPage()
		{
			InitializeComponent();
		    BindingContext = new SPViewModel();
		}

        private void SfButton_Clicked(object sender, EventArgs e)
        {
            base.GoHome();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await SetupAsync();
        }
        public SPViewModel ViewModel => BindingContext as SPViewModel;

	    private async Task SetupAsync()
	    {
	        if (ViewModel != null && !ViewModel.IsLoaded)
	            await ViewModel.LoadViewModel();
	    }



        private void AutoComplete_OnCompleted(object sender, EventArgs e)
	    {
	        if (ViewModel.ValidateDestinationCommand.CanExecute(null))
	            ViewModel.ValidateDestinationCommand.Execute(null);
	    }
	}

}