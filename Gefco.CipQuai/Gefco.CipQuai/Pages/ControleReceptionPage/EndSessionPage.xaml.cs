using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Xamarin.Forms;

namespace Gefco.CipQuai.ControleReceptionPage
{
    public partial class EndSessionPage : ExtContentPage
    {
        public CRViewModel ViewModel { get; }

        public EndSessionPage(CRViewModel CRViewModel)
        {
            ViewModel = CRViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            BindingContext = new EndSessionPageViewModel(CRViewModel);
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
        public CRViewModel ViewModel { get; }
        public string PageInvite { get; }

        public EndSessionPageViewModel()
        {
            
        }
        public EndSessionPageViewModel(CRViewModel CRViewModel)
        {
            ViewModel = CRViewModel;
            PageInvite = $"Le contrôle arrivage\r\na été déclaré avec\r\nsuccès !";
        }

        public string StartNewDeclarationInvite => "Déclarer autre contrôle";
        public string GoHomeInvite => "Retour à l'accueil";

    }
}
