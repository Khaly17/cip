using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.NonConformityPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage
	{
		public StartPage()
		{
			InitializeComponent();
		    BindingContext = new NCViewModel();
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
        public NCViewModel ViewModel => BindingContext as NCViewModel;

        private async Task SetupAsync()
        {
            if (ViewModel != null && !ViewModel.IsLoaded)
                await ViewModel.LoadViewModel();
        }

    }

}