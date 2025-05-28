using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceLocator;
using FFImageLoading;
using FFImageLoading.Forms;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Extensions;
using Gefco.CipQuai.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Gefco.CipQuai.DoubleDeckPage
{
    public partial class DPViewModel : ViewModelBaseExt
    {
        private Agence _agenceDepart;
        private Agence _agenceArrivee;
        private TractionListItem _selectedTraction;
        private bool _isInvalid;

        public DPViewModel()
        {
            ValidateTractionCommand = new RelayCommand(ValidateAndGo, CanExecute);
            LoadingCommands.Add(ValidateTractionCommand);
            ValidateDestinationCommand = new RelayCommand(ValidateDestinationAndGo, () => !IsInvalidDestination);
            LoadingCommands.Add(ValidateDestinationCommand);
            ValidateNbDPCasséesCommand = new RelayCommand(() =>
            {
                if (string.IsNullOrWhiteSpace(NbDPCassées))
                    return;
                IsSuccessInput = true;
                GoBack();
            }, () => IsValidNbDPCassées);
            LoadingCommands.Add(ValidateNbDPCasséesCommand);
            ValidateAutreMotifDPCommand = new RelayCommand(() =>
            {
                if (string.IsNullOrWhiteSpace(AutreMotifDP))
                    return;
                IsSuccessInput = true;
                GoBack();
            }, () => IsValidAutreMotifDP);
            LoadingCommands.Add(ValidateAutreMotifDPCommand);
        }

        private IList<Resource> Resources => App.Settings.Resources;
        public override async Task LoadViewModel()
        {
            if (IsLoaded || IsLoading)
                return;
            IsLoading = true;
            await LoadConfigAsync();
            await LoadResourcesAsync();
            await LoadMotifDPsAsync();
            await LoadDataAsync();

            IsLoading = false;
            IsLoaded = true;
        }

        private async Task LoadConfigAsync()
        {
            if (App.Settings.Configurations.IsNullOrEmpty())
                await App.LoadConfigurationsAsync();

            _declarationDateSortOrder = App.Settings.Configurations.SingleOrDefault(p => p.Name == "Mobile.DoubleDeckPage.DeclarationDateSortOrder");
            _tractionListItemSortOrder = App.Settings.Configurations.SingleOrDefault(p => p.Name == "Mobile.DoubleDeckPage.TractionListItemSortOrder");
            if (_declarationDateSortOrder == null)
                _declarationDateSortOrder = new Configuration() { Name = "Mobile.DoubleDeckPage.DeclarationDateSortOrder", Value = "Descending" };
            if (_tractionListItemSortOrder == null)
                _tractionListItemSortOrder = new Configuration() { Name = "Mobile.DoubleDeckPage.TractionListItemSortOrder", Value = "DeclarationTraction" };
        }
        private async Task LoadMotifDPsAsync()
        {
            if (App.Settings.MotifDPs.IsNullOrEmpty())
                await App.LoadMotifDPsAsync();
            if (App.Settings.MotifDPs.IsNullOrEmpty())
                return;

            MotifsDP = App.Settings.MotifDPs.OrderBy(p => p.DisplayOrder).Select(p => new MotifDPViewModel(p)).ToList();
        }
        private async Task LoadResourcesAsync()
        {
            if (Resources.IsNullOrEmpty())
                await App.LoadResourcesAsync();
            if (Resources.IsNullOrEmpty())
                return;

            PageTitle = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(PageTitle)}")?.Value ?? PageTitle;
            TractionInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(TractionInvite)}")?.Value ?? TractionInvite;
            AgenceDepartLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(AgenceDepartLabel)}")?.Value ?? AgenceDepartLabel;
            AgenceArriveeLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(AgenceArriveeLabel)}")?.Value ?? AgenceArriveeLabel;
            RemorqueNumberLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(RemorqueNumberLabel)}")?.Value ?? RemorqueNumberLabel;
            ValidationSummary = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(ValidationSummary)}")?.Value ?? ValidationSummary;
            NoResultsMessage = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(NoResultsMessage)}")?.Value ?? NoResultsMessage;
            TakeLoadPictureButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(TakeLoadPictureButtonLabel)}")?.Value ?? TakeLoadPictureButtonLabel;
            CantTakePictureButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(CantTakePictureButtonLabel)}")?.Value ?? CantTakePictureButtonLabel;
            NextButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(NextButtonLabel)}")?.Value ?? NextButtonLabel;
            TakePictureButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(TakePictureButtonLabel)}")?.Value ?? TakePictureButtonLabel;
            CancelInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(CancelInvite)}")?.Value ?? CancelInvite;
            ChoosePictureButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(ChoosePictureButtonLabel)}")?.Value ?? ChoosePictureButtonLabel;
            DeletePictureButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(DeletePictureButtonLabel)}")?.Value ?? DeletePictureButtonLabel;
            SaisieNbBarresDPCasseesInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(SaisieNbBarresDPCasseesInvite)}")?.Value ?? SaisieNbBarresDPCasseesInvite;
            SaisieAutreMotifDPInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(SaisieAutreMotifDPInvite)}")?.Value ?? SaisieAutreMotifDPInvite;
            CancelDeclarationInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(CancelDeclarationInvite)}")?.Value ?? CancelDeclarationInvite;
            SaisieNbBarresDPCasseesInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(SaisieNbBarresDPCasseesInvite)}")?.Value ?? SaisieNbBarresDPCasseesInvite;
            SaisieAutreMotifDPInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(SaisieAutreMotifDPInvite)}")?.Value ?? SaisieAutreMotifDPInvite;
            SuspendButtonInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(SuspendButtonInvite)}")?.Value ?? SuspendButtonInvite;
            ResumeButtonInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(ResumeButtonInvite)}")?.Value ?? ResumeButtonInvite;
            ColleagueButtonInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(ColleagueButtonInvite)}")?.Value ?? ColleagueButtonInvite;
            RestartButtonInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(RestartButtonInvite)}")?.Value ?? RestartButtonInvite;
            DeclarationImpossibleInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(DeclarationImpossibleInvite)}")?.Value ?? DeclarationImpossibleInvite;
            ImpossiblePageInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(ImpossiblePageInvite)}")?.Value ?? ImpossiblePageInvite;
            MotifsLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(MotifsLabel)}")?.Value ?? MotifsLabel;
            RetakePictureInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(RetakePictureInvite)}")?.Value ?? RetakePictureInvite;
            TakeNextPictureInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(TakeNextPictureInvite)}")?.Value ?? TakeNextPictureInvite;
            EndImpossibleDeclarationInvite = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(EndImpossibleDeclarationInvite)}")?.Value ?? EndImpossibleDeclarationInvite;
            TakeFirstPictureButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(TakeFirstPictureButtonLabel)}")?.Value ?? TakeFirstPictureButtonLabel;
            TakeSecondPictureButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(TakeSecondPictureButtonLabel)}")?.Value ?? TakeSecondPictureButtonLabel;
            StartPageInviteSingle = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(StartPageInviteSingle)}")?.Value ?? StartPageInviteSingle;
            StartPageInviteMultiple = Resources.SingleOrDefault(p => p.Key == $"{nameof(DoubleDeckPage)}.{nameof(StartPageInviteMultiple)}")?.Value ?? StartPageInviteMultiple;
        }

        private async Task LoadDataAsync()
        {
            Tractions.Clear();
            await AddDeclarationsAsync();
            await AddTractionsAsync();

            var sorted = Tractions.OrderBy(p => p.AgenceDepart?.Name).ThenBy(p => p.AgenceArrivee?.Name).ThenByDescending(p => p.DueDate).ToList();
            for (int i = 0; i < sorted.Count(); i++)
                Tractions.Move(Tractions.IndexOf(sorted[i]), i);

            OnPropertyChanged(nameof(StartPageInvite));
            var items = await App.LoadProvenancesAsync();
            if (items != null && items.Any())
                FillDestinations(items);
            else if (App.Settings.Agences != null && App.Settings.Agences.Any())
                FillDestinations(App.Settings.Agences);
        }

        private void FillDestinations(IList<Agence> destinations)
        {
            foreach (var destination in destinations.Where(p => p.IsGefcoFrance).OrderBy(p => p.Name))
            {
                Destinations.Add(destination);
            }
        }

        private async Task AddTractionsAsync()
        {
            var tractions = await App.LoadTractionsAsync();
            if (tractions != null && tractions.Any())
                FillTractions(tractions);
            else if (App.Settings.Tractions != null && App.Settings.Tractions.Any())
                FillTractions(App.Settings.Tractions);
        }

        private void FillTractions(IList<Traction> tractions)
        {
            _tractions.Clear();
            var status = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "ToBeValidated");
            if (status != null)
                foreach (var traction in tractions.Where(p => App.Settings.AddDeclarationDoublePlanchers.Where(q => q.TractionId == p.Id).All(q => q.CurrentStatus.Id != status.Id) && App.Settings.UpdateDeclarationDoublePlanchers.Where(q => q.TractionId == p.Id).All(q => q.CurrentStatus.Id != status.Id)).OrderBy(p => p.Name))
                {
                    _tractions.Add(traction);
                    Tractions.Add(new TractionListItem(traction));
                }
        }

        private async Task AddDeclarationsAsync()
        {
            var status = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "ToBeValidated");
            if (status != null)
            {
                var declarations = await App.LoadActiveDeclarationDoublePlanchersAsync();
                if (declarations != null && declarations.Any())
                    FillDeclarations(declarations);
                else if (App.Settings.DeclarationDoublePlanchers != null && App.Settings.DeclarationDoublePlanchers.Any())
                    FillDeclarations(App.Settings.DeclarationDoublePlanchers.ToList());
            }
        }

        private void FillDeclarations(IList<DeclarationDoublePlancher> declarations)
        {
            _declarations.Clear();

            IOrderedEnumerable<DeclarationDoublePlancher> orderedEnumerable;
            if (_declarationDateSortOrder?.Value == "Ascending")
                orderedEnumerable = declarations.OrderByDescending(p => p.CurrentStatus.CreationDate);
            else
                orderedEnumerable = declarations.OrderByDescending(p => p.CurrentStatus.CreationDate);
            var status = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "ToBeValidated");
            foreach (var declaration in orderedEnumerable)
            {
                if (App.Settings.UpdateDeclarationDoublePlanchers.Any(q => q.TractionId == declaration.TractionId && q.CurrentStatus.Id == status.Id))
                    continue;
                _declarations.Add(declaration);
                Tractions.Add(new TractionListItem(declaration));
            }
        }

        public bool IsSuccessInput { get; set; }

        #region Resources

        public string PageTitle { get; private set; } = "Chargement DP";
        public string TractionInvite { get; private set; } = "Remorque";
        public string AgenceDepartLabel { get; private set; } = "Agence de départ";
        public string AgenceArriveeLabel { get; private set; } = "Agence d'arrivée";
        public string RemorqueNumberLabel { get; private set; } = "Remorque N°";
        public string ValidationSummary { get; private set; } = "Aucune remorque sélectionnée";
        public string NoResultsMessage { get; private set; } = "Aucune remorque trouvée...";
        public string TakeLoadPictureButtonLabel { get; private set; } = "Prendre photo du chargement";
        public string CantTakePictureButtonLabel { get; private set; } = "Chargement en DP impossible";
        public string ImpossiblePageInvite { get; private set; } = "Chargement en DP impossible";
        public string MotifsLabel { get; private set; } = "Motif(s)";
        public string NextButtonLabel { get; private set; } = "Suivant >";
        public string TakePictureButtonLabel { get; private set; } = "Prendre photo";
        public string CancelInvite { get; private set; } = "Annuler";
        public string ChoosePictureButtonLabel { get; private set; } = "Choisir depuis la galerie";
        public string DeletePictureButtonLabel { get; private set; } = "Supprimer";
        public string SaisieNbBarresDPCasseesInvite { get; private set; } = "Veuillez indiquer le nombre\r\nde barres DP cassées";
        public string SaisieAutreMotifDPInvite { get; private set; } = "Chargement en DP Impossible,\r\nVeuillez indiquer la raison.";
        public string SaisieAutreMotifDPPlaceholder { get; private set; } = "Autre raison";
        public string DeclarationImpossibleInvite { get; private set; } = "Chargement en DP Impossible,\r\nVeuillez indiquer les raisons.";
        public string CancelDeclarationInvite { get; private set; } = "Vous êtes sur le point de suspendre votre déclaration,\r\nvous pouvez la reprendre à tout moment";
        public string SuspendButtonInvite { get; private set; } = "Suspendre";
        public string ResumeButtonInvite { get; private set; } = "Reprendre";
        public string ColleagueButtonInvite { get; private set; } = "Un collègue peut terminer ma déclaration";
        public string RestartLabelInvite { get; private set; } = "Vous êtes sur le point de rependre une déclaration.\r\nVous pouvez aussi la refaire depuis le début.";
        public string RestartButtonInvite { get; private set; } = "Refaire depuis le début.";
        public string RetakePictureInvite { get; private set; } = "Refaire photo";
        public string TakeNextPictureInvite { get; private set; } = "Conserver photo et\r\nprendre nouvelle photo";
        public string EndImpossibleDeclarationInvite { get; private set; } = "Conserver photo et\r\nTerminer déclaration";
        public string TakeFirstPictureButtonLabel { get; private set; } = "Photo à moitié du chargement";
        public string TakeSecondPictureButtonLabel { get; private set; } = "Photo à fin de chargement";
        public string StartPageInviteSingle { get; private set; } = "1 remorque DP est à charger\r\nsur votre site";
        public string StartPageInviteMultiple { get; private set; } = " remorques DP sont à charger\r\nsur votre site";
        public string DestinationInviteLabel { get; private set; } = "Veuillez vérifier l'agence de destination";

        #endregion

        public string StartPageInvite
        {
            get
            {
                if (Tractions != null)
                {
                    if (Tractions.Count == 1)
                        return StartPageInviteSingle;
                    else
                        return Tractions.Count + StartPageInviteMultiple;
                }
                else
                    return 0 + StartPageInviteMultiple;
            }
        }
        public Agence AgenceDepart
        {
            get => _agenceDepart;
            set
            {
                if (value == _agenceDepart)
                    return;
                _agenceDepart = value;
                OnPropertyChanged();
            }
        }
        public Agence AgenceArrivee
        {
            get => _agenceArrivee;
            set
            {
                if (value == _agenceArrivee)
                    return;
                _agenceArrivee = value;
                OnPropertyChanged();
            }
        }

        private const string MockRemorqueNumber = "TODO XXXX-DISTRI";
        private string _remorqueNumber = MockRemorqueNumber;

        public string RemorqueNumber
        {
            get { return _remorqueNumber; }
            set
            {
                if (value == _remorqueNumber)
                    return;
                _remorqueNumber = value;
                OnPropertyChanged();
            }
        }

        #region Sélection Traction

        private List<Traction> _tractions { get; } = new List<Traction>();
        public ObservableCollection<TractionListItem> Tractions { get; set; } = new ObservableCollection<TractionListItem>();
        private List<DeclarationDoublePlancher> _declarations { get; } = new List<DeclarationDoublePlancher>();
        static TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Paris");
        
        public TractionListItem SelectedTraction
        {
            get => _selectedTraction;
            set
            {
                if (value == _selectedTraction)
                    return;
                _selectedTraction = value;
                if (_selectedTraction != null)
                {
                    AgenceDepart = _selectedTraction.AgenceDepart;
                    AgenceArrivee = _selectedTraction.AgenceArrivee;
                }
                ValidateTraction();
                OnPropertyChanged();
                ValidateTractionCommand.RaiseCanExecuteChanged();
            }
        }

        private string _tractionName;

        public string TractionName
        {
            get { return _tractionName; }
            set
            {
                if (_tractionName == value)
                    return;
                _tractionName = value;
                if (SelectedTraction?.Name != _tractionName)
                {
                    var items = Tractions.Where(p => p.Name.ToLower() == _tractionName.ToLower()).ToList();
                    if (items.Any())
                    {
                        if (items.Count == 1)
                        {
                            var item = items.Single();
                            SelectedTraction = item;
                            _tractionName = item.Name;
                            goto Notify;
                        }
                    }
                    _selectedTraction = null;
                    var strings = _tractionName?.Split('-');
                    if (strings?.Length >= 2)
                    {
                        AgenceDepart = new Agence(strings[0].Trim(), Guid.Empty.ToString(), DateTime.Now);
                        var remain = strings.Skip(1).ToList();
                        AgenceArrivee = new Agence(string.Join("-", remain), Guid.Empty.ToString(), DateTime.Now);
                    }
                    else
                    {
                        AgenceDepart = null;
                        AgenceArrivee = null;
                    }
                }
                Notify:
                ValidateTraction();
                OnPropertyChanged();
                ValidateTractionCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ValidateTractionCommand { get; set; }
        public RelayCommand ValidateDestinationCommand { get; set; }
        public override bool CanExecute => SelectedTraction != null;

        public bool IsInvalid
        {
            get => _isInvalid;
            set
            {
                if (value == _isInvalid)
                    return;
                _isInvalid = value;
                OnPropertyChanged();
            }
        }


        private bool _isValidTraction;
        public bool IsValidTraction
        {
            get { return _isValidTraction; }
            set
            {
                if (value == _isValidTraction)
                    return;
                _isValidTraction = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Agence> Destinations { get; } = new ObservableCollection<Agence>();
        private Agence _selectedDestination;
        public Agence SelectedDestination
        {
            get => _selectedDestination;
            set
            {
                if (value == _selectedDestination)
                    return;
                _selectedDestination = value;
                ValidateDestination();
                OnPropertyChanged();
                ValidateDestinationCommand.RaiseCanExecuteChanged();
            }
        }

        public async void ValidateDestination()
        {
            ErrorMessage = "";

            if (_selectedDestination == null)
            {
                IsInvalidDestination = true;
                ErrorMessage = "Vous devez sélectionner un élément dans la liste";
            }
            else
            {
                ErrorMessage = "";
                IsInvalidDestination = false;
                AgenceArrivee = _selectedDestination;
            }
        }
        public async void ValidateDestinationAndGo()
        {
            ValidateDestination();
            if (_selectedDestination != null)
                await Gotodeclaration();
        }

        private bool _isInvalidDestination;

        public bool IsInvalidDestination
        {
            get { return _isInvalidDestination; }
            set
            {
                if (value == _isInvalidDestination)
                    return;
                _isInvalidDestination = value;
                RaisePropertyChanged();
                ValidateDestinationCommand.RaiseCanExecuteChanged();
            }
        }

        private async void ValidateAndGo()
        {
            ValidateTraction();
            if (AgenceArrivee == null || AgenceArrivee.Id == Guid.Empty.ToString())
            {
                IsLoading = true;
                var page = new ChoixDestinationPage(this);
                await NavigationService.PushAsync(page);
                IsLoading = false;
                return;
            }
            if (!IsInvalid && IsValidTraction)
            {
                await Gotodeclaration();
            }
        }

        private async Task Gotodeclaration()
        {
            IsLoading = true;
            var oldDeclaration = Declaration;
            var isComingBack = IsComingBack;
            await InitWorkflowAsync();
            IsComingBack = false;
            if (isComingBack && oldDeclaration.Id == Declaration.Id)
            {
                var page = new DeclarationPage(this);
                await NavigationService.PushAsync(page);
                IsLoading = false;
                return;
            }
            if (IsResuming)
                DeclarationResuming?.Invoke();
            else
            {
                ResumeDeclaration();
            }
            IsLoading = false;
        }

        #endregion

        public FlowState State { get; set; }

        private async Task UpdateDeclaration()
        {
            Declaration.AutreMotifDP = AutreMotifDP;
            Declaration.MotifDps = MotifsDP?.Where(p => p.IsChecked).Select(p => p.MotifDP).ToList();
            Declaration.IsDPUsed = !Declaration.MotifDps?.Any() ?? false;
            if (NbDPCassées != null)
                Declaration.NbDPCassées = int.Parse(NbDPCassées);
            else
                Declaration.NbDPCassées = 0;
            var s = await SavePicture(Picture1, PictureType.HalfLoadPicture);
            if (s != null)
                Picture1 = s;
            s = await SavePicture(Picture2, PictureType.FullLoadPicture);
            if (s != null)
                Picture2 = s;
            s = await SavePicture(ErrorPicture1, PictureType.ErrorPicture1);
            if (s != null)
                ErrorPicture1 = s;
            s = await SavePicture(ErrorPicture2, PictureType.ErrorPicture2);
            if (s != null)
                ErrorPicture2 = s;
            s = await SavePicture(ErrorPicture3, PictureType.ErrorPicture3);
            if (s != null)
                ErrorPicture3 = s;
            //Declaration.Pictures
        }

        private async Task<string> SavePicture(string filePath, PictureType pictureType)
        {
            if (!filePath.IsNullOrWhiteSpace() && filePath != "Camera.svg")
            {
                if (!filePath.StartsWith("http"))
                {
                    var parameter = new Settings.UploadPictureParameter()
                                    {
                                        Picture = new Picture() {PicturePath = filePath, PictureType = (int?) pictureType, DeclarationId = Declaration.Id},
                                        PictureType = Settings.UploadPictureType.DP
                                    };
                    App.Settings.UploadPicture.Add(parameter);
                    App.Settings.UploadPicture = App.Settings.UploadPicture;
                    var bytes = File.ReadAllBytes(filePath);
                    var fileContent = Convert.ToBase64String(bytes);
                    var result = await DataService.UploadPicDP(fileContent, Path.GetFileName(filePath), pictureType, Declaration.Id);
                    if (result?.IsSuccess ?? false)
                    {
                        try
                        {
                            await ImageService.Instance.LoadUrl(result.Value.PicturePath).PreloadAsync();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        App.Settings.UploadPicture.Remove(parameter);
                        App.Settings.UploadPicture = App.Settings.UploadPicture;
                        return result.Value.PicturePath;
                    }
                }
            }
            return null;
        }

        public async Task SaveDeclarationAsync(FlowState state)
        {
            IsDirty = true;
            await UpdateDeclaration();
            Declaration.CurrentWorkflowStep = (int) state;
            await DataService.UpdateDeclarationDoublePlancherAsync(Declaration, Declaration.CurrentStatus?.Id);
            State = state;
        }
        public async Task EndDeclarationAsync(bool isDpUsed)
        {
            await UpdateDeclaration();
            var status = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "ToBeValidated");
            Declaration.CurrentStatus = status;
            Declaration.IsDPUsed = isDpUsed;
            await DataService.UpdateDeclarationDoublePlancherAsync(Declaration, Declaration.CurrentStatus.Id);
        }
        public async void SuspendDeclaration(FlowState state)
        {
            await UpdateDeclaration();
            var status = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "PausedAndLocked");
            Declaration.CurrentStatus = status;
            Declaration.CurrentWorkflowStep = Math.Max((int)state, Declaration.CurrentWorkflowStep);
            await DataService.UpdateDeclarationDoublePlancherAsync(Declaration, status.Id);
            State = state;
        }

        public async void StopAndFreeDeclaration(FlowState state)
        {
            await UpdateDeclaration();
            var status = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "PausedAndFree");
            Declaration.CurrentStatus = status;
            Declaration.CurrentWorkflowStep = Math.Max((int)state, Declaration.CurrentWorkflowStep);
            await DataService.UpdateDeclarationDoublePlancherAsync(Declaration, status.Id);
            State = state;
        }

        private async Task InitWorkflowAsync()
        {
            Reset();
            Traction traction = null;
            if (SelectedTraction != null)
            {
                var tractionListItem = SelectedTraction;
                if (tractionListItem.IsDeclaration)
                {
                    Declaration = _declarations.Single(p => p.Id == SelectedTraction.DeclarationId);
                    traction = Declaration.Traction;
                }
                else
                {
                    traction = _tractions.Single(p => p.Id == SelectedTraction.Id);
                    //on cherche les nouvelles déclarations
                    Declaration = _declarations.SingleOrDefault(p => p.Traction.Id == SelectedTraction.Id);
                }
            }
            else //todo New traction
            {
                AgenceDepart = App.Settings.User.MobileUserAgence;
                traction = new Traction()
                {
                    Name = TractionName,
                    Id = Guid.Empty.ToString(),
                    AgenceArrivee = AgenceArrivee,
                    AgenceDepart = AgenceDepart
                };
            }
            if (Declaration == null)
            {
                IsResuming = false;
                await CreateDeclarationAsync(traction);
            }
            else
            {
                IsResuming = true;
                await LoadDeclarationAsync();
            }
        }

        public bool IsResuming { get; set; }

        private async Task CreateDeclarationAsync(Traction traction)
        {
            Declaration = new DeclarationDoublePlancher()
            {
                AutreAgenceArrivee = AgenceArrivee.Id == Guid.Empty.ToString() ? AgenceArrivee.Name : null,
                CreationDate = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                CurrentWorkflowStep = (int) FlowState.Declaration,
                Traction = traction,
                TractionId = traction.Id//, pas besoin de traction dans le traitement
                //todo Remorque = RemorqueNumber.IsNullOrEmpty() || RemorqueNumber == MockRemorqueNumber ? null : new Remorque(Guid.NewGuid().ToString(), DateTime.Now, RemorqueNumber, true),
            };
            var res = await DataService.AddDeclarationDoublePlancher(Declaration, traction.Id);
            if (res.IsSuccess ?? false)
            {
                if (!res.Value.IsNullOrWhiteSpace())
                {
                    Declaration.Traction.Id = res.Value;
                    Declaration.TractionId = res.Value;
                    _declarations.Add(Declaration);
                    Declaration.CurrentStatus = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "InProgress");
                    State = FlowState.Declaration;
                }
                else
                {
                    if (res.ErrorMessage.Contains("DeclarationInProgressException"))
                    {
                        await ServiceDialog.Instance.ShowMessage("La déclaration a déjà été créée", "Attention");
                    }
                    else if (res.ErrorMessage.Contains("DeclarationExistsException"))
                    {
                        await ServiceDialog.Instance.ShowMessage("La déclaration est en cours d'utilisation", "Attention");
                    }
                    await LoadDataAsync();
                }
            }
        }

        private async Task LoadDeclarationAsync()
        {
            State = (FlowState) Declaration.CurrentWorkflowStep;
            AutreMotifDP = Declaration.AutreMotifDP;
            if (Declaration.MotifDps?.Any() ?? false)
                foreach (var motifDp in Declaration.MotifDps)
                {
                    var model = MotifsDP.SingleOrDefault(p => p.MotifDP.Id == motifDp.Id);
                    if (model != null)
                        model.IsChecked = true;
                }
            NbDPCassées = Declaration.NbDPCassées == 0 ? null : Declaration.NbDPCassées+"";
            if (Declaration.Pictures?.Any() ?? false)
            {
                var picture1 = Declaration.Pictures.OrderByDescending(p => p.CreationDate).FirstOrDefault(p => p.PictureType == (int?)PictureType.HalfLoadPicture);
                if (picture1 != null)
                    Picture1 = picture1.PicturePath;
                var picture2 = Declaration.Pictures.OrderByDescending(p => p.CreationDate).FirstOrDefault(p => p.PictureType == (int?)PictureType.FullLoadPicture);
                if (picture2 != null)
                    Picture2 = picture2.PicturePath;
                var errorPicture1 = Declaration.Pictures.OrderByDescending(p => p.CreationDate).FirstOrDefault(p => p.PictureType == (int?)PictureType.ErrorPicture1);
                if (errorPicture1 != null)
                    ErrorPicture1 = errorPicture1.PicturePath;
                var errorPicture2 = Declaration.Pictures.OrderByDescending(p => p.CreationDate).FirstOrDefault(p => p.PictureType == (int?)PictureType.ErrorPicture2);
                if (errorPicture2 != null)
                    ErrorPicture2 = errorPicture2.PicturePath;
                var errorPicture3 = Declaration.Pictures.OrderByDescending(p => p.CreationDate).FirstOrDefault(p => p.PictureType == (int?)PictureType.ErrorPicture3);
                if (errorPicture3 != null)
                    ErrorPicture3 = errorPicture3.PicturePath;
            }
            AgenceDepart = Declaration.Traction?.AgenceDepart;
            AgenceArrivee = Declaration.Traction?.AgenceArrivee;
            var status = App.Settings.RemorqueStatuses.SingleOrDefault(p => p.Name == "InProgress");
            Declaration.CurrentStatus = status;
            IsDirty = true;
            await DataService.UpdateDeclarationDoublePlancherAsync(Declaration, status.Id);
        }

        private void Reset(bool withDeclaration = true)
        {
            IsDpUsed = null;
            State = default(FlowState);
            AutreMotifDP = null;
            MotifsDP.ForEach(p => p.IsChecked = false);
            NbDPCassées = null;
            Picture1 = "Camera.svg";
            Picture2 = "Camera.svg";
            ErrorPicture1 = "Camera.svg";
            ErrorPicture2 = "Camera.svg";
            ErrorPicture3 = "Camera.svg";
            IsDirty = false;
            if (withDeclaration)
                Declaration = null;
        }

        public DeclarationDoublePlancher Declaration { get; set; }

        public void ValidateTraction()
        {
            Reset();
            ErrorMessage = "";

            if (_selectedTraction == null)
            {
                if (string.IsNullOrEmpty(TractionName))
                {
                    IsInvalid = false;
                    IsValidTraction = false;
                }
                else
                {
                    //(!AgenceDepart?.Id.IsEmptyId() ?? false) && (!AgenceArrivee?.Name.IsEmptyId() ?? false)
                    var items = Tractions.Where(p => p.Name.ToLower() == _tractionName.ToLower()).ToList();
                    if (items.Any())
                    {
                        IsInvalid = true;
                        ErrorMessage = "Vous devez sélectionner un élément dans la liste";
                    }
                    else
                    {
                        IsInvalid = false;
                    }

                    IsValidTraction = !IsInvalid && (!AgenceDepart?.Name.IsNullOrWhiteSpace() ?? false) && (!AgenceArrivee?.Name.IsNullOrWhiteSpace() ?? false);
                }
            }
            else
            {
                ErrorMessage = "";
                IsInvalid = /*_selectedTraction == null && string.IsNullOrEmpty(TractionName) ||*/ (AgenceDepart == null) && AgenceArrivee == null;
                IsValidTraction = !IsInvalid;
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (value == _errorMessage)
                    return;
                _errorMessage = value;
                OnPropertyChanged();
            }
        }


        #region TakePictures

        private string _picture1 = "Camera.svg";

        public string Picture1
        {
            get { return _picture1; }
            set
            {
                if (value == _picture1)
                    return;
                _picture1 = value;
                OnPropertyChanged();
            }
        }
        private string _picture2 = "Camera.svg";

        public string Picture2
        {
            get { return _picture2; }
            set
            {
                if (value == _picture2)
                    return;
                _picture2 = value;
                OnPropertyChanged();
            }
        }

        private async void GoBack()
        {
            IsLoading = true;
            await NavigationService.PopAsync();
            IsLoading = false;
        }

        #endregion

        #region DeclarationImpossible


        #region SaisieNbBarresDPCassees

        public bool IsValidNbDPCassées
        {
            get
            {
                if (NbDPCassées == null)
                    return true;
                try
                {
                    return int.Parse(NbDPCassées) > 0;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        private string _nbDPCassées;
        public string NbDPCassées
        {
            get { return _nbDPCassées; }
            set
            {
                if (value == _nbDPCassées)
                    return;
                _nbDPCassées = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValidNbDPCassées));
                ValidateNbDPCasséesCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ValidateNbDPCasséesCommand { get; set; }


        #endregion

        #region SaisieAutreMotifDP

        private string _autreMotifDP;

        public string AutreMotifDP
        {
            get { return _autreMotifDP; }
            set
            {
                if (value == _autreMotifDP)
                    return;
                _autreMotifDP = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValidAutreMotifDP));
                ValidateAutreMotifDPCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsValidAutreMotifDP
        {
            get
            {
                if (AutreMotifDP == null)
                    return true;
                return !string.IsNullOrWhiteSpace(AutreMotifDP);
            }
        }

        public RelayCommand ValidateAutreMotifDPCommand { get; set; }

        #endregion


        public List<MotifDPViewModel> MotifsDP { get; private set; } = new List<MotifDPViewModel>()
        {
            new MotifDPViewModel(new MotifDP(){Name="Fret trop volumineux", DisplayOrder = 0}),
            new MotifDPViewModel(new MotifDP(){Name="Fret insuffisant", DisplayOrder = 1}),
            new MotifDPViewModel(new MotifDP(){Name="Barre DP cassée", DisplayOrder = 2}),
            new MotifDPViewModel(new MotifDP(){Name="Autre raison", DisplayOrder = 3}),
        };

        private string _errorPicture1 = "Camera.svg";

        public string ErrorPicture1
        {
            get { return _errorPicture1; }
            set
            {
                if (value == _errorPicture1)
                    return;
                _errorPicture1 = value;
                OnPropertyChanged();
            }
        }
        private string _errorPicture2 = "Camera.svg";

        public string ErrorPicture2
        {
            get { return _errorPicture2; }
            set
            {
                if (value == _errorPicture2)
                    return;
                _errorPicture2 = value;
                OnPropertyChanged();
            }
        }
        private string _errorPicture3 = "Camera.svg";
        private string _endSessionInvite;
        private Configuration _declarationDateSortOrder;
        private Configuration _tractionListItemSortOrder;

        public string ErrorPicture3
        {
            get { return _errorPicture3; }
            set
            {
                if (value == _errorPicture3)
                    return;
                _errorPicture3 = value;
                OnPropertyChanged();
            }
        }

        public string EndSessionInvite
        {
            get { return _endSessionInvite; }
            set
            {
                if (value == _endSessionInvite)
                    return;
                _endSessionInvite = value;
                OnPropertyChanged();
            }
        }

        public bool? IsDpUsed { get; set; }

        public bool IsComingBack { get; set; }
        public bool NeedsPicture
        {
            get { return MotifsDP.Any(p => p.IsChecked && (p.MotifDP.NeedPicture ?? false)); }
        }

        public bool IsDirty { get; set; }


        public event Action DeclarationResuming;

        #endregion

        public async void ResumeDeclaration()
        {
            switch (State)
            {
                case FlowState.Declaration:
                    {
                        var page = new DeclarationPage(this);
                        await NavigationService.PushAsync(page);
                        return;
                    }
                case FlowState.TakePictures:
                    {
                        var page = new TakePicturesPage(this);
                        await NavigationService.PushAsync(page);
                        NavigationService.InsertPageBefore(new DeclarationPage(this), page);
                        return;
                    }
                case FlowState.DPUsedSummary:
                    {
                        var page = new DPUsedSummaryPage(this);
                        await NavigationService.PushAsync(page);
                        NavigationService.InsertPageBefore(new DeclarationPage(this), page);
                        NavigationService.InsertPageBefore(new TakePicturesPage(this), page);
                        return;
                    }
                case FlowState.DeclarationImpossible:
                    {
                        var page = new DeclarationImpossiblePage(this);
                        await NavigationService.PushAsync(page);
                        NavigationService.InsertPageBefore(new DeclarationPage(this), page);
                        return;
                    }
                case FlowState.TakeErrorPictures:
                    {
                        var page = new TakeErrorPicturesPage(this);
                        await NavigationService.PushAsync(page);
                        NavigationService.InsertPageBefore(new DeclarationPage(this), page);
                        NavigationService.InsertPageBefore(new DeclarationImpossiblePage(this), page);
                        return;
                    }
                case FlowState.DPNotUsedSummary:
                    {
                        var page = new DPNotUsedSummaryPage(this);
                        await NavigationService.PushAsync(page);
                        NavigationService.InsertPageBefore(new DeclarationPage(this), page);
                        NavigationService.InsertPageBefore(new DeclarationImpossiblePage(this), page);
                        NavigationService.InsertPageBefore(new TakeErrorPicturesPage(this), page);
                        return;
                    }
            }
        }

        public async void RestartDeclaration()
        {
            IsLoading = true;
            Reset(false);
            var page = new DeclarationPage(this);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }
    }

    public class MotifDPViewModel : ObservableObject
    {
        public static event Action<MotifDPViewModel> Checked;
        public MotifDP MotifDP { get; }
        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked)
                    return;
                _isChecked = value;
                Checked?.Invoke(this);
                OnPropertyChanged();
            }
        }

        public MotifDPViewModel(MotifDP motifDP)
        {
            MotifDP = motifDP;
        }
    }
}