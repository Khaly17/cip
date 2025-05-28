using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Gefco.CipQuai.ApiClient.Models;

namespace Gefco.CipQuai.Services
{
    public interface ILoginService
    {
        Task SaveAccount(ApplicationUser user);
        Task<ApplicationUser> GetUser();
        Task Logout();
        void Restart();
        event EventHandler<CancelEventArgs> LoggingOut;
        Task<bool> IsLoggedIn();
    }
}