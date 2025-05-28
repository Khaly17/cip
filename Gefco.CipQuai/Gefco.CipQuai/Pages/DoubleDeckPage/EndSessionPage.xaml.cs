using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;

namespace Gefco.CipQuai.DoubleDeckPage
{
    public partial class EndSessionPage : ExtContentPage
    {
        public DPViewModel ViewModel { get; }

        public EndSessionPage(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new EndSessionPageViewModel(dpViewModel);
        }

        private async void StartNewDeclaration(object sender, EventArgs e)
        {
            IsLoading = true;
            var page = new DoubleDeckPage();
            await NavigationService.PushAsync(page);
            IsLoading = false;
            Navigation.RemovePage(this);
        }

        private new async void GoHome(object sender, EventArgs e)
        {
            await NavigationService.PopToRootAsync();
        }
    }

    public class EndSessionPageViewModel : ViewModelBaseExt
    {
        public DPViewModel ViewModel { get; }
        public string PageInvite { get; }

        public EndSessionPageViewModel()
        {
            
        }
        public EndSessionPageViewModel(DPViewModel dpViewModel)
        {
            ViewModel = dpViewModel;
            PageInvite = $"La traction {ViewModel.TractionName}\r\na été déclarée avec\r\nsuccès !";
        }

        public string StartNewDeclarationInvite => "Déclarer autre remorque";
        public string GoHomeInvite => "Retour à l'accueil";

    }
}
