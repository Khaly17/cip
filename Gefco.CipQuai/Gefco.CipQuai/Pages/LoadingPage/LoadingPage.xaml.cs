using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.LoadingPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPage
    {
        private DependencyRegistratorBase _registrator;

        public LoadingPage(DependencyRegistratorBase registrator)
        {
            _registrator = registrator;
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance(typeof(ViewModel));
        }
        
        public ViewModel ViewModel => BindingContext as ViewModel;

        protected override void OnAppearing()
        {
            ViewModel.Loaded += ViewModelOnLoaded;
            ViewModel.Load(_registrator);
            base.OnAppearing();
            //await App.Toaster.Snack($"App.Width: {App.Width}, App.Height: {App.Height}"); //800x1232 rockchip //411x797 s9 //360x672 wiko view
        }
        private void ViewModelOnLoaded()
        {
            if (App.Current.MainPage is MainPage)
                return;
            var page = new MainPage();
            App.Current.MainPage = (Page) page;
        }
    }

}