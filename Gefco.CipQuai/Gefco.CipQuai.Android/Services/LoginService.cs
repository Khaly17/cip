using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Services;
using Newtonsoft.Json;
using Xamarin.Auth;

namespace Gefco.CipQuai.Droid.Services
{
    public class LoginService : ILoginService
    {

        public async Task SaveAccount(ApplicationUser user)
        {
            var account = new Account { Username = App.AppId };
            account.Properties.Add("ApplicationUser", JsonConvert.SerializeObject(user));
            account.Properties.Add("LoggedIn", "true");
            await SecureStorageAccountStore.SaveAsync(account, App.AppId);
        }

        public async Task<ApplicationUser> GetUser()
        {
            var account = (await SecureStorageAccountStore.FindAccountsForServiceAsync(App.AppId)).LastOrDefault();
            if (account == null)
                return null;

            var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(account.Properties["ApplicationUser"]);
            return applicationUser;
        }

        public async Task Logout()
        {
            var account = (await SecureStorageAccountStore.FindAccountsForServiceAsync(App.AppId)).FirstOrDefault();
            if (account != null)
            {
                var args = new CancelEventArgs();
                LoggingOut?.Invoke(this, args);
                if (args.Cancel)
                    return;
                //accountStore.Delete(account, App.AppId);
                account.Properties["LoggedIn"] = "false";
                await SecureStorageAccountStore.SaveAsync(account, App.AppId);
                MainActivity.Instance.FinishAffinity();
            }
        }
        public void Restart()
        {
            MainActivity.Instance.FinishAffinity();
        }

        public event EventHandler<CancelEventArgs> LoggingOut;
        public async Task<bool> IsLoggedIn()
        {
            var account = (await SecureStorageAccountStore.FindAccountsForServiceAsync(App.AppId)).FirstOrDefault();
            if (account == null)
                return false;

            var loggedIn = account.Properties["LoggedIn"];
            return loggedIn == "true";
        }
    }
}