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

namespace Gefco.CipQuai.BestPracticePage
{
    public enum FlowState
    {
        Start = 0,
        ChoixActeur = 1,
        DescriptionBP = 2,
        RésuméActeurDescription = 3,
        TakePictures = 4,
        Summary = 5,
        EndDeclaration = 6
    }

    public class BPViewModel : ViewModelBaseExt
    {
        public string PageTitle
        {
            get => $"Bonne pratique";
            set { }
        }

        public string StartPageInvite => $"Veuillez indiquer l'acteur concerné";
        public string NextButtonLabel { get; private set; } = "Suivant >";
        public string ActeurInviteLabel { get; } = "Acteur concerné";
        public string DescriptionInviteLabel { get; } = "Description";
        public string NoResultsMessage { get; private set; } = "Aucun acteur trouvé...";
        public string SaisieDescriptionInvite { get; private set; } = "Veuillez décrire la bonne\r\npratique";
        public string SaisieDescriptionPlaceholder { get; private set; } = "Description bonne pratique";
        public string CancelDeclarationInvite { get; private set; } = "Vous êtes sur le point d'abandonner votre déclaration,\r\nsouhaitez vous continuer ?";
        public string ContinueButtonInvite { get; private set; } = "Continuer";
        public string CancelInvite { get; private set; } = "Annuler";
        public string RetakePictureInvite { get; private set; } = "Refaire photo";
        public string TakeNextPictureInvite { get; private set; } = "Conserver photo et\r\nprendre nouvelle photo";
        public string EndImpossibleDeclarationInvite { get; private set; } = "Conserver photo et\r\nTerminer déclaration";
        public string RésuméActeurDescriptionTakePictureButtonLabel { get; private set; } = "Ajouter photo BP\r\net / ou selfie";
        public string TakePictureButtonLabel { get; private set; } = "Prendre photo";
        public string ChoosePictureButtonLabel { get; private set; } = "Choisir depuis la galerie";
        public string DeletePictureButtonLabel { get; private set; } = "Supprimer";


        public BPViewModel()
        {
            ValidateActeurCommand = new RelayCommand(ValidateAndGo, CanExecute);
            LoadingCommands.Add(ValidateActeurCommand);
            ValidateDescriptionCommand = new RelayCommand(ValidateDescriptionAndGo, () => IsValidDescription);
            LoadingCommands.Add(ValidateDescriptionCommand);
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

            IsLoading = false;
            IsLoaded = true;
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

            PageTitle = Resources.SingleOrDefault(p => p.Key == $"{nameof(BestPracticePage)}.{nameof(PageTitle)}")?.Value ?? PageTitle;
            NextButtonLabel = Resources.SingleOrDefault(p => p.Key == $"{nameof(BestPracticePage)}.{nameof(NextButtonLabel)}")?.Value ?? NextButtonLabel;
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
        private string _selectedActeur;
        public string SelectedActeur
        {
            get => _selectedActeur;
            set
            {
                if (value == _selectedActeur)
                    return;
                _selectedActeur = value;
                ValidateActeur();
                OnPropertyChanged();
                ValidateActeurCommand.RaiseCanExecuteChanged();
            }
        }
        public void ValidateActeur(bool toNext = false)
        {
            ErrorMessage = "";

            if (_selectedActeur == null)
            {
                IsInvalid = true;
                ErrorMessage = "Vous devez saisir un nom d'acteur concerné";
            }
            else if (toNext && _selectedActeur.IsNullOrWhiteSpace())
            {
                IsInvalid = true;
                ErrorMessage = "Vous devez saisir un nom d'acteur concerné";
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

        public RelayCommand ValidateActeurCommand { get; set; }
        public bool IsSuccessInput { get; set; }

        private async void ValidateAndGo()
        {
            IsLoading = true;
            ValidateActeur(true);
            if (!IsInvalid)
            {
                await InitWorkflowAsync();
                IsComingBack = false;
                var page = new SaisieDescriptionBPPage(this);
                await NavigationService.PushAsync(page);
            }
            IsLoading = false;
        }
        private async void ValidateDescriptionAndGo()
        {
            if (IsValidDescription && !string.IsNullOrWhiteSpace(Description))
            {
                IsLoading = true;
                var page = new RésuméActeurDescriptionPage(this);
                await NavigationService.PushAsync(page);
                IsLoading = false;
            }
        }

        public bool IsComingBack { get; set; }

        private async Task InitWorkflowAsync()
        {
            Reset();
            Declaration = new DeclarationBonnePratique()
            {
                CreationDate = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                CurrentWorkflowStep = (int)FlowState.ChoixActeur,
                AutreAgentConcerné = SelectedActeur
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

        public DeclarationBonnePratique Declaration { get; set; }
        private async Task<string> SavePictureAsync(string filePath, PictureType pictureType)
        {
            if (!filePath.IsNullOrWhiteSpace() && filePath != "Camera.svg")
            {
                if (!filePath.StartsWith("http"))
                {
                    var parameter = new Settings.UploadPictureParameter()
                                    {
                                        Picture = new Picture() {PicturePath = filePath, PictureType = (int?) pictureType, DeclarationId = Declaration.Id},
                                        PictureType = Settings.UploadPictureType.BP
                                    };
                    App.Settings.UploadPicture.Add(parameter);
                    App.Settings.UploadPicture = App.Settings.UploadPicture;
                    var bytes = File.ReadAllBytes(filePath);
                    var fileContent = Convert.ToBase64String(bytes);
                    var result = await DataService.UploadPicBP(fileContent, Path.GetFileName(filePath), pictureType, Declaration.Id);
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
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                    return;
                _description = value;
                RaisePropertyChanged();
                OnPropertyChanged(nameof(IsValidDescription));
                ValidateDescriptionCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsValidDescription
        {
            get
            {
                if (Description == null)
                    return true;
                return !string.IsNullOrWhiteSpace(Description);
            }
        }

        public RelayCommand ValidateDescriptionCommand { get; set; }

        public async Task EndDeclaration()
        {
            IsLoading = true;
            Declaration.AutreAgentConcerné = SelectedActeur;
            Declaration.Description = Description;
            var before = DateTime.Now;
            var res = await DataService.AddDeclarationBonnePratique(Declaration);
            var now = DateTime.Now;
            Console.WriteLine($"{nameof(DataService.AddDeclarationBonnePratique)} took {(now-before).TotalMilliseconds} ms to execute");
            if (res.IsSuccess ?? false)
            {
                App.Settings.AddDeclarationBonnePratiques.RemoveWhere(p => p.Id == Declaration.Id);
                App.Settings.AddDeclarationBonnePratiques = App.Settings.AddDeclarationBonnePratiques;
                var s = await SavePictureAsync(Picture1, PictureType.Picture1);
                if (s != null)
                    Picture1 = s;
                now = DateTime.Now;
                Console.WriteLine($"{nameof(SavePictureAsync)} 1 took {(now-before).TotalMilliseconds} ms to execute");
                before = DateTime.Now;
                s = await SavePictureAsync(Picture2, PictureType.Picture2);
                if (s != null)
                    Picture2 = s;
                now = DateTime.Now;
                Console.WriteLine($"{nameof(SavePictureAsync)} 2 took {(now-before).TotalMilliseconds} ms to execute");
                before = DateTime.Now;
                s = await SavePictureAsync(Picture3, PictureType.Picture3);
                if (s != null)
                    Picture3 = s;
                now = DateTime.Now;
                Console.WriteLine($"{nameof(SavePictureAsync)} 3 took {(now-before).TotalMilliseconds} ms to execute");
            }
            else
            {
                now = DateTime.Now;
                Console.WriteLine($"{nameof(DataService.AddDeclarationBonnePratique)} took {(now-before).TotalMilliseconds} ms to FAIL");
            }
            IsLoading = false;
        }

        public FlowState State { get; set; }

    }
}
