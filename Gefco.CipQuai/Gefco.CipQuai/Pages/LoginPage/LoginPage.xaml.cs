using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Gefco.CipQuai.LoginPage.Views;

namespace Gefco.CipQuai.LoginPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : INavigationHandler
	{
	    public LoginPage()
		{
		    InitializeComponent();

		    this.BindingContext = new LoginPageViewModel { NavigationHandler = this };

            this.Content = new LoginView();
        }

	    public void LoadView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.LoginView:
                    this.Content = new LoginView();
                    break;
                case ViewType.PasswordResetView:
                    this.Content = new PasswordResetView();
                    break;
                case ViewType.PasswordResetConfirmedView:
                    this.Content = new PasswordResetConfirmedView();
                    break;
            }
        }
        
        protected override bool OnBackButtonPressed()
        {
            var viewType = this.Content.GetType();

            if (viewType == typeof(PasswordResetView))
            {
                this.Content = new LoginView();
                return true;
            }

            return base.OnBackButtonPressed();
        }
    }
}