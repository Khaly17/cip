using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.DoubleDeckPage;
using Gefco.CipQuai.Extensions;
using Gefco.CipQuai.Services;

namespace Gefco.CipQuai.ControleReceptionPage
{
    public enum FlowState
    {
        Start = 0,
        ChoixAgence = 1,
        TakePictures = 2,
        Summary = 3,
        EndDeclaration = 4
    }

    public class CRViewModel : ViewModelBaseExt
    {
        public string PageTitle
        {
            get => $"Contrôle arrivage";
            set { }
        }

        public string StartPageInvite => $"Veuillez indiquer l'agence de provenance";
        public string NextButtonLabel { get; private set; } = "Suivant >";
        public string DestinationInviteLabel { get; private set; } = "Agence de provenance";
        public string NoResultsMessage { get; private set; } = "Aucune agence trouvée...";
        public string CancelDeclarationInvite { get; private set; } = "Vous êtes sur le point d'abandonner votre déclaration,\r\nsouhaitez vous continuer ?";
        public string ContinueButtonInvite { get; private set; } = "Continuer";
        public string CancelInvite { get; private set; } = "Annuler";
        public string RetakePictureInvite { get; private set; } = "Refaire photo";
        public string TakeNextPictureInvite { get; private set; } = "Conserver photo et\r\nprendre nouvelle photo";
        public string EndImpossibleDeclarationInvite { get; private set; } = "Conserver photo et\r\nTerminer déclaration";
        public string RésuméAgenceDescriptionTakePictureButtonLabel { get; private set; } = "Ajouter photo";
        public string TakePictureButtonLabel { get; private set; } = "Prendre photo";
        public string ChoosePictureButtonLabel { get; private set; } = "Choisir depuis la galerie";
        public string DeletePictureButtonLabel { get; private set; } = "Supprimer";


        public CRViewModel()
        {
            ValidateDestinationCommand = new RelayCommand(ValidateDestinationAndGo, () => !IsInvalidDestination);
            LoadingCommands.Add(ValidateDestinationCommand);
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

        public ObservableCollection<Agence> Destinations { get; } = new ObservableCollection<Agence>();
        private Agence _selectedDestination;
        public string AgenceDestination
        {
            get => SelectedDestination?.Name;
            set { }
        }
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
                OnPropertyChanged(nameof(AgenceDestination));
                ValidateDestinationCommand.RaiseCanExecuteChanged();
            }
        }

        private Agence _agenceArrivee;
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
        public RelayCommand ValidateDestinationCommand { get; set; }
        public async void ValidateDestinationAndGo()
        {
            IsLoading = true;
            ValidateDestination();
            if (_selectedDestination != null)
            {
                var oldDeclaration = Declaration;
                var isComingBack = IsComingBack;
                await InitWorkflowAsync();
                IsComingBack = false;
                var page = new TakePicturesPage(this);
                await NavigationService.PushAsync(page);
                IsLoading = false;
                return;
            }
            IsLoading = false;
        }

        private async void GoBack()
        {
            IsLoading = true;
            await NavigationService.PopAsync();
            IsLoading = false;
        }

        public override async Task LoadViewModel()
        {
            if (IsLoaded || IsLoading)
                return;
            IsLoading = true;
            await LoadConfigAsync();
            await LoadResourcesAsync();
            await LoadDataAsync();

            IsLoading = false;
            IsLoaded = true;
        }
        private async Task LoadDataAsync()
        {
            var items = await App.LoadAgencesCRAsync();
            if (items != null && items.Any())
                FillDestinations(items);
            else if (App.Settings.AgencesCR != null && App.Settings.AgencesCR.Any())
                FillDestinations(App.Settings.AgencesCR);
        }

        private void FillDestinations(IList<Agence> destinations)
        {
            foreach (var destination in destinations.OrderBy(p => p.Name))
            {
                Destinations.Add(destination);
            }
        }

        private async Task LoadConfigAsync()
        {
            if (App.Settings.Configurations.IsNullOrEmpty())
                await App.LoadConfigurationsAsync();
        }
        private IList<Resource> Resources => App.Settings.Resources;

        private async Task LoadResourcesAsync()
        {
            if (Resources.IsNullOrEmpty())
                await App.LoadResourcesAsync();
            if (Resources.IsNullOrEmpty())
                return;

            PageTitle = Resources.SingleOrDefault(p => p.Key == $"{nameof(SimpleDeckPage)}.{nameof(PageTitle)}")?.Value ?? PageTitle;
            NextButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(SimpleDeckPage)}.{nameof(NextButtonLabel)}")?.Value ?? NextButtonLabel;
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

        public bool IsSuccessInput { get; set; }

        public bool IsComingBack { get; set; }

        private async Task InitWorkflowAsync()
        {
            Reset();
            Declaration = new DeclarationControleReception()
            {
                CreationDate = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                CurrentWorkflowStep = (int)FlowState.ChoixAgence,
                AutreAgenceArrivee = AgenceArrivee.Name
            };
        }

        private void Reset()
        {
            State = default(FlowState);
            Picture1 = "Camera.svg";
            Picture2 = "Camera.svg";
            Picture3 = "Camera.svg";
            Declaration = null;
        }

        public DeclarationControleReception Declaration { get; set; }
        private async Task<string> SavePictureAsync(string filePath, PictureType pictureType)
        {
            if (!filePath.IsNullOrWhiteSpace() && filePath != "Camera.svg")
            {
                if (!filePath.StartsWith("http"))
                {
                    var parameter = new Settings.UploadPictureParameter()
                                    {
                                        Picture = new Picture() {PicturePath = filePath, PictureType = (int?) pictureType, DeclarationId = Declaration.Id},
                                        PictureType = Settings.UploadPictureType.CR
                                    };
                    App.Settings.UploadPicture.Add(parameter);
                    App.Settings.UploadPicture = App.Settings.UploadPicture;
                    var bytes = File.ReadAllBytes(filePath);
                    var fileContent = Convert.ToBase64String(bytes);
                    var result = await DataService.UploadPicCR(fileContent, Path.GetFileName(filePath), pictureType, Declaration.Id);
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
            Declaration.AutreAgenceArrivee = AgenceArrivee.Name;
            var res = await DataService.AddDeclarationControleReception(Declaration);
            if (res.IsSuccess ?? false)
            {
                App.Settings.AddDeclarationControleReceptions.RemoveWhere(p => p.Id == Declaration.Id);
                App.Settings.AddDeclarationControleReceptions = App.Settings.AddDeclarationControleReceptions;
                var s = await SavePictureAsync(Picture1, PictureType.Picture1);
                if (s != null)
                    Picture1 = s;
                s = await SavePictureAsync(Picture2, PictureType.Picture2);
                if (s != null)
                    Picture2 = s;
                s = await SavePictureAsync(Picture3, PictureType.Picture3);
                if (s != null)
                    Picture3 = s;
            }
            IsLoading = false;
        }

        public FlowState State { get; set; }

    }
}
