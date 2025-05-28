using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.DoubleDeckPage;
using Gefco.CipQuai.Pages;

namespace Gefco.CipQuai.StartPage
{
    public class StartPageViewModel : ViewModelBaseExt
    {
        public string StartPageInvite
        {
            get
            {
                if (string.IsNullOrWhiteSpace(App.Settings.User.FirstName))
                    return "Bienvenue sur votre espace,\r\nque souhaitez-vous déclarer ?";
                return $"Bienvenue sur votre espace {App.Settings.User.FirstName},\r\nque souhaitez-vous déclarer ?";
            }
        }

        public RelayCommand SimpleDeckCommand { get; set; }
        public RelayCommand ControleReceptionCommand { get; set; }
        public RelayCommand DoubleDeckCommand { get; set; }
        public RelayCommand NonConformityCommand { get; set; }
        public RelayCommand BestPracticeCommand { get; set; }

        public bool HasDebug2
        {
            get => App.Settings.AddDeclarationRemorques.Any()
                   || App.Settings.AddDeclarationDoublePlanchers.Any() || App.Settings.UpdateDeclarationDoublePlanchers.Any()
                   || App.Settings.AddDeclarationNonConformites.Any()
                   || App.Settings.AddDeclarationBonnePratiques.Any()
                   || App.Settings.AddDeclarationControleReceptions.Any()
                   || App.Settings.UploadPicture.Any();
        }
        public bool HasDebug
        {
            get
            {
                SetDebug();
                return Items.Any();
            }
        }
        public ObservableCollection<DebugListItem> Items { get; set; }
        public RelayCommand<DebugListItem> ItemClickCommand { get; set; }

        public StartPageViewModel()
        {
            SimpleDeckCommand = new RelayCommand(GotoSimpleDeck, () => CanExecute);
            DoubleDeckCommand = new RelayCommand(GotoDoubleDeck, () => CanExecute);
            ControleReceptionCommand = new RelayCommand(GotoControleReception, () => CanExecute);
            NonConformityCommand = new RelayCommand(GotoNonConformity, () => CanExecute);
            BestPracticeCommand = new RelayCommand(GotoBestPractice, () => CanExecute);
            ItemClickCommand = new RelayCommand<DebugListItem>(ItemClick, (o) => CanExecute);
            LoadingCommands.Add(SimpleDeckCommand);
            LoadingCommands.Add(DoubleDeckCommand);
            LoadingCommands.Add(NonConformityCommand);
            LoadingCommands.Add(BestPracticeCommand);

            Items = new ObservableCollection<DebugListItem>();
            App.SyncHappened += AppOnSyncHappened;
            SetDebug();
        }
        private void AppOnSyncHappened()
        {
            //context.RaisePropertyChanged(nameof(context.HasDebug));
            RaisePropertyChanged(nameof(HasDebug2));
        }

        private async void ItemClick(DebugListItem obj)
        {
            await obj.SyncAsync();
        }
        public void SetDebug()
        {
            Items.Clear();
            if (App.Settings.AddDeclarationRemorques.Any())
                Items.Add(new DebugListItem(DebugType.SP, $"Déclarations Simples planchers ({App.Settings.AddDeclarationRemorques.Count})", App.Settings.AddDeclarationRemorques.Count));
            if (App.Settings.AddDeclarationDoublePlanchers.Any() || App.Settings.UpdateDeclarationDoublePlanchers.Any())
                Items.Add(new DebugListItem(DebugType.DP, $"Déclarations Doubles planchers ({App.Settings.AddDeclarationDoublePlanchers.Count + App.Settings.UpdateDeclarationDoublePlanchers.Count})", App.Settings.AddDeclarationDoublePlanchers.Count + App.Settings.UpdateDeclarationDoublePlanchers.Count));
            if (App.Settings.AddDeclarationNonConformites.Any())
                Items.Add(new DebugListItem(DebugType.NC, $"Déclarations Non conformités ({App.Settings.AddDeclarationNonConformites.Count})", App.Settings.AddDeclarationNonConformites.Count));
            if (App.Settings.AddDeclarationBonnePratiques.Any())
                Items.Add(new DebugListItem(DebugType.BP, $"Déclarations Bonnes pratiques ({App.Settings.AddDeclarationBonnePratiques.Count})", App.Settings.AddDeclarationBonnePratiques.Count));
            if (App.Settings.AddDeclarationControleReceptions.Any())
                Items.Add(new DebugListItem(DebugType.CR, $"Déclarations Contrôles arrivage ({App.Settings.AddDeclarationControleReceptions.Count})", App.Settings.AddDeclarationControleReceptions.Count));
            if (App.Settings.UploadPicture.Any())
                Items.Add(new DebugListItem(DebugType.Picture, $"Photos ({App.Settings.UploadPicture.Count})", App.Settings.UploadPicture.Count));
            RaisePropertyChanged(nameof(Items));
            RaisePropertyChanged(nameof(HasDebug2));
        }

        private async void GotoSimpleDeck()
        {
            IsLoading = true;
            var page = new SimpleDeckPage.StartPage();
            await NavigationService.PushAsync(page, true);
            IsLoading = false;
        }
        private async void GotoDoubleDeck()
        {
            IsLoading = true;
            var page = new DoubleDeckPage.DoubleDeckPage();
            await NavigationService.PushAsync(page, true);
            IsLoading = false;
        }
        private async void GotoControleReception()
        {
            IsLoading = true;
            var page = new ControleReceptionPage.StartPage();
            await NavigationService.PushAsync(page, true);
            IsLoading = false;
        }
        private async void GotoNonConformity()
        {
            IsLoading = true;
            var page = new NonConformityPage.StartPage();
            await NavigationService.PushAsync(page, true);
            IsLoading = false;
        }
        private async void GotoBestPractice()
        {
            IsLoading = true;
            var page = new BestPracticePage.StartPage();
            await NavigationService.PushAsync(page, true);
            IsLoading = false;
        }
    }
}
