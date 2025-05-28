using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Services;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace Gefco.CipQuai.Menu
{
    /// <summary>
    ///     Menu view model.
    /// </summary>
    public sealed class MenuPageViewModel : ViewModelBaseExt
    {
        public static MenuPageViewModel Instance => _instance;
        public Settings ServiceAppSetting => App.Settings;

        private void OpenPageAction(MenuPageItem menuItem)
        {
        }

        private MenuPageItem _selectedMenu;

        private static MenuPageViewModel _instance;


        [Preserve]
        public MenuPageViewModel()
        {
            _instance = this;
            CmdOpenPage = new RelayCommand<MenuPageItem>(OpenPageAction);
            LogoutCommand = new RelayCommand(Logout);
        }

        private void Logout()
        {
            ServiceLocator.Current.GetInstance<ILoginService>().Logout();
        }

        #region Property

        /// <summary>
        ///     Gets or sets the menu items.
        /// </summary>
        /// <value>The menu items.</value>
        public List<MenuPageItem> MenuItems { get; } = new List<MenuPageItem>() {
        new MenuPageItem
            {
                IconSource = "PictoDP",
                Title = "Chargement en DP",
                TargetType = typeof(DoubleDeckPage.DoubleDeckPage),
                IconHeight = 17,
                IconWidth = 17
            },
        new MenuPageItem
            {
                IconSource = "PictoNC",
                Title = "Non conformité",
               TargetType = typeof(NonConformityPage.StartPage),
                IconHeight = 14,
                IconWidth = 14
            },
        new MenuPageItem
            {
                IconSource = "PictoBP",
                Title = "Bonne pratique",
                TargetType = typeof(BestPracticePage.StartPage),
                IconHeight = 17,
                IconWidth = 17
            },
        new MenuPageItem
        {
            IconSource = "PictoDP",
            Title = "Simple plancher",
            TargetType = typeof(SimpleDeckPage.StartPage),
            IconHeight = 17,
            IconWidth = 17
        },
        new MenuPageItem
            {
                IconSource = "PictoProfil",
                Title = "Profil utilisateur",
                TargetType = typeof(ProfilePage.StartPage),
                IconHeight = 16,
                IconWidth = 16
            },
        };

        /// <summary>
        ///     Gets or sets the selected menu. When changed, it navigates to another page.
        /// </summary>
        /// <value>The selected menu.</value>
        public MenuPageItem SelectedMenu
        {
            get => _selectedMenu;

            set
            {
                if (_selectedMenu == value) return;
                if (_selectedMenu != null)
                    _selectedMenu.IsSelected = false;
                _selectedMenu = value;
                if (_selectedMenu != null)
                {
                    _selectedMenu.IsSelected = true;

                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the command open sections.
        /// </summary>
        /// <value>The command open sections.</value>
        public ICommand CmdOpenPage { get; }

        public string DisplayName
        {
            get
            {
                if (App.Settings.User == null)
                    return "Mickaël Martin";
                if (string.IsNullOrWhiteSpace(App.Settings.User.FirstName + " " + App.Settings.User.LastName))
                    return App.Settings.User.Email;
                return App.Settings.User.FirstName + " " + App.Settings.User.LastName;
            }
        }

        public RelayCommand LogoutCommand { get; }

        public string Version
        {
            get
            {
                try
                {
                    return "v" + VersionTracking.CurrentVersion;
                }
                catch (Exception)
                {
                    return "v1.0.0";
                }
            }
        }

        #endregion

    }
}
