using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;

namespace Gefco.CipQuai.BestPracticePage
{
    public partial class EndSessionPage : ExtContentPage
    {
        public BPViewModel ViewModel { get; }

        public EndSessionPage(BPViewModel bpViewModel)
        {
            ViewModel = bpViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new EndSessionPageViewModel(bpViewModel);
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
        public BPViewModel ViewModel { get; }
        public string PageInvite { get; }

        public EndSessionPageViewModel()
        {
            
        }
        public EndSessionPageViewModel(BPViewModel bpViewModel)
        {
            ViewModel = bpViewModel;
            PageInvite = $"La bonne pratique\r\na été déclarée avec\r\nsuccès !";
        }

        public string StartNewDeclarationInvite => "Déclarer autre BP";
        public string GoHomeInvite => "Retour à l'accueil";

    }
}
