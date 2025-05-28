using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonServiceLocator;
using FFImageLoading;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Extensions;
using Gefco.CipQuai.Services;

namespace Gefco.CipQuai.ProfilePage
{
    public class ViewModel : ViewModelBaseExt
    {
        public string PageTitle => "Profil";
        public string FirstNameInvite => "Prénom";
        public string LastNameInvite => "Nom";
        public string EmailInvite => "Adresse e-mail";
        public string CodePinInvite => "Code pin";
        public string ModifierCodePinLink => "Modifier code pin";
        public string DéconnexionLink => "Déconnexion";
        public string ModifierCodePinInvite => "Pour des raisons de sécurité,\r\nVeuillez saisir votre code pin";
        public string ForcerModifierCodePinInvite => "Pour des raisons de sécurité,\r\nVeuillez modifier votre code pin";
        public string CodePinActuelInvite => "Code pin actuel";
        public string NouveauPinInvite => "Nouveau code pin à quatre chiffres";
        public string CodePinInvalide => "Le code pin saisi est incorrect";
        public string NouveauPinComporte4Chiffres => "Comporte quatre chiffres";
        public string NouveauPinDifférentActuel => "Différent du code actuel";
        public string TakePictureButtonLabel { get; private set; } = "Prendre photo";
        public string ChoosePictureButtonLabel { get; private set; } = "Choisir depuis la galerie";
        public string CancelInvite { get; private set; } = "Annuler";

        public RelayCommand LogoutCommand { get; }
        private void Logout()
        {
            ServiceLocator.Current.GetInstance<ILoginService>().Logout();
        }

        public RelayCommand ModifierCodePinCommand { get; }
        private async void ModifierCodePin()
        {
            IsLoading = true;
            if (CodePin == null)
                return;
            await NavigationService.PushAsync(new ModifierCodePinPage(this));
            await NavigationService.PopModalAsync(false);
            IsLoading = false;
        }
        public RelayCommand VerifierCodePinCommand { get; }
        private async void VerifierCodePin()
        {
            IsLoading = true;
            CodePin = null;
            await NavigationService.PushModalAsync(new VerifierCodePinPage(this));
            IsLoading = false;
        }
        public RelayCommand ModifierLastNameCommand { get; }
        private async void ModifierLastName()
        {
            IsLoading = true;
            await NavigationService.PushModalAsync(new ModifierLastNamePage(this));
            IsLoading = false;
        }
        public RelayCommand ModifierFirstNameCommand { get; }
        private async void ModifierFirstName()
        {
            IsLoading = true;
            await NavigationService.PushModalAsync(new ModifierFirstNamePage(this));
            IsLoading = false;
        }

        public ApplicationUser User => App.Settings.User;

        #region FirstName

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName)
                    return;
                _firstName = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsValidFirstName));
                SaveFirstNameCommand.RaiseCanExecuteChanged();
            }
        }

        private string _userFirstName;

        public string UserFirstName
        {
            get { return _userFirstName; }
            set
            {
                if (value == _userFirstName)
                    return;
                _userFirstName = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValidFirstName => !FirstName.IsNullOrWhiteSpace();

        public RelayCommand SaveFirstNameCommand { get; }
        private async void SaveFirstName()
        {
            IsLoading = true;
            User.FirstName = FirstName;
            UserFirstName = FirstName;
            App.Settings.User = User;
            SaveUser();
            await NavigationService.PopModalAsync(true);
            IsLoading = false;
        }
        

        #endregion

        #region LastName

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value == _lastName)
                    return;
                _lastName = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsValidLastName));
                SaveLastNameCommand.RaiseCanExecuteChanged();
            }
        }

        private string _userLastName;

        public string UserLastName
        {
            get { return _userLastName; }
            set
            {
                if (value == _userLastName)
                    return;
                _userLastName = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValidLastName => !LastName.IsNullOrWhiteSpace();

        public RelayCommand SaveLastNameCommand { get; }
        private async void SaveLastName()
        {
            IsLoading = true;
            User.LastName = LastName;
            UserLastName = LastName;
            App.Settings.User = User;
            SaveUser();
            await NavigationService.PopModalAsync(true);
            IsLoading = false;
        }


        #endregion

        #region CodePin

        private string _codePin;

        public string CodePin
        {
            get { return _codePin; }
            set
            {
                if (value == _codePin)
                    return;
                _codePin = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsValidCodePin));
                ModifierCodePinCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsValidCodePin
        {
            get
            {
                if (CodePin == null)
                    return true;
                return CodePin == (App.Settings.UserPin ?? "1234");
            }
        }

        #endregion

        #region NouveauPin

        private string _nouveauPin;

        public string NouveauPin
        {
            get { return _nouveauPin; }
            set
            {
                if (value == _nouveauPin)
                    return;
                _nouveauPin = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsValidNouveauPin));
                RaisePropertyChanged(nameof(HasLengthNouveauPin));
                RaisePropertyChanged(nameof(IsDifferentNouveauPin));
                SaveNouveauPinCommand.RaiseCanExecuteChanged();
            }
        }

        public bool? HasLengthNouveauPin => string.IsNullOrWhiteSpace(NouveauPin) ? (bool?) null : NouveauPin.Length == 4;
        public bool? IsDifferentNouveauPin => string.IsNullOrWhiteSpace(NouveauPin) ? (bool?) null : (HasLengthNouveauPin ?? false) ? NouveauPin != CodePin : (bool?) null;

        public bool IsValidNouveauPin => (HasLengthNouveauPin ?? false) && (IsDifferentNouveauPin ?? false);

        public RelayCommand SaveNouveauPinCommand { get; }

        private async void SaveNouveauPin()
        {
            IsLoading = true;
            try
            {
                await SavePin();
                App.Settings.UserPin = NouveauPin;
                App.Settings.User.NeedsChangePin = false;
                var loginService = ServiceLocator.Current.GetInstance<ILoginService>();
                loginService.SaveAccount(App.Settings.User);
                try
                {
                    await NavigationService.PopAsync(true);
                }
                catch (Exception e)
                {
                }
                try
                {
                    await NavigationService.PopModalAsync(true);
                }
                catch (Exception e)
                {
                }
            }
            catch (Exception e)
            {
            }

            IsLoading = false;
        }


        #endregion

        public ViewModel()
        {
            LogoutCommand = new RelayCommand(Logout, CanExecute);
            LoadingCommands.Add(LogoutCommand);
            VerifierCodePinCommand = new RelayCommand(VerifierCodePin, CanExecute);
            LoadingCommands.Add(VerifierCodePinCommand);
            ModifierCodePinCommand = new RelayCommand(ModifierCodePin, CanExecute);
            LoadingCommands.Add(ModifierCodePinCommand);
            ModifierLastNameCommand = new RelayCommand(ModifierLastName, CanExecute);
            LoadingCommands.Add(ModifierLastNameCommand);
            ModifierFirstNameCommand = new RelayCommand(ModifierFirstName, CanExecute);
            LoadingCommands.Add(ModifierFirstNameCommand);
            SaveFirstNameCommand = new RelayCommand(SaveFirstName, () => CanExecute && IsValidFirstName);
            LoadingCommands.Add(SaveFirstNameCommand);
            SaveLastNameCommand = new RelayCommand(SaveLastName, () => CanExecute && IsValidLastName);
            LoadingCommands.Add(SaveLastNameCommand);
            SaveNouveauPinCommand = new RelayCommand(SaveNouveauPin, () => CanExecute && IsValidNouveauPin);
            LoadingCommands.Add(SaveNouveauPinCommand);

#if DEBUG
            FirstName = User?.FirstName ?? "Mickaël";
            LastName = User?.LastName ?? "Martin";
#else
            FirstName = User.FirstName;
            LastName = User.LastName;
#endif
            UserFirstName = FirstName;
            UserLastName = LastName;
            UserPicture = User?.ProfilePicture?.PicturePath ?? "User.svg";
            CodePin = App.Settings.UserPin;
        }

        private string _userPicture;

        public string UserPicture
        {
            get { return _userPicture; }
            set
            {
                if (value == _userPicture)
                    return;
                _userPicture = value;
                App.Settings.ProfilePicture = value;
                RaisePropertyChanged();
            }
        }

        private async Task<string> SavePicture(string filePath)
        {
            if (!filePath.IsNullOrWhiteSpace() && filePath != "User.svg")
            {
                if (!filePath.StartsWith("http"))
                {
                    var parameter = new Settings.UploadPictureParameter()
                                    {
                                        Picture = new Picture() {PicturePath = filePath, PictureType = (int?) PictureType.Profile},
                                        PictureType = Settings.UploadPictureType.Profile
                                    };
                    App.Settings.UploadPicture.Add(parameter);
                    App.Settings.UploadPicture = App.Settings.UploadPicture;
                    var bytes = File.ReadAllBytes(filePath);
                    var fileContent = Convert.ToBase64String(bytes);
                    var before = DateTime.Now;
                    var result = await DataService.UploadProfilePic(fileContent, Path.GetFileName(filePath));
                    if (result.IsSuccess ?? false)
                    {
                        var now = DateTime.Now;
                        await ServiceDialog.Instance.ShowMessage($"Took {(now - before).TotalMilliseconds} ms to execute", "Ok");
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

        private async Task UpdateUser()
        {
            User.FirstName = UserFirstName;
            User.LastName = UserLastName;
            var s = await SavePicture(UserPicture);
            if (s != null)
                UserPicture = s;
        }
        public async void SaveUser()
        {
            await UpdateUser();
            await DataService.UpdateUserAsync(User);
        }
        public async Task SavePin()
        {
            await DataService.ChangePassword(CodePin, NouveauPin);
        }

        public async void SaveUserPicture()
        {
            var s = await SavePicture(UserPicture);
            if (s != null)
                UserPicture = s;
        }
    }
}