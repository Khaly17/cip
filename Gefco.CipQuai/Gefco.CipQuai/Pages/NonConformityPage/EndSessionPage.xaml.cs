using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;

namespace Gefco.CipQuai.NonConformityPage
{
    public partial class EndSessionPage : ExtContentPage
    {
        public NCViewModel ViewModel { get; }

        public EndSessionPage(NCViewModel ncViewModel)
        {
            ViewModel = ncViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new EndSessionPageViewModel(ncViewModel);
        }

        private async void StartNewDeclaration(object sender, EventArgs e)
        {
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
        public NCViewModel ViewModel { get; }
        public string PageInvite { get; }

        public EndSessionPageViewModel()
        {
            
        }
        public EndSessionPageViewModel(NCViewModel ncViewModel)
        {
            ViewModel = ncViewModel;
            PageInvite = $"La non conformité\r\na été déclarée avec\r\nsuccès !";
        }

        public string StartNewDeclarationInvite => "Déclarer autre NC";
        public string GoHomeInvite => "Retour à l'accueil";

    }
}
