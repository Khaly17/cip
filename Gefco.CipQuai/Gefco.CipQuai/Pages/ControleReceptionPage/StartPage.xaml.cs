using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Extensions;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ValueChangedEventArgs = Syncfusion.SfAutoComplete.XForms.ValueChangedEventArgs;

namespace Gefco.CipQuai.ControleReceptionPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage
	{
		public StartPage()
		{
			InitializeComponent();
		    BindingContext = new CRViewModel();
            AutoComplete.DataSource.GroupDescriptors.Add(new GroupDescriptor()
                                                         {
                                                             PropertyName = "Agence",
                                                             KeySelector = (object obj1) =>
                                                                           {
                                                                               var item = (obj1 as Agence);
                                                                               return item.Name[0].ToString();
                                                                           }
                                                         });
            AutoComplete.DataSource.Filter = o =>
                                             {
                                                 if (OldAutoComplete.Text.IsNullOrWhiteSpace())
                                                     return true;
                                                 if (o is Agence agence)
                                                     return agence.Name.ToLower().Contains(OldAutoComplete.Text.ToLower());
                                                 return false;
                                             };
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
        public CRViewModel ViewModel => BindingContext as CRViewModel;

	    private async Task SetupAsync()
	    {
	        if (ViewModel != null && !ViewModel.IsLoaded)
	            await ViewModel.LoadViewModel();
	    }

        private void OldAutoComplete_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.Value != ViewModel.SelectedDestination?.Name)
                ViewModel.SelectedDestination = null;
            AutoComplete.DataSource.RefreshFilter();
        }
    }

}