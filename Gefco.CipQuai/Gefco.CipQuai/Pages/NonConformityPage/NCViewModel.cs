using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Extensions;
using Gefco.CipQuai.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gefco.CipQuai.NonConformityPage
{
    public enum FlowState
    {
        Start = 0,
        ChoixProvenance = 1,
        ChoixMotif = 2,
        TakePictures = 3,
        Summary = 4
    }

    public class NCViewModel : ViewModelBaseExt
    {
        public string PageTitle
        {
            get => $"Non-conformité";
            set { }
        }

        public string StartPageInvite => $"Veuillez choisir la provenance";
        public string GefcoButtonLabel { get; private set; } = "GEFCO France";
        public string InternationalButtonLabel { get; private set; } = "International";
        public string ConfrèresButtonLabel { get; private set; } = "Confrères";
        public string NextButtonLabel { get; private set; } = "Suivant >";
        public string ProvenanceInviteLabel { get; } = "Provenance";
        public string SummaryAgenceDepartLabel { get; } = "Agence de départ";
        public string SummaryAgenceArriveeLabel { get; } = "Agence d'arrivée";
        public string NoResultsMessage { get; private set; } = "Aucune provenance trouvée...";
        public string ChoixMotifInvite { get; private set; } = "Veuillez choisir ci-dessous les motifs";
        public string CancelDeclarationInvite { get; private set; } = "Vous êtes sur le point d'abandonner votre déclaration,\r\nsouhaitez vous continuer ?";
        public string ContinueButtonInvite { get; private set; } = "Continuer";
        public string CancelInvite { get; private set; } = "Annuler";
        public string MotifsLabel { get; private set; } = "Motif(s)";
        public string RetakePictureInvite { get; private set; } = "Refaire photo";
        public string TakeNextPictureInvite { get; private set; } = "Conserver photo et\r\nprendre nouvelle photo";
        public string EndImpossibleDeclarationInvite { get; private set; } = "Conserver photo et\r\nTerminer déclaration";
        public string TakePictureButtonLabel { get; private set; } = "Prendre photo";
        public string ChoosePictureButtonLabel { get; private set; } = "Choisir depuis la galerie";
        public string DeletePictureButtonLabel { get; private set; } = "Supprimer";
        public string SaisieAutreMotifNCInvite { get; private set; } = "Veuillez saisir le motif\r\nci-dessous";
        public string SaisieAutreMotifNCPlaholder { get; private set; } = "Autre motif, 30 caractères max";
        public string SaisieNumVoyageInvite { get; private set; } = "Veuillez saisir le numéro de bordereau\r\nou l'identifiant de voyage";
        public string SaisieNumVoyagePlaceholder { get; private set; } = "N° bordereau / Id voyage";

        public RelayCommand GefcoFranceCommand { get; set; }
        public RelayCommand InternationalCommand { get; set; }
        public RelayCommand ConfrèresCommand { get; set; }


        public NCViewModel()
        {
            GefcoFranceCommand = new RelayCommand(GotoGefcoFrance, CanExecute);
            InternationalCommand = new RelayCommand(GotoInternational, CanExecute);
            ConfrèresCommand = new RelayCommand(GotoConfrères, CanExecute);
            ValidateProvenanceCommand = new RelayCommand(ValidateAndGo, CanExecute);
            LoadingCommands.Add(ValidateProvenanceCommand);
            ValidateAutreMotifNCCommand = new RelayCommand(() =>
            {
                if (string.IsNullOrWhiteSpace(AutreMotifNC))
                    return;
                IsSuccessInput = true;
                GoBack();
            }, () => IsValidAutreMotifNC);
            LoadingCommands.Add(ValidateAutreMotifNCCommand);
            ValidateNumVoyageCommand = new RelayCommand(ValidateNumVoyageAndGo, () => IsValidNumVoyage);
            LoadingCommands.Add(ValidateNumVoyageCommand);

        }

        private async void GoBack()
        {
            IsLoading = true;
            await NavigationService.PopAsync();
            IsLoading = false;
        }
        private async void GotoGefcoFrance()
        {
            IsLoading = true;
            ProvenanceType = ProvenanceType.France;
            var page = new ChoixProvenancePage(this);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }
        private async void GotoInternational()
        {
            IsLoading = true;
            ProvenanceType = ProvenanceType.International;
            var page = new ChoixProvenancePage(this);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }
        private async void GotoConfrères()
        {
            IsLoading = true;
            ProvenanceType = ProvenanceType.Confrères;
            var page = new ChoixProvenancePage(this);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }

        public ProvenanceType ProvenanceType { get; set; }
        public List<MotifNCViewModel> MotifNCs { get; private set; } = new List<MotifNCViewModel>()
        {
            new MotifNCViewModel(new MotifNC(){Name="Fret trop volumineux", DisplayOrder = 0}),
            new MotifNCViewModel(new MotifNC(){Name="Fret insuffisant", DisplayOrder = 1}),
            new MotifNCViewModel(new MotifNC(){Name="Barre DP cassée", DisplayOrder = 2}),
            new MotifNCViewModel(new MotifNC(){Name="Autre raison", DisplayOrder = 3}),
        };
        public override async Task LoadViewModel()
        {
            if (IsLoaded || IsLoading)
                return;
            IsLoading = true;
            await LoadConfigAsync();
            await LoadResourcesAsync();
            await LoadMotifNCsAsync();
            await LoadDataAsync();

            IsLoading = false;
            IsLoaded = true;
        }

        private async Task LoadConfigAsync()
        {
            if (App.Settings.Configurations.IsNullOrEmpty())
                await App.LoadConfigurationsAsync();
        }
        private async Task LoadMotifNCsAsync()
        {
            if (App.Settings.MotifNCs.IsNullOrEmpty())
                await App.LoadMotifNCsAsync();
            if (App.Settings.MotifNCs.IsNullOrEmpty())
                return;

            MotifNCs = App.Settings.MotifNCs.OrderBy(p => p.DisplayOrder).Select(p => new MotifNCViewModel(p)).ToList();
        }
        private IList<Resource> Resources => App.Settings.Resources;

        private async Task LoadResourcesAsync()
        {
            if (Resources.IsNullOrEmpty())
                await App.LoadResourcesAsync();
            if (Resources.IsNullOrEmpty())
                return;

            PageTitle = Resources.SingleOrDefault(p => p.Key == $"{nameof(NonConformityPage)}.{nameof(PageTitle)}")?.Value ?? PageTitle;
            NextButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(NonConformityPage)}.{nameof(NextButtonLabel)}")?.Value ?? NextButtonLabel;
        }

        private async Task LoadDataAsync()
        {
            var items = await App.LoadProvenancesAsync();
            if (items != null && items.Any())
                FillProvenances(items);
            else if (App.Settings.Agences != null && App.Settings.Agences.Any())
                FillProvenances(App.Settings.Agences);
        }


        private void FillProvenances(IList<Agence> provenances)
        {
            foreach (var provenance in provenances.OrderBy(p => p.Name))
            {
                Provenances.Add(provenance);
            }
        }
        private string _picture1 = "Camera.svg";

        public string Picture1
        {
            get { return _picture1; }
            set
            {
                if (value == _picture1)
                    return;
                _picture1 = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }
        private string _picture3 = "Camera.svg";

        public string Picture3
        {
            get { return _picture3; }
            set
            {
                if (value == _picture3)
                    return;
                _picture3 = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Agence> Provenances { get; } = new ObservableCollection<Agence>();
        private Agence _selectedProvenance;
        public Agence SelectedProvenance
        {
            get => _selectedProvenance;
            set
            {
                if (value == _selectedProvenance)
                    return;
                _selectedProvenance = value;
                ValidateProvenance();
                OnPropertyChanged();
                ValidateProvenanceCommand.RaiseCanExecuteChanged();
            }
        }
        public void ValidateProvenance()
        {
            ErrorMessage = "";

            if (_selectedProvenance == null)
            {
                IsInvalid = true;
                ErrorMessage = "Vous devez sélectionner un élément dans la liste";
            }
            else
            {
                ErrorMessage = "";
                IsInvalid = false;
            }
        }

        private bool _isInvalid;

        public bool IsInvalid
        {
            get { return _isInvalid; }
            set
            {
                if (value == _isInvalid)
                    return;
                _isInvalid = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        public RelayCommand ValidateProvenanceCommand { get; set; }
        public bool IsSuccessInput { get; set; }

        private async void ValidateAndGo()
        {
            if (SelectedProvenance == null)
                return;
            if (!IsInvalid)
            {
                IsLoading = true;
                await InitWorkflowAsync();
                IsComingBack = false;
            }
            IsLoading = true;
            var page = new SaisieNumVoyagePage(this);
            await NavigationService.PushAsync(page);
            IsLoading = false;
        }

        public bool IsComingBack { get; set; }

        private async Task InitWorkflowAsync()
        {
            Reset();
            Declaration = new DeclarationNonConformite()
            {
                CreationDate = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                CurrentWorkflowStep = (int)FlowState.ChoixProvenance,
                AgenceConcernée = SelectedProvenance//, pas besoin de Provenance dans le traitement
            };
        }

        private void Reset()
        {
            State = default(FlowState);
            AutreMotifNC = null;
            MotifNCs.ForEach(p => p.IsChecked = false);
            Picture1 = "Camera.svg";
            Picture2 = "Camera.svg";
            Picture3 = "Camera.svg";
            Declaration = null;
        }

        #region SaisieAutreMotifNC

        private string _autreMotifNC;

        public string AutreMotifNC
        {
            get { return _autreMotifNC; }
            set
            {
                if (value == _autreMotifNC)
                    return;
                _autreMotifNC = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValidAutreMotifNC));
                ValidateAutreMotifNCCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsValidAutreMotifNC
        {
            get
            {
                if (AutreMotifNC == null)
                    return true;

                return !string.IsNullOrWhiteSpace(AutreMotifNC);
            }
        }

        public RelayCommand ValidateAutreMotifNCCommand { get; set; }

        #endregion

        #region SaisieNumVoyage

        private string _numVoyage;

        public string NumVoyage
        {
            get { return _numVoyage; }
            set
            {
                if (value == _numVoyage)
                    return;
                if (value != String.Empty)
                    _numVoyage = value;
                else
                    _numVoyage = null;
                RaisePropertyChanged();
                if (IsValidNumVoyage)
                    _isInvalidNumVoyage = null;
                if (!IsInvalidNumVoyage)
                    NumVoyageErrorMessage = "";

                OnPropertyChanged(nameof(IsValidNumVoyage));
                OnPropertyChanged(nameof(IsInvalidNumVoyage));
                ValidateNumVoyageCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsValidNumVoyage
        {
            get
            {
                if (NumVoyage == null)
                    return true;
                return !string.IsNullOrWhiteSpace(NumVoyage);
            }
        }

        private bool? _isInvalidNumVoyage;
        public bool IsInvalidNumVoyage
        {
            get
            {
                if (_isInvalidNumVoyage.HasValue)
                    return _isInvalidNumVoyage.Value;
                if (NumVoyage == null)
                    return false;
                return string.IsNullOrWhiteSpace(NumVoyage);
            }
            set
            {
                _isInvalidNumVoyage = value;
                OnPropertyChanged();
            }
        }

        private string _numVoyageErrorMessage;

        public string NumVoyageErrorMessage
        {
            get { return _numVoyageErrorMessage; }
            set
            {
                if (value == _numVoyageErrorMessage)
                    return;
                _numVoyageErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ValidateNumVoyageCommand { get; set; }

        private async void ValidateNumVoyageAndGo()
        {
            IsLoading = true;
            if (IsValidNumVoyage && !string.IsNullOrWhiteSpace(NumVoyage))
            {
                NumVoyageErrorMessage = "";
                Declaration.NumVoyage = NumVoyage;
                var page = new ChoixMotifPage(this);
                await NavigationService.PushAsync(page);
            }
            else
            {
                NumVoyageErrorMessage = "Cette information est obligatoire";
                IsInvalidNumVoyage = true;
            }
            IsLoading = false;
        }

        #endregion

        public DeclarationNonConformite Declaration { get; set; }
        private async Task<string> SavePicture(string filePath, PictureType pictureType)
        {
            if (!filePath.IsNullOrWhiteSpace() && filePath != "Camera.svg")
            {
                if (!filePath.StartsWith("http"))
                {
                    var parameter = new Settings.UploadPictureParameter()
                                    {
                                        Picture = new Picture() {PicturePath = filePath, PictureType = (int?) pictureType, DeclarationId = Declaration.Id},
                                        PictureType = Settings.UploadPictureType.NC
                                    };
                    App.Settings.UploadPicture.Add(parameter);
                    App.Settings.UploadPicture = App.Settings.UploadPicture;
                    var bytes = File.ReadAllBytes(filePath);
                    var fileContent = Convert.ToBase64String(bytes);
                    var result = await DataService.UploadPicNC(fileContent, Path.GetFileName(filePath), pictureType, Declaration.Id);
                    if (result.IsSuccess ?? false)
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
        public async Task EndDeclaration()
        {
            IsLoading = true;
            Declaration.AgenceConcernée = SelectedProvenance;
            Declaration.AutreMotifNC = AutreMotifNC;
            Declaration.MotifNCs = MotifNCs.Where(p => p.IsChecked).Select(p => p.MotifNC).ToList();
            var res = await DataService.AddDeclarationNonConformite(Declaration);
            if (res.IsSuccess ?? false)
            {
                if (res.Value ?? false)
                {
                    App.Settings.AddDeclarationNonConformites.RemoveWhere(p => p.Id == Declaration.Id);
                    App.Settings.AddDeclarationNonConformites = App.Settings.AddDeclarationNonConformites;

                    var s = await SavePicture(Picture1, PictureType.Picture1);
                    if (s != null)
                        Picture1 = s;
                    s = await SavePicture(Picture2, PictureType.Picture2);
                    if (s != null)
                        Picture2 = s;
                    s = await SavePicture(Picture3, PictureType.Picture3);
                    if (s != null)
                        Picture3 = s;

                }
            }
            IsLoading = false;
        }

        public FlowState State { get; set; }

    }

    public enum ProvenanceType
    {
        France = 0,
        International = 1,
        Confrères = 2
    }
    public class MotifNCViewModel : ObservableObject
    {
        public static event Action<MotifNCViewModel> Checked;
        public MotifNC MotifNC { get; }
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

        public MotifNCViewModel(MotifNC motifNC)
        {
            MotifNC = motifNC;
        }
    }

}
