using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Gefco.CipQuai.Menu;
using Gefco.CipQuai.Services;
using Xamarin.Forms;

namespace Gefco.CipQuai
{
    public partial class MainPage
    {
        /// <summary>
        ///     The title page selected.
        /// </summary>
        private string _titlePageSelected;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            _loginService = SimpleIoc.Default.GetInstance<ILoginService>();

            MenuPageViewModel.Instance.PropertyChanged += MenuOnPropertyChanged;
            App.MenuPressed += AppOnMenuPressed;
            _loginService.LoggingOut += LoginServiceOnLoggingOut;
            _reged = true;
            _instance = this;
        }

        public static MainPage Instance => _instance ?? (_instance = new MainPage());

        protected override void OnDisappearing()
        {
            _loginService.LoggingOut -= LoginServiceOnLoggingOut;
            MenuPageViewModel.Instance.PropertyChanged -= MenuOnPropertyChanged;
            App.MenuPressed -= AppOnMenuPressed;
            _reged = false;
            base.OnDisappearing();
        }

        /// <summary>
        /// Event that is raised when a detail appears.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            if (!_reged)
            {
                MenuPageViewModel.Instance.PropertyChanged += MenuOnPropertyChanged;
                App.MenuPressed += AppOnMenuPressed;
                _loginService.LoggingOut += LoginServiceOnLoggingOut;
            }
            base.OnAppearing();
        }

        private void AppOnMenuPressed()
        {
            IsPresented = !IsPresented;
        }

        private void NavPageOnPopped(object sender, NavigationEventArgs args)
        {
            _titlePageSelected = null;
            CheckPage();
        }

        private void CheckPage()
        {
            //TODO
            //if (NavPage.CurrentPage is IShootPage)
            //{
            //    MenuPageViewModel.Instance.PropertyChanged -= MenuOnPropertyChanged;
            //    MenuPageViewModel.Instance.SelectedMenu = _firstMenu;
            //    MenuPageViewModel.Instance.PropertyChanged += MenuOnPropertyChanged;
            //}
        }

        private readonly List<Page> _helpPages = new List<Page>();
        private void NavPageOnPushed(object sender, NavigationEventArgs args)
        {
            CheckPage();
            NavigationPage.SetBackButtonTitle(NavPage.CurrentPage, "");
        }

        private MenuPageItem _firstMenu;
        private readonly ILoginService _loginService;
        private bool _reged;
        private static MainPage _instance;

        private async void MenuOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var selectedMenu = MenuPageViewModel.Instance.SelectedMenu;
            if (_firstMenu == null)
                _firstMenu = MenuPageViewModel.Instance.SelectedMenu;
            if (selectedMenu == null)
            {
                _titlePageSelected = null;
            }
            if (e.PropertyName == "SelectedMenu" && selectedMenu != null && selectedMenu.Title != _titlePageSelected)
            {
                _titlePageSelected = selectedMenu.Title;
                var page = Activator.CreateInstance(selectedMenu.TargetType);

                await ServiceNavigation.Instance.PushAsync((Page)page);
                IsPresented = false;
            }
        }



        private void LoginServiceOnLoggingOut(object sender, CancelEventArgs cancelEventArgs)
        {
            MenuPageViewModel.Instance.PropertyChanged -= MenuOnPropertyChanged;
            MenuPageViewModel.Instance.SelectedMenu = null;
        }
    }
}
