using Gefco.CipQuai.Web.Hubs;
using Gefco.CipQuai.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Gefco.CipQuai.Web
{
    public class Dal : IDisposable
    {
        public void Dispose()
        {
            _ctx.Dispose();
        }

        public Dal()
        {
            _ctx = new ApplicationDbContext();
        }

        private ApplicationDbContext _ctx;

        #region Agence

        public Agence FindAgence(string id)
        {
            return _ctx.Agences.Find(id);
        }
        public IQueryable<Agence> GetAgences(string userId)
        {
            var items = _ctx.Agences.Where(p => !p.IsDeleted && (p.UserCreated == false || p.CreatedBy_Id == userId));

            return items;
        }
        public IQueryable<Agence> GetAgencesCR(string userId)
        {
            var items = _ctx.Agences.Where(p => !p.IsDeleted && (p.UserCreated == false || p.CreatedBy_Id == userId) && p.IsUnderWatch);

            return items;
        }
        public void InsertAgence(Agence agence)
        {
            _ctx.Agences.AddOrUpdate(agence);
            _ctx.SaveChanges();
        }

        #endregion

        #region AgenceType

        public IQueryable<AgenceType> GetAgenceTypes()
        {
            var AgenceTypes = _ctx.AgenceTypes;

            return AgenceTypes;
        }

        #endregion

        #region User

        public ApplicationUser FindUser(string id)
        {
            return _ctx.Users.Find(id);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _ctx.Users.AddOrUpdate(user);
            _ctx.SaveChanges();
        }
        public IQueryable<ApplicationUser> GetUsers()
        {
            return _ctx.Users;
        }

        #endregion

        #region Traction

        public Traction FindTraction(string id)
        {
            return _ctx.Tractions.Find(id);
        }

        public void InsertTraction(Traction traction)
        {
            _ctx.Tractions.AddOrUpdate(traction);
            _ctx.SaveChanges();
        }

        public static object LockObj = new object();
        //public IEnumerable<Traction> GetTractions(ApplicationUser user)
        //{
        //    lock (LockObj)
        //    {
        //        List<TractionDefinition> definitions;
        //        List<Traction> tractions;
        //        if (user?.MobileUserAgence == null)
        //        {
        //            definitions = _ctx.TractionDefinitions.Where(p => !p.IsDeleted).ToList();
        //            tractions = _ctx.Tractions.Where(p => !p.IsDeleted && p.DueDate == DateTime.Today).ToList();
        //        }
        //        else
        //        {
        //            definitions = _ctx.TractionDefinitions.Where(p => !p.IsDeleted && user.MobileUserAgence.Id == p.AgenceDepart.Id).ToList();
        //            tractions = _ctx.Tractions.Where(p => !p.IsDeleted && user.MobileUserAgence.Id == p.AgenceDepart.Id && p.DueDate == DateTime.Today).ToList();
        //        }
        //        IEnumerable<TractionDefinition> undeclaredTractions = definitions.Where(definition => tractions.All(traction => !(traction.TractionDefinition?.Id == definition.Id && traction.DueDate == DateTime.Today))).ToList();
        //        var today = DateTime.Today.DayOfWeek;
        //        foreach (var definition in undeclaredTractions)
        //        {
        //            if (definition.DaysOfWeek.Contains((Days) today))
        //            {
        //                try
        //                {
        //                    var traction = new Traction()
        //                    {
        //                        AgenceDepart = definition.AgenceDepart,
        //                        AgenceArrivee = definition.AgenceArrivee,
        //                        CreatedBy = user,
        //                        TractionDefinition = definition,
        //                        Id = Guid.NewGuid().ToString(),
        //                        Name = definition.Name,
        //                        IsDeleted = false,
        //                        IsCreated = false,
        //                        CreationDate = DateTime.UtcNow,
        //                        DueDate = DateTime.Today
        //                    };
        //                    _ctx.Tractions.Add(traction);
        //                    _ctx.SaveChanges();
        //                    tractions.Add(traction);
        //                }
        //                catch (Exception e)
        //                {
        //                    SimpleLogger.GetOne().Error(nameof(Dal) + "." + nameof(GetTractions));
        //                    SimpleLogger.GetOne().Error(e);
        //                    Console.WriteLine(e);
        //                }
        //            }
        //        }
        //        return tractions.Where(p => !p.IsCreated);
        //    }
        //}
        public IEnumerable<Traction> GetTractions(ApplicationUser user)
        {
            lock (LockObj)
            {
                List<TractionDefinition> definitions;
                List<Traction> tractions = new List<Traction>();
                var endDate = DateTime.Today;

                for (int i = 0; i < 7; i++)
                {
                    var date = endDate.AddDays(-i);
                    if (user?.MobileUserAgence == null)
                    {
                        definitions = _ctx.TractionDefinitions.Where(p => !p.IsDeleted).ToList();
                        tractions.AddRange(_ctx.Tractions.Where(p => p.DueDate == date).ToList());
                    }
                    else
                    {
                        definitions = _ctx.TractionDefinitions.Where(p => !p.IsDeleted && user.MobileUserAgence.Id == p.AgenceDepart.Id).ToList();
                        tractions.AddRange(_ctx.Tractions.Where(p => user.MobileUserAgence.Id == p.AgenceDepart.Id && p.DueDate == date).ToList());
                    }
                    IEnumerable<TractionDefinition> undeclaredTractions = definitions.Where(definition => tractions.All(traction => !(traction.TractionDefinition?.Id == definition.Id && traction.DueDate == date))).ToList();
                    var today = date.DayOfWeek;
                    foreach (var definition in undeclaredTractions)
                    {
                        if (definition.DaysOfWeek.Contains((Days)today))
                        {
                            try
                            {
                                var traction = new Traction()
                                {
                                    AgenceDepart = definition.AgenceDepart,
                                    AgenceArrivee = definition.AgenceArrivee,
                                    CreatedBy = user,
                                    TractionDefinition = definition,
                                    Id = Guid.NewGuid().ToString(),
                                    Name = definition.Name,
                                    IsDeleted = false,
                                    IsCreated = false,
                                    CreationDate = DateTime.UtcNow,
                                    DueDate = date
                                };
                                _ctx.Tractions.Add(traction);
                                _ctx.SaveChanges();
                                tractions.Add(traction);
                            }
                            catch (Exception e)
                            {
                                SimpleLogger.GetOne().Error(nameof(Dal) + "." + nameof(GetTractions));
                                SimpleLogger.GetOne().Error(e);
                                Console.WriteLine(e);
                            }
                        }
                    }
                }

                return tractions.Where(p => !p.IsCreated && !p.IsDeleted && !p.IsCancelled);
            }
        }

        public IQueryable<Traction> GetTractions()
        {
            return _ctx.Tractions;
        }

        #endregion

        #region TractionDefinition

        public IQueryable<TractionDefinition> GetTractionDefinitions()
        {
            return _ctx.TractionDefinitions;
        }


        #endregion

        #region Picture

        public Picture FindPicture(string id)
        {
            return _ctx.Pictures.Find(id);
        }
        public Picture FindTempPicture(string id)
        {
            return new Picture(_ctx.TempPictures.Find(id));
        }

        public IQueryable<Picture> GetPictures()
        {
            return _ctx.Pictures;
        }
        public void InsertPicture(Picture picture)
        {
            var tempPicture = new TempPicture(picture);
            _ctx.TempPictures.AddOrUpdate(tempPicture);
            _ctx.SaveChanges();
            new Task(async () =>
                     {
                         try
                         {
                             await Task.Delay(TimeSpan.FromSeconds(5));
                             using (var ctx = new ApplicationDbContext())
                             {
                                 var cmd = ctx.Database.Connection.CreateCommand();
                                 cmd.CommandTimeout = Int32.MaxValue;
                                 await ctx.Database.Connection.OpenAsync();
                                 cmd.CommandText = @"INSERT INTO [dbo].[Pictures]
           ([Id]
           ,[PicturePath]
           ,[PictureType]
           ,[IsDeleted]
           ,[CreationDate]
           ,[CreatedBy_Id]
           ,[DeclarationRemorque_Id]
           ,[DeclarationBonnePratique_Id]
           ,[DeclarationNonConformite_Id]
           ,[ApplicationUser_Id]
           ,[DeclarationControleReception_Id]
           ,[DeclarationRemorque_Id1])
     Select Id, PicturePath, PictureType, IsDeleted, CreationDate, CreatedBy_Id, DeclarationRemorque_Id, DeclarationBonnePratique_Id, DeclarationNonConformite_Id, ApplicationUser_Id, DeclarationControleReception_Id, DeclarationRemorque_Id FROM TempPictures
	 WHERE Id NOT IN (SELECT Id FROM Pictures)";
                                 await cmd.ExecuteNonQueryAsync();
                                 cmd.Dispose();
                                 cmd = ctx.Database.Connection.CreateCommand();
                                 cmd.CommandText = @"DELETE FROM TempPictures WHERE Id IN (SELECT Id FROM Pictures)";
                                 await cmd.ExecuteNonQueryAsync();
                             }
                         }
                         catch (Exception e)
                         {
                             SimpleLogger.GetOne().Error(nameof(InsertPicture), e);
                         }
                     }).Start();
        }

        #endregion

        #region DeclarationDoublePlancher

        public DeclarationDoublePlancher FindDeclarationDoublePlancher(string id)
        {
            return _ctx.DeclarationDoublePlanchers.Include(p => p.Pictures).Include(p => p.MotifDps).SingleOrDefault(p => p.Id == id);
        }
        public IQueryable<DeclarationDoublePlancher> GetActiveDeclarationDoublePlanchers(ApplicationUser user)
        {
            var declarationDoublePlanchers = from declaration in _ctx.DeclarationDoublePlanchers
                                             let isMine = declaration.CreatedBy.Id == user.Id
                                             let isMineActive = isMine && RemorqueStatus.OwnerActiveStatuses.Contains(declaration.CurrentStatus.Name)
                                             let isOthersActive = !isMine && RemorqueStatus.OthersActiveStatuses.Contains(declaration.CurrentStatus.Name)
                                             where (isMineActive || isOthersActive) && user.MobileUserAgence.Id == declaration.Traction.AgenceDepart.Id
                                             select declaration;
            return declarationDoublePlanchers;
        }
        public void InsertDeclarationDoublePlancher(DeclarationDoublePlancher declaration)
        {
            _ctx.DeclarationDoublePlanchers.AddOrUpdate(declaration);
            _ctx.DeclarationRemorqueStatuses.Add(new DeclarationRemorqueStatus()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = declaration.CreatedBy,
                IsDeleted = false,
                CreationDate = DateTime.UtcNow,
                DeclarationRemorque = declaration,
                RemorqueStatus = declaration.CurrentStatus
            });
            _ctx.SaveChanges();
        }
        public void UpdateDeclarationDoublePlancher(DeclarationDoublePlancher declaration, ApplicationUser context)
        {
            _ctx.DeclarationDoublePlanchers.AddOrUpdate(declaration);
            _ctx.DeclarationRemorqueStatuses.Add(new DeclarationRemorqueStatus()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = context,
                IsDeleted = false,
                CreationDate = DateTime.UtcNow,
                DeclarationRemorque = declaration,
                RemorqueStatus = declaration.CurrentStatus
            });
            _ctx.SaveChanges();
            new TaskFactory().StartNew(() => EventBrokerHub.Instance.InsertDeclarationDpAsync(declaration.Id));
        }
        public IQueryable<DeclarationDoublePlancher> GetDeclarationDoublePlanchers()
        {
            var items = _ctx.DeclarationDoublePlanchers;
            return items;
        }

        #endregion
        #region DeclarationRemorque

        public DeclarationSimplePlancher FindDeclarationSimplePlancher(string id)
        {
            return _ctx.DeclarationSimplePlanchers.Find(id);
        }
        public void InsertDeclarationRemorque(DeclarationSimplePlancher declaration)
        {
            _ctx.DeclarationSimplePlanchers.AddOrUpdate(declaration);
            _ctx.DeclarationRemorqueStatuses.Add(new DeclarationRemorqueStatus()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedBy = declaration.CreatedBy,
                IsDeleted = false,
                CreationDate = DateTime.UtcNow,
                DeclarationRemorque = declaration,
                RemorqueStatus = declaration.CurrentStatus
            });
            _ctx.SaveChanges();
            new TaskFactory().StartNew(() => EventBrokerHub.Instance.InsertDeclarationSpAsync(declaration.Id));
        }
        public IQueryable<DeclarationSimplePlancher> GetDeclarationRemorques()
        {
            var items = _ctx.DeclarationSimplePlanchers;
            return items;
        }

        #endregion
        #region DeclarationControleReception

        public DeclarationControleReception FindDeclarationControleReception(string id)
        {
            return _ctx.DeclarationControleReceptions.Find(id);
        }
        public void InsertDeclarationControleReception(DeclarationControleReception declaration)
        {
            _ctx.DeclarationControleReceptions.AddOrUpdate(declaration);
            //_ctx.DeclarationControleReceptionStatuses.Add(new DeclarationControleReceptionStatus()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    CreatedBy = declaration.CreatedBy,
            //    IsDeleted = false,
            //    CreationDate = DateTime.UtcNow,
            //    DeclarationControleReception = declaration,
            //    ControleReceptionStatus = declaration.CurrentStatus
            //});
            _ctx.SaveChanges();
            new TaskFactory().StartNew(() => EventBrokerHub.Instance.InsertDeclarationCrAsync(declaration.Id));
        }
        public IQueryable<DeclarationControleReception> GetDeclarationControleReceptions()
        {
            var items = _ctx.DeclarationControleReceptions;
            return items;
        }

        #endregion

        #region DeclarationNonConformite

        public DeclarationNonConformite FindDeclarationNonConformite(string id)
        {
            return _ctx.DeclarationNonConformites.Find(id);
        }
        public void InsertDeclarationNonConformite(DeclarationNonConformite declaration)
        {
            _ctx.DeclarationNonConformites.Add(declaration);
            _ctx.SaveChanges();
            new TaskFactory().StartNew(() => EventBrokerHub.Instance.InsertDeclarationNcAsync(declaration.Id));
        }
        public IQueryable<DeclarationNonConformite> GetDeclarationNonConformites()
        {
            var items = _ctx.DeclarationNonConformites;
            return items;
        }

        #endregion

        #region DeclarationBonnePratique

        public DeclarationBonnePratique FindDeclarationBonnePratique(string id)
        {
            return _ctx.DeclarationBonnePratiques.Find(id);
        }
        public void InsertDeclarationBonnePratique(DeclarationBonnePratique declaration)
        {
            _ctx.DeclarationBonnePratiques.Add(declaration);
            _ctx.SaveChanges();
            new TaskFactory().StartNew(() => EventBrokerHub.Instance.InsertDeclarationBpAsync(declaration.Id));
        }

        #endregion

        #region Device

        public IQueryable<Device> GetDevices()
        {
            return _ctx.Devices;
        }
        public void InsertDevice(Device device)
        {
            _ctx.Devices.AddOrUpdate(device);
            _ctx.SaveChanges();
        }
        public void UpdateDevice(Device device)
        {
            _ctx.Devices.AddOrUpdate(device);
            _ctx.SaveChanges();
        }

        #endregion

        #region Configuration

        public IQueryable<Models.Configuration> GetConfigurations()
        {
            var configurations = _ctx.Configurations.Where(p => !p.IsDeleted);

            return configurations;
        }

        #endregion

        #region MotifDP

        public IQueryable<MotifDP> GetMotifDPs()
        {
            var items = _ctx.MotifDPs.Where(p => !p.IsDeleted);

            return items;
        }

        #endregion

        #region MotifNC

        public IQueryable<MotifNC> GetMotifNCs()
        {
            var items = _ctx.MotifNCs.Where(p => !p.IsDeleted);

            return items;
        }


        #endregion

        #region Resource

        public IQueryable<Resource> GetResources(ApplicationUser user, DateTime lastUpdate)
        {
            if (_ctx.Resources.Any(p => p.LastUpdate > lastUpdate))
                return _ctx.Resources.Where(p => !p.IsDeleted && p.IsForAll || p.Users.Any(q => q.Id == user.Id));
            return new List<Resource>().AsQueryable();
        }


        #endregion

        #region RemorqueStatus

        public IQueryable<RemorqueStatus> GetRemorqueStatuses()
        {
            return _ctx.RemorqueStatuses.Where(p => !p.IsDeleted);
        }


        #endregion
        #region DéclarationNCStatus

        public IQueryable<DeclarationNcStatus> GetDeclarationNcStatuses()
        {
            return _ctx.DeclarationNcStatuses.Where(p => !p.IsDeleted);
        }


        #endregion


        public ApplicationDbContext ObjectContext
        {
            get { return _ctx; }
        }

        #region Region

        public IQueryable<Region> GetRegions()
        {
            var items = _ctx.Regions.Where(p => !p.IsDeleted);
            return items;
        }

        #endregion

        public Models.Configuration FindConfiguration(string key)
        {
            return _ctx.Configurations.SingleOrDefault(p => p.Name == key);
        }

        public EmailTemplate FindEmailTemplate(string key)
        {
            return _ctx.EmailTemplates.SingleOrDefault(p => p.Name == key);
        }
    }
}