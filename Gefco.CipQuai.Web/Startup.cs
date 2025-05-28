using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gefco.CipQuai.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Gefco.CipQuai.Web.Startup))]
namespace Gefco.CipQuai.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration { };
                map.RunSignalR(hubConfiguration);
            });
            _timer = new Timer(TractionGenerator, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            _timer2 = new Timer(AutoClose, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        private static DateTime _lastUpdateCommande = default(DateTime);
        private static DateTime _lastCloseCommande = default(DateTime);
        private static Timer _timer;
        private static Timer _timer2;

        private void TractionGenerator(object state)
        {
            try
            {
                using (var dal = new Dal())
                {
                    if (_lastUpdateCommande != default(DateTime)) // Déja envoyé hier ou aujourd'hui
                        if (_lastUpdateCommande.Date == DateTime.UtcNow.Date) // Déja envoyé
                            return;
                    _lastUpdateCommande = DateTime.UtcNow;
                    SimpleLogger.GetOne().Info($"{nameof(TractionGenerator)}: Executing with ts: {_lastUpdateCommande}");
                    dal.GetTractions(null);
                    SimpleLogger.GetOne().Info($"{nameof(TractionGenerator)}: Executed");
                }
            }
            catch (Exception e)
            {
                SimpleLogger.GetOne().Error(e);
                Console.WriteLine(e);
            }
        }
        private async void AutoClose(object state)
        {
             try
             {
                 using (var ctx = new ApplicationDbContext())
                 {
                     if (_lastCloseCommande != default(DateTime))             // Déja envoyé hier ou aujourd'hui
                         if (_lastCloseCommande.Date == DateTime.UtcNow.Date) // Déja envoyé
                             return;
                     _lastCloseCommande = DateTime.UtcNow;
                     SimpleLogger.GetOne().Info($"{nameof(AutoClose)}: Executing with ts: {_lastCloseCommande}");
                     var dr = ctx.DeclarationDoublePlanchers.Include(p => p.Pictures).Include(p => p.CreatedBy).Include(p => p.Agence.Region).Where(p => new[] { "F516E0DD-830E-4695-986C-5AF2FCB7E8C9", "9706DF5F-85E5-435D-BF18-3C0502F810CF", "62C4E8FD-F31A-4A59-B24D-6669D5620161" }.Contains(p.CurrentStatus_Id) && ((p.IsDPUsed && p.Pictures.Count >= 2) || (!p.IsDPUsed && p.Pictures.Count >= 1)) && p.Agence.Region.AutoCloseDP).ToList().Where(p => p.CreationDate <= DateTime.UtcNow.AddDays(-1));
                     foreach (var declaration in dr)
                     {
                         if (!declaration.CompletionDate.HasValue)
                             declaration.CompletionDate = DateTime.UtcNow;
                         declaration.CurrentStatus_Id = "460BF74D-9D95-4D13-A8FC-E76CA933AD1A";
                         ctx.DeclarationDoublePlanchers.AddOrUpdate(declaration);
                         ctx.DeclarationRemorqueStatuses.Add(new DeclarationRemorqueStatus()
                                                             {
                                                                 Id = Guid.NewGuid().ToString(),
                                                                 CreatedBy = declaration.CreatedBy,
                                                                 IsDeleted = false,
                                                                 CreationDate = DateTime.UtcNow,
                                                                 DeclarationRemorque = declaration,
                                                                 RemorqueStatus_Id = "460BF74D-9D95-4D13-A8FC-E76CA933AD1A"
                                                             });
                     }
                     await ctx.SaveChangesAsync();
                     SimpleLogger.GetOne().Info($"{nameof(AutoClose)}: Executed");
                 }
             }
             catch (Exception e)
             {
                 SimpleLogger.GetOne().Error(nameof(AutoClose), e);
             }

        }
    }
}
