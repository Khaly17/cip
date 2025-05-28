using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gefco.CipQuai.ApiClient;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Extensions;
using Microsoft.Rest;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Gefco.CipQuai
{
    public static class DataService
    {
        static DataService()
        {
#if DEBUG
            ServiceClientTracing.IsEnabled = true;
            ServiceClientTracing.AddTracingInterceptor(new Interceptor());
#endif
        }

        public static async Task<ApplicationUser> Login(string userName, string password)
        {
            var isConnected = App.ServiceNetwork.IsConnected();
            if (!isConnected)
                return null;
            try
            {
                var result = await GefcoCipQuaiWeb.Instance.Account.LoginAsync(VersionTracking.CurrentVersion, userName, password);
                return result;
            }
            catch (HttpOperationException ex1)
            {
                await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception e)
            {
                await App.ServiceDialog.ShowError(e, "Error in " + nameof(Login), "OK", () => { });
            }

            return null;
        }

        public static async Task<object> ChangePassword(string oldPassword, string newPassword)
        {
            var isConnected = App.ServiceNetwork.IsConnected();
            if (!isConnected)
                return null;
            try
            {
                var result =
                    await GefcoCipQuaiWeb.Instance.Account.ChangePasswordAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, oldPassword, newPassword);
                return 200;
            }
            catch (HttpOperationException ex1)
            {
                await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception e)
            {
                await App.ServiceDialog.ShowError(e, "Error in " + nameof(ChangePassword), "OK", () => { });
            }

            return null;
        }
        public static async Task<object> ForgotPassword(string email)
        {
            var isConnected = App.ServiceNetwork.IsConnected();
            if (!isConnected)
                return null;
            try
            {
                var result = await GefcoCipQuaiWeb.Instance.Account.ForgotPasswordAsync(VersionTracking.CurrentVersion, email);
                return result;
            }
            catch (HttpOperationException ex1)
            {
                await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception e)
            {
                await App.ServiceDialog.ShowError(e, "Error in " + nameof(ChangePassword), "OK", () => { });
            }

            return null;
        }

        public static async Task<UserServiceResult> UpdateUser(Settings.UpdateUserParameters parameter)
        {
            var isConnected = App.ServiceNetwork.IsConnected();
            if (!isConnected)
                return null;
            try
            {
                var result = await GefcoCipQuaiWeb.Instance.Account.UpdateAsync(VersionTracking.CurrentVersion, parameter.UserId, parameter.Email, parameter.FirstName, parameter.LastName, parameter.PhoneNumber);
                return result;
            }
            catch (HttpOperationException ex1)
            {
                await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception e)
            {
                await App.ServiceDialog.ShowError(e, "Error in " + nameof(UpdateUser), "OK", () => { });
            }

            return null;
        }

        public static async Task<BooleanServiceResult> SetDevice(string deviceToken)
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            BooleanServiceResult res = new BooleanServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.Account.SetDeviceAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, 1, deviceToken);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<ConfigurationListServiceResult> GetConfigurations()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new ConfigurationListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.Business.GetConfigurationsAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception )
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<MotifDPListServiceResult> GetMotifDPs()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new MotifDPListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationDoublePlancher.GetMotifDPsAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<MotifNCListServiceResult> GetMotifNCs()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new MotifNCListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationNonConformite.GetMotifNCsAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<AgenceListServiceResult> GetAgences()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new AgenceListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationNonConformite.GetAgencesAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<AgenceListServiceResult> GetAgencesCR()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new AgenceListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationControleReception.GetAgencesCRAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<ApplicationUserListServiceResult> GetActeurs()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new ApplicationUserListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationBonnePratique.GetActeursAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<ResourceListServiceResult> GetResources()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new ResourceListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.Business.GetResourcesAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, App.Settings.LastResourceUpdate);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<RemorqueStatusListServiceResult> GetRemorqueStatuses()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new RemorqueStatusListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.Business.GetRemorqueStatusesAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<DeclarationDoublePlancherListServiceResult> GetActiveDeclarationDoublePlanchers()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new DeclarationDoublePlancherListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationDoublePlancher.GetActiveDeclarationDoublePlanchersAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
        public static async Task<StringServiceResult> AddDeclarationDoublePlancher(DeclarationDoublePlancher declaration, string tractionId)
        {
            var res = new StringServiceResult();

            App.Settings.AddDeclarationDoublePlanchers.Add(declaration);
            App.Settings.AddDeclarationDoublePlanchers = App.Settings.AddDeclarationDoublePlanchers;

            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                goto End;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationDoublePlancher.AddDeclarationDoublePlancherAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, declaration, tractionId);
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            End:
            if (res.IsSuccess ?? false)
            {
                App.Settings.AddDeclarationDoublePlanchers.Remove(declaration);
                App.Settings.AddDeclarationDoublePlanchers = App.Settings.AddDeclarationDoublePlanchers;
            }
            return res;
        }
        public static async Task<BooleanServiceResult> AddDeclarationNonConformite(DeclarationNonConformite declaration)
        {
            var res = new BooleanServiceResult();

            App.Settings.AddDeclarationNonConformites.Add(declaration);
            App.Settings.AddDeclarationNonConformites = App.Settings.AddDeclarationNonConformites;

            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                goto End;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationNonConformite.AddDeclarationNonConformiteAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, declaration);
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            End:
            if (res.IsSuccess ?? false)
            {
                App.Settings.AddDeclarationNonConformites.Remove(declaration);
                App.Settings.AddDeclarationNonConformites = App.Settings.AddDeclarationNonConformites;
            }
            return res;
        }
        public static async Task<BooleanServiceResult> AddDeclarationBonnePratique(DeclarationBonnePratique declaration)
        {
            var res = new BooleanServiceResult();

            App.Settings.AddDeclarationBonnePratiques.Add(declaration);
            App.Settings.AddDeclarationBonnePratiques = App.Settings.AddDeclarationBonnePratiques;

            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                goto End;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationBonnePratique.AddDeclarationBonnePratiqueAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, declaration);
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            End:
            if (res.IsSuccess ?? false)
            {
                App.Settings.AddDeclarationBonnePratiques.Remove(declaration);
                App.Settings.AddDeclarationBonnePratiques = App.Settings.AddDeclarationBonnePratiques;
            }
            return res;
        }
        public static async Task<BooleanServiceResult> AddDeclarationRemorque(DeclarationSimplePlancher declaration)
        {
            var res = new BooleanServiceResult();

            App.Settings.AddDeclarationRemorques.Add(declaration);
            App.Settings.AddDeclarationRemorques = App.Settings.AddDeclarationRemorques;

            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                goto End;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationRemorque.AddDeclarationRemorqueAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, declaration);
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            End:
            if (res.IsSuccess ?? false)
            {
                App.Settings.AddDeclarationRemorques.Remove(declaration);
                App.Settings.AddDeclarationRemorques = App.Settings.AddDeclarationRemorques;
            }
            return res;
        }
        public static async Task<BooleanServiceResult> AddDeclarationControleReception(DeclarationControleReception declaration)
        {
            var res = new BooleanServiceResult();

            App.Settings.AddDeclarationControleReceptions.Add(declaration);
            App.Settings.AddDeclarationControleReceptions = App.Settings.AddDeclarationControleReceptions;

            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                goto End;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationControleReception.AddDeclarationControleReceptionAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, declaration);
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            End:
            if (res.IsSuccess ?? false)
            {
                App.Settings.AddDeclarationControleReceptions.Remove(declaration);
                App.Settings.AddDeclarationControleReceptions = App.Settings.AddDeclarationControleReceptions;
            }
            return res;
        }
        public static async Task<BooleanServiceResult> UpdateDeclarationDoublePlancherAsync(DeclarationDoublePlancher declaration, string statusId)
        {
            var res = new BooleanServiceResult();


            App.Settings.UpdateDeclarationDoublePlanchers.RemoveWhere(p => p.TractionId == declaration.TractionId);
            App.Settings.UpdateDeclarationDoublePlanchers.Add(declaration);
            App.Settings.UpdateDeclarationDoublePlanchers = App.Settings.UpdateDeclarationDoublePlanchers;

            if (!App.ServiceNetwork.IsConnected() || statusId == null)
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                goto End;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationDoublePlancher.UpdateDeclarationDoublePlancherAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, declaration, statusId);
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            End:
            if (res.IsSuccess ?? false)
            {
                App.Settings.UpdateDeclarationDoublePlanchers.Remove(declaration);
                App.Settings.UpdateDeclarationDoublePlanchers = App.Settings.UpdateDeclarationDoublePlanchers;
            }
            return res;
        }
        public static async Task<TractionListServiceResult> GetTractions()
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new TractionListServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationDoublePlancher.GetTractionsAsync(VersionTracking.CurrentVersion, App.Settings.User.Id);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            return res;
        }

        public static async Task<PictureServiceResult> UploadPicDP(string fileContent, string fileName, PictureType pictureType, string declarationId)
        {
            var res = new PictureServiceResult();
            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                return res;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationDoublePlancher.UploadPicDPAsync(VersionTracking.CurrentVersion, App.Settings.User.Id,(int) pictureType, declarationId, fileName, fileContent);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            return res;
        }
        public static async Task<PictureServiceResult> UploadPicSP(string fileContent, string fileName, PictureType pictureType, string declarationId)
        {
            var res = new PictureServiceResult();
            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                return res;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationRemorque.UploadPicSPAsync(VersionTracking.CurrentVersion, App.Settings.User.Id,(int) pictureType, declarationId, fileName, fileContent);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            return res;
        }
        public static async Task<PictureServiceResult> UploadPicCR(string fileContent, string fileName, PictureType pictureType, string declarationId)
        {
            var res = new PictureServiceResult();
            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                return res;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationControleReception.UploadPicCRAsync(VersionTracking.CurrentVersion, App.Settings.User.Id,(int) pictureType, declarationId, fileName, fileContent);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            return res;
        }
        
        public static async Task<PictureServiceResult> UploadPicBP(string fileContent, string fileName, PictureType pictureType, string declarationId)
        {
            var res = new PictureServiceResult();
            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                return res;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationBonnePratique.UploadPicBPAsync(VersionTracking.CurrentVersion, App.Settings.User.Id,(int) pictureType, declarationId, fileName, fileContent);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            return res;
        }
        
        public static async Task<PictureServiceResult> UploadPicNC(string fileContent, string fileName, PictureType pictureType, string declarationId)
        {
            var res = new PictureServiceResult();
            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                return res;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.DeclarationNonConformite.UploadPicNCAsync(VersionTracking.CurrentVersion, App.Settings.User.Id,(int) pictureType, declarationId, fileName, fileContent);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            return res;
        }
        
        public static async Task<PictureServiceResult> UploadProfilePic(string fileContent, string fileName)
        {
            var res = new PictureServiceResult();
            if (!App.ServiceNetwork.IsConnected())
            {
                res.ErrorMessage = "Vérifiez vos paramètres et réessayez ultérieurement";
                return res;
            }
            try
            {
                res = await GefcoCipQuaiWeb.Instance.Account.UploadProfilePicAsync(VersionTracking.CurrentVersion, App.Settings.User.Id, fileName, fileContent);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                res.ErrorMessage = ex1.ToString();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.ToString();
            }
            return res;
        }

        public static async Task<UserServiceResult> UpdateUserAsync(ApplicationUser user)
        {
            if (!App.ServiceNetwork.IsConnected())
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
                return null;
            }
            var res = new UserServiceResult();
            try
            {
                res = await GefcoCipQuaiWeb.Instance.Account.UpdateAsync(VersionTracking.CurrentVersion, user.Id, user.Email, user.FirstName, user.LastName, user.PhoneNumber ?? string.Empty);
                return res;
            }
            catch (HttpOperationException ex1)
            {
                //await App.ServiceDialog.ShowMessage("Vérifiez vos paramètres et réessayez ultérieurement", "Erreur");
            }
            catch (Exception)
            {
                //await App.ServiceDialog.ShowError(e, "Error in " + nameof(SetDevice), "OK", () => { });
            }
            return res;
        }
    }
}
