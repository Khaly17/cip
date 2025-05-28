using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gefco.CipQuai.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Gefco.CipQuai.Web.Hubs
{
    [HubName("EventBroker")]
    public class EventBrokerHub : Hub<IEngineCallback>
    {
        private static EventBrokerHub _instance;
        public static EventBrokerHub Instance => _instance;

        public async Task ClientLoggedInAsync(string clientToken)
        {
            Console.WriteLine(nameof(ClientLoggedInAsync));
            SimpleLogger.GetOne().Debug($"EventBroker: {nameof(ClientLoggedInAsync)} [{clientToken}]");
            try
            {
                await UpdateClientAsync(clientToken);
            }
            catch (Exception ex)
            {
                SimpleLogger.GetOne().Error($"EventBroker: Exception in {nameof(ClientLoggedInAsync)}()", ex);
            }

        }

        private enum Roles
        {
            SuperAdmin,
            Admin,
            NationalUsers,
            RegionalUsers,
            AgenceUsers,
        }

        private async Task UpdateClientAsync(string clientToken)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var user = ctx.Users.Find(clientToken);
                if (user == null)
                    return;

                var userRoles = user.Roles.Select(p => p.RoleId).ToList();
                var roles = ctx.Roles.Where(p => userRoles.Contains(p.Id));
                Roles upperRole = Roles.AgenceUsers;

                foreach (var role in roles)
                {
                    if (role.Name == "National Users")
                        upperRole = Min(Roles.NationalUsers, upperRole);
                    else if (role.Name == "Regional Users")
                        upperRole = Min(Roles.RegionalUsers, upperRole);
                    else if (role.Name == "Agence Users")
                        upperRole = Min(Roles.AgenceUsers, upperRole);
                    else if (role.Name == "Super Admin")
                        upperRole = Min(Roles.SuperAdmin, upperRole);
                    else if (role.Name == "Admin")
                        upperRole = Min(Roles.SuperAdmin, upperRole);
                }
                switch (upperRole)
                {
                    case Roles.Admin:
                        await Groups.Add(Context.ConnectionId, "Admin");
                        break;
                    case Roles.SuperAdmin:
                        await Groups.Add(Context.ConnectionId, "Super Admin");
                        break;
                    case Roles.NationalUsers:
                        await Groups.Add(Context.ConnectionId, "National Users");
                        break;
                    case Roles.RegionalUsers:
                    {
                        await Groups.Add(Context.ConnectionId, "Regional Users");
                    }
                        break;
                    case Roles.AgenceUsers:
                        await Groups.Add(Context.ConnectionId, "Agence Users");
                        break;
                }
            }
        }
        public static T Min<T>(T a, T b) where T : IComparable
        {
            return a.CompareTo(b) <= 0 ? a : b;
        }
        static EventBrokerHub()
        {
        }

        public EventBrokerHub()
        {
            if (_instance == null)
                _instance = this;
        }

        public async Task InsertDeclarationDpAsync(string id)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationDoublePlancher(id);
                if (declaration?.Traction == null)
                    return;
                var agenceId = declaration.Traction.AgenceDepart.Id;
                var regionId = declaration.Traction.AgenceDepart.Region.Id;
                await Clients.Groups(new List<string>(){ "Super Admin", "Admin", "National Users" }).DeclarationUpdateAsync("DP", "National", "");
                await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users" }).DeclarationUpdateAsync("DP", "Region", regionId);
                await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users", "Agence Users"}).DeclarationUpdateAsync("DP", "Agence", agenceId);
                
            }
        }
        public async Task InsertDeclarationSpAsync(string id)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationSimplePlancher(id);
                var agenceId = declaration.AgenceId;
                var regionId = declaration.Agence.Region.Id;
                await Clients.Groups(new List<string>(){ "Super Admin", "Admin", "National Users" }).DeclarationUpdateAsync("SP", "National", "");
                await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users" }).DeclarationUpdateAsync("SP", "Region", regionId);
                await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users", "Agence Users"}).DeclarationUpdateAsync("SP", "Agence", agenceId);
                
            }
        }
        public async Task InsertDeclarationCrAsync(string id)
        {
            using (var dal = new Dal())
            {
                DeclarationControleReception declaration = dal.FindDeclarationControleReception(id);
                var agenceId = declaration.AgenceId;
                var regionId = declaration.Agence.Region.Id;
                await Clients.Groups(new List<string>(){ "Super Admin", "Admin", "National Users" }).DeclarationUpdateAsync("CR", "National", "");
                await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users" }).DeclarationUpdateAsync("CR", "Region", regionId);
                await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users", "Agence Users"}).DeclarationUpdateAsync("CR", "Agence", agenceId);
                
            }
        }
        public async Task InsertDeclarationBpAsync(string id)
        {
            using (var dal = new Dal())
            {
                await Clients.Groups(new List<string>(){ "Super Admin", "Admin", "National Users" }).DeclarationUpdateAsync("BP", "National", "");

                var declaration = dal.FindDeclarationBonnePratique(id);
                var agenceId = declaration.AgenceId;
                if (agenceId != null)
                {
                    var agence = dal.FindAgence(agenceId);
                    if (agence.Region_Id != null)
                        await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users" }).DeclarationUpdateAsync("BP", "Region", agence.Region_Id);
                    await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users", "Agence Users" }).DeclarationUpdateAsync("BP", "Agence", agenceId);
                }
            }
        }
        public async Task InsertDeclarationNcAsync(string id)
        {
            using (var dal = new Dal())
            {
                var declaration = dal.FindDeclarationNonConformite(id);
                await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users" }).DeclarationUpdateAsync("NC", "National", "");
                var agenceId1 = declaration.AgenceConcernée_Id;
                var agenceId2 = declaration.AgenceId;
                if (agenceId1 != null)
                {
                    var agence = dal.FindAgence(agenceId1);
                    if (agence.Region_Id != null)
                        await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users" }).DeclarationUpdateAsync("NC", "Region", agence.Region_Id);
                    await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users", "Agence Users" }).DeclarationUpdateAsync("NC", "Agence", agenceId1);
                }
                if (agenceId2 != null)
                {
                    var agence = dal.FindAgence(agenceId2);
                    if (agence.Region_Id != null)
                        await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users" }).DeclarationUpdateAsync("NC", "Region", agence.Region_Id);
                    await Clients.Groups(new List<string>() { "Super Admin", "Admin", "National Users", "Regional Users", "Agence Users" }).DeclarationUpdateAsync("NC", "Agence", agenceId2);
                }

            }
        }
    }
    public interface IEngineCallback
    {
        Task DeclarationUpdateAsync(string declarationType, string eventType, string id);
    }

}