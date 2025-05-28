using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;

namespace Gefco.CipQuai.SimpleDeckPage
{
    public partial class EndSessionPage : ExtContentPage
    {
        public SPViewModel ViewModel { get; }

        public EndSessionPage(SPViewModel SPViewModel)
        {
            ViewModel = SPViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new EndSessionPageViewModel(SPViewModel);
        }

        private async void StartNewDeclaration(object sender, EventArgs e)
        {
            //todo keep
            IsLoading = true;
            var page = new StartPage();
            await NavigationService.PushAsync(page);
            Navigation.RemovePage(this);
            IsLoading = false;
        }

        private new async void GoHome(object sender, EventArgs e)
        {
            await NavigationService.PopToRootAsync();
        }
    }

    public class EndSessionPageViewModel : ViewModelBaseExt
    {
        public SPViewModel ViewModel { get; }
        public string PageInvite { get; }

        public EndSessionPageViewModel()
        {
            
        }
        public EndSessionPageViewModel(SPViewModel SPViewModel)
        {
            ViewModel = SPViewModel;
            PageInvite = $"La remorque\r\na été déclarée avec\r\nsuccès !";
        }

        public string StartNewDeclarationInvite => "Déclarer autre remorque";
        public string GoHomeInvite => "Retour à l'accueil";

    }
}
