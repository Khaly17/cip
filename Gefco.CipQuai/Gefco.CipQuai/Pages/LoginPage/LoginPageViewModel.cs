using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Services;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace Gefco.CipQuai.LoginPage
{
    public class LoginPageViewModel : ViewModelBaseExt
    {
        private string _email;
        private bool _isLoading;
        private string _password;
        private string _username;


        public LoginPageViewModel()
        {
            GoToViewCommand = new Command(LoadView);
            LoginCommand = new Command(async (o) => await ExecuteLogin(o), (o) => CanExecute);
            ResetPasswordCommand = new Command(SubmitPasswordReset, () => CanExecute);
#if DEBUG
            Username = "sv";
            Password = "1234";
#endif
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username == value)
                    return;

                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email == value)
                    return;

                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value)
                    return;

                _password = value;
                OnPropertyChanged();
            }
        }

        public string LoginInvite { get; } = "Bienvenue sur l'application CIP QUAI\r\nVeuillez vous identifier avec les informations\r\nreçues par mail.";
        public string LostPasswordInvite { get; } = "Veuillez saisir votre adresse mail afin qu'un\r\nlien vous permettant de redéfinir\r\nvotre code pin vous soit envoyé.";
        public string LostPasswordConfirmedInvite { get; } = "Le lien vous permettant de redéfinir votre\r\ncode pin vous a été envoyé par email.\r\n\r\nVérifiez votre boite mail sans oublier\r\nle dossier des spams !";
        public Command GoToViewCommand { get; set; }

        public Command LoginCommand { get; set; }

        public Command ResetPasswordCommand { get; set; }

        public INavigationHandler NavigationHandler { private get; set; }

        public new bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanExecute));
            }
        }

        public override bool CanExecute => !_isLoading;

        private void LoadView(object viewType)
        {
            NavigationHandler.LoadView((ViewType) viewType);
        }

        private async Task ExecuteLogin(object obj)
        {
            switch ((LoginType) obj)
            {
                case LoginType.Normal:
                    //await Application.Current.MainPage.DisplayAlert("Normal Login", $"You have logged in with:\r\nUsername: {Username}\nPassword: {Password}", "ok");

                    if (IsLoading)
                        return;

                    IsLoading = true;

                    try
                    {
                        ApplicationUser user = null;
                        if (Username == "sv" && Password == "1234")
                            user = await DataService.Login("sv@sensor6ty.com", "1357");
                        else if (Username == "sc" && Password == "1234")
                            user = await DataService.Login("sc@gefco.net", "Carlier1234!");
                        else if (Username == "fv" && Password == "1234")
                            user = await DataService.Login("fv@gefco.net", "Fred1234!");
                        else if (Username == "zs" && Password == "1234")
                            user = await DataService.Login("zs@gefco.net", "Zoran1234!");
                        else
                            user = await DataService.Login(Username, Password);
                        if (user != null && !string.IsNullOrEmpty(user.Id) && user.Id != Guid.Empty.ToString())
                        {
                            App.Settings.UserPin = Password;
                            App.Settings.User = user;
                            App.Settings.ProfilePicture = user?.ProfilePicture?.PicturePath ?? "User.svg";

                            var loginService = ServiceLocator.Current.GetInstance<ILoginService>();
                            loginService.SaveAccount(user);

                            NavigateToSuccessPage();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Attention requise", "L'authentification a échoué, nous vous conseillons de vérifier votre login ainsi que votre mot de passe, merci.", "ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
#if DEBUG
                        throw;
#else
                        await Application.Current.MainPage.DisplayAlert("Attention requise", "L'authentification a échoué, nous vous conseillons de vérifier votre login ainsi que votre mot de passe, merci.", "ok");
#endif
                    }
                    IsLoading = false;


                    break;
                case LoginType.SignUp:
                    // Use Username, Email and Password properties
                    await Application.Current.MainPage.DisplayAlert("Signup Login", $"You have signed up with:\r\nUsername: {Username}\nEmail:{Email}\nPassword: {Password}", "ok");
                    break;
                case LoginType.PasswordReset:
                    try
                    {
                        await DataService.ForgotPassword(Email);
                        LoadView(ViewType.PasswordResetConfirmedView);
                        //await Application.Current.MainPage.DisplayAlert("Information", $"Le lien", "ok");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }                    // Use Email property for reset request
                    break;
            }
        }

        private void SubmitPasswordReset()
        {
            // Use the Email property
            Application.Current.MainPage.DisplayAlert("Password Reset", $"You have requested a password reset for: {Email}", "ok");
        }

        public void NavigateToSuccessPage()
        {
            var app = (App) Application.Current;
            Application.Current.MainPage = new LoadingPage.LoadingPage(app.DependencyRegistrator);
            //Application.Current.MainPage = new MainPage();
        }
    }
}