using Gefco.CipQuai.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Gefco.CipQuai.Web.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Gefco.CipQuai.Web.Models.ApplicationDbContext>
    {
        private IdentityRole _sAdminRole;
        private IdentityRole _nationalRole;
        private static readonly Region _regionIDF = new Region() { Id = "9ED6CB25-FFC9-4C7E-9C03-6846F86275E0", CreationDate = DateTime.UtcNow, Name = "NORD/IDF" };
        private static readonly Region _regionGrandOuest = new Region() { Id = "A75E2B9B-FDE3-4CDD-9678-23732722E0E5", CreationDate = DateTime.UtcNow, Name = "Grand Ouest" };
        private static readonly Region _regionSudEst = new Region() { Id = "D6B8196C-6CA4-4356-BCB8-7422D322ECF1", CreationDate = DateTime.UtcNow, Name = "Sud Est" };
        private static readonly Region _regionGrandEst = new Region() { Id = "FE46E982-DCC5-4F78-9F9A-B7E6331C95C1", CreationDate = DateTime.UtcNow, Name = "Grand Est" };

        private static readonly AgenceType _agenceTypeGefco = new AgenceType() { Key = 1, Value = "Gefco France", Description = "Gefco France" };
        private static readonly AgenceType _agenceTypeInternational = new AgenceType() { Key = 2, Value = "International", Description = "International" };
        private static readonly AgenceType _agenceTypeConfrères = new AgenceType() { Key = 3, Value = "Confrères", Description = "Confrères" };

        private readonly Agence _agence70SDTL = new Agence { Id = "3664A2EC-17CD-46C2-9A9B-F158E25E6DC1", Name = "70SDTL", IsEnd = true, AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agenceAmiens = new Agence { Id = "2DC2A55F-5D0C-4A93-9E18-D58E54073BF3", Name = "Amiens", Region_Id = _regionIDF.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceAvignon = new Agence { Id = "795048EE-EFF6-4774-8B8E-14B614FC2EE2", Name = "Avignon", Region_Id = _regionSudEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceBordeaux = new Agence { Id = "17C592F1-2CDD-484D-959E-9632076F4BDC", Name = "Bordeaux", Region_Id = _regionGrandOuest.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceBoucBelAir = new Agence { Id = "5667C5A2-0B11-4DFA-95A3-05F03C4C313A", Name = "Bouc Bel Air", Region_Id = _regionSudEst.Id, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceBruxelles = new Agence { Id = "53958AF0-DE87-4EBD-A61A-35EE6A006475", Name = "Bruxelles", IsEnd = true, AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceCharleville = new Agence { Id = "DA54724F-1FD4-46E2-AAC3-B9DD064BFE80", Name = "Charleville", Region_Id = _regionGrandEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceChateauroux = new Agence { Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", Name = "Chateauroux", Region_Id = _regionGrandOuest.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceClermontFerrand = new Agence { Id = "05DA48FC-E932-43FB-A48C-BC8505ADAF7E", Name = "Clermont Ferrand", Region_Id = _regionSudEst.Id, IsStart = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceCornier = new Agence { Id = "D70F3C75-276E-4489-ABF9-C9D558415C33", Name = "Cornier", Region_Id = _regionSudEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceCoventry = new Agence { Id = "7C07EFD5-F120-414F-AE42-1040DE26AE2A", Name = "Coventry", IsEnd = true, AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceDijon = new Agence { Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", Name = "Dijon", Region_Id = _regionGrandEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceDijonLyon = new Agence { Id = "2619ECD7-1E59-4294-B61B-B4FF69044C4F", Name = "Dijon-Lyon", Region_Id = _regionGrandEst.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceGennevilliers = new Agence { Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", Name = "Gennevilliers", Region_Id = _regionIDF.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceGennevilliersOrléans = new Agence { Id = "9D988267-5915-4F20-A4BD-8012EADDAB3E", Name = "Gennevilliers-Orléans", Region_Id = _regionIDF.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceGrenoble = new Agence { Id = "20A82D4B-9F24-45C0-8532-BBC4CE5EF57A", Name = "Grenoble", Region_Id = _regionSudEst.Id, IsStart = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceLille = new Agence { Id = "071406AD-A58E-4B8B-A158-89F5953146F9", Name = "Lille", Region_Id = _regionIDF.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceLyon = new Agence { Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", Name = "Lyon", Region_Id = _regionSudEst.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceMarseille = new Agence { Id = "49E46CAA-2F0B-48D4-98CE-005CEF4EA711", Name = "Marseille", Region_Id = _regionSudEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceMetz = new Agence { Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", Name = "Metz", Region_Id = _regionGrandEst.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceMoissy = new Agence { Id = "5FD84692-0823-4470-9AFA-A85A36A01B76", Name = "Moissy", Region_Id = _regionIDF.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceMontpellier = new Agence { Id = "187E54F5-ABA4-4773-97BA-1BADD67167DC", Name = "Montpellier", Region_Id = _regionSudEst.Id, IsStart = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceMulhouse = new Agence { Id = "BF81D7C8-BC56-4987-8E63-02EEDCE40273", Name = "Mulhouse", Region_Id = _regionGrandEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceNantes = new Agence { Id = "766E38D2-176E-42C6-9E73-4D45BD6E7918", Name = "Nantes", Region_Id = _regionGrandOuest.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceNice = new Agence { Id = "FFBA7E8B-07EE-494F-AC76-FF3AB4A1E8AD", Name = "Nice", Region_Id = _regionSudEst.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceOosterhout = new Agence { Id = "4DC5F8C6-57DE-4E1C-8308-485AF9CAC568", Name = "Oosterhout", IsEnd = true, AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceOrleans = new Agence { Id = "729BB666-D101-409C-9649-57F193629AC8", Name = "Orleans", Region_Id = _regionIDF.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceQuimper = new Agence { Id = "F68711EF-3DAD-42F9-B431-EDB01D3FE256", Name = "Quimper", Region_Id = _regionGrandOuest.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceReims = new Agence { Id = "772CEED5-7EA9-4A84-8E2D-47D0C98F52CF", Name = "Reims", Region_Id = _regionGrandEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceRennes = new Agence { Id = "DC1FC53C-5FC2-49AC-AC29-9F313712C704", Name = "Rennes", Region_Id = _regionGrandOuest.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceRouen = new Agence { Id = "7631E505-0ACC-4A21-A71D-E33AFAEAAF37", Name = "Rouen", Region_Id = _regionIDF.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceStrasbourg = new Agence { Id = "4ABB9048-615A-4782-A8F4-0A4D0B93815A", Name = "Strasbourg", Region_Id = _regionGrandEst.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceToulouse = new Agence { Id = "00D4B929-6658-4CD3-83CD-997CF84E5D18", Name = "Toulouse", Region_Id = _regionGrandOuest.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceTroyes = new Agence { Id = "B1801992-71DD-46DD-9E96-5B11E7F53682", Name = "Troyes", Region_Id = _regionGrandEst.Id, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceValenciennes = new Agence { Id = "064B3930-8B10-4A09-B92A-172876B188D0", Name = "Valenciennes", Region_Id = _regionIDF.Id, IsStart = true, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceValenciennesOuLille = new Agence { Id = "AF4E93F8-30F1-4FCC-97B6-2A93D78054E8", Name = "Valenciennes/Lille", Region_Id = _regionIDF.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceValenciennesAmiens = new Agence { Id = "A116EB37-14F5-4A51-9ADF-0412BB650DA6", Name = "Valenciennes-Amiens", Region_Id = _regionIDF.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceValenciennesChâteauroux = new Agence { Id = "A21C5F17-1B13-48DA-84B0-749CCF3007F2", Name = "Valenciennes-Châteauroux", Region_Id = _regionIDF.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceValenciennesLille = new Agence { Id = "3B883446-9B94-4FAA-AA87-28A2A42B3C99", Name = "Valenciennes-Lille", Region_Id = _regionIDF.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceValenciennesStrasbourg = new Agence { Id = "8E865411-6B05-4374-8DAC-B68F745E69E5", Name = "Valenciennes-Strasbourg", Region_Id = _regionIDF.Id, IsEnd = true, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceVenissieux = new Agence { Id = "C5BF9FFC-AF39-4F3F-AA5A-66D3CB075FF3", Name = "Venissieux", Region_Id = _regionSudEst.Id, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceVesoul = new Agence { Id = "D20F0248-CEF6-45F8-8E4D-ABB623EF4337", Name = "Vesoul", Region_Id = _regionGrandEst.Id, AgenceType_Id = _agenceTypeGefco.Key };

        private readonly Agence _agenceOyonnax = new Agence { Id = Guid.NewGuid().ToString(), Name = "Oyonnax", AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agencePerpignan = new Agence { Id = Guid.NewGuid().ToString(), Name = "Perpignan", Region_Id = _regionSudEst.Id, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceSaintEtienne = new Agence { Id = Guid.NewGuid().ToString(), Name = "Saint-Etienne", Region_Id = _regionSudEst.Id, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceStJeanDeLuz = new Agence { Id = Guid.NewGuid().ToString(), Name = "St Jean de Luz", Region_Id = _regionGrandOuest.Id, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceTarbes = new Agence { Id = Guid.NewGuid().ToString(), Name = "Tarbes", Region_Id = _regionGrandOuest.Id, AgenceType_Id = _agenceTypeGefco.Key };
        private readonly Agence _agenceTours = new Agence { Id = Guid.NewGuid().ToString(), Name = "Tours", Region_Id = _regionGrandOuest.Id, AgenceType_Id = _agenceTypeGefco.Key };

        private readonly Agence _agenceBarcelone = new Agence { Id = Guid.NewGuid().ToString(), Name = "Barcelone", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceBerlin = new Agence { Id = Guid.NewGuid().ToString(), Name = "Berlin", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceDartford = new Agence { Id = Guid.NewGuid().ToString(), Name = "Dartford", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceDelemont = new Agence { Id = Guid.NewGuid().ToString(), Name = "Delemont", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceDourges = new Agence { Id = Guid.NewGuid().ToString(), Name = "Dourges", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceFrankfort = new Agence { Id = Guid.NewGuid().ToString(), Name = "Frankfort", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceFrankfurt = new Agence { Id = Guid.NewGuid().ToString(), Name = "Frankfurt", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceGbrnetwo = new Agence { Id = Guid.NewGuid().ToString(), Name = "Gbrnetwo", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceGeneve = new Agence { Id = Guid.NewGuid().ToString(), Name = "Geneve", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceKatowice = new Agence { Id = Guid.NewGuid().ToString(), Name = "Katowice", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceMadrid = new Agence { Id = Guid.NewGuid().ToString(), Name = "Madrid", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceManchester = new Agence { Id = Guid.NewGuid().ToString(), Name = "Manchester", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceMilano = new Agence { Id = Guid.NewGuid().ToString(), Name = "Milano", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceMiranda = new Agence { Id = Guid.NewGuid().ToString(), Name = "Miranda", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agencePadova = new Agence { Id = Guid.NewGuid().ToString(), Name = "Padova", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agencePorto = new Agence { Id = Guid.NewGuid().ToString(), Name = "Porto", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceTorino = new Agence { Id = Guid.NewGuid().ToString(), Name = "Torino", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceValencia = new Agence { Id = Guid.NewGuid().ToString(), Name = "Valencia", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceVigoPcvepr = new Agence { Id = Guid.NewGuid().ToString(), Name = "Vigo-Pcvepr", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceWien = new Agence { Id = Guid.NewGuid().ToString(), Name = "Wien", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceWroclaw = new Agence { Id = Guid.NewGuid().ToString(), Name = "Wroclaw", AgenceType_Id = _agenceTypeInternational.Key };
        private readonly Agence _agenceWuppertal = new Agence { Id = Guid.NewGuid().ToString(), Name = "Wuppertal", AgenceType_Id = _agenceTypeInternational.Key };

        private readonly Agence _agence05Distri = new Agence { Id = Guid.NewGuid().ToString(), Name = "05Distri", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence15Lacass = new Agence { Id = Guid.NewGuid().ToString(), Name = "15Lacass", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence16Tamtam = new Agence { Id = Guid.NewGuid().ToString(), Name = "16Tamtam", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence17Tamtam = new Agence { Id = Guid.NewGuid().ToString(), Name = "17Tamtam", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence18Mrci = new Agence { Id = Guid.NewGuid().ToString(), Name = "18Mrci", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence26Blanc = new Agence { Id = Guid.NewGuid().ToString(), Name = "26Blanc", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence34Orchestra = new Agence { Id = Guid.NewGuid().ToString(), Name = "34Orchestra", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence42Ziegler = new Agence { Id = Guid.NewGuid().ToString(), Name = "42Ziegler", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence43Sanial = new Agence { Id = Guid.NewGuid().ToString(), Name = "43Sanial", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence54Mcl = new Agence { Id = Guid.NewGuid().ToString(), Name = "54Mcl", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence56Stg = new Agence { Id = Guid.NewGuid().ToString(), Name = "56Stg", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence68Hutchinson = new Agence { Id = Guid.NewGuid().ToString(), Name = "68Hutchinson", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agence83Joyau = new Agence { Id = Guid.NewGuid().ToString(), Name = "83Joyau", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agenceMrci18 = new Agence { Id = Guid.NewGuid().ToString(), Name = "Mrci18", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agenceMrci19 = new Agence { Id = Guid.NewGuid().ToString(), Name = "Mrci19", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agenceMrci87 = new Agence { Id = Guid.NewGuid().ToString(), Name = "Mrci87", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agenceVolvoStPriest = new Agence { Id = Guid.NewGuid().ToString(), Name = "Volvo St Priest", AgenceType_Id = _agenceTypeConfrères.Key };
        private readonly Agence _agenceWurth = new Agence { Id = Guid.NewGuid().ToString(), Name = "Wurth", AgenceType_Id = _agenceTypeConfrères.Key };

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Gefco.CipQuai.Web.Models.ApplicationDbContext";
            this.CommandTimeout = 60 * 5; 
        }

        protected override void Seed(Gefco.CipQuai.Web.Models.ApplicationDbContext context)
        {
            return;
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            AddRoles(context);
            AddSections(context);
            AddPages(context);
            AddConfigurations(context);
            AddMotifsDP(context);
            AddMotifsNC(context);
            AddRegions(context);
            AddResources(context);
            AddRemorqueStatuses(context);
            AddAgenceTypes(context);
            AddAgences(context);
            AddTractionDefinitions(context);
        }

        private void AddTractionDefinitions(ApplicationDbContext context)
        {
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "0BADC6A9-3EB6-4BCB-9DB2-39BB5D1CC73C", Name = "LILLE-LYON", AgenceDepart_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "0CE3DCCA-5191-43D7-83A1-CD095A9DA8ED", Name = "GRENOBLE-LYON", AgenceDepart_Id = "20A82D4B-9F24-45C0-8532-BBC4CE5EF57A", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "126D3A33-CEBB-4010-9F74-EB051C1F44D4", Name = "CORNIER-DIJON", AgenceDepart_Id = "D70F3C75-276E-4489-ABF9-C9D558415C33", AgenceArrivee_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "17DD692F-4F80-4E99-8707-9B6BB93C5D4B", Name = "LILLE-VALENCIENNES", AgenceDepart_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", AgenceArrivee_Id = "064B3930-8B10-4A09-B92A-172876B188D0", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = true });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "193EFDEE-0028-4986-B6FE-4352065051F2", Name = "VA-LILLE", AgenceDepart_Id = "064B3930-8B10-4A09-B92A-172876B188D0", AgenceArrivee_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "207C568C-7A71-497F-B520-78B42938383C", Name = "CORNIER-LYON", AgenceDepart_Id = "D70F3C75-276E-4489-ABF9-C9D558415C33", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "21D760A3-9C0F-4B6A-8BE3-359200C200BB", Name = "LILLE-VA-CHATEAUROUX", AgenceDepart_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", AgenceArrivee_Id = "A21C5F17-1B13-48DA-84B0-749CCF3007F2", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "229BA0A0-2486-44EA-BEBD-2A02B61F1EF6", Name = "MONTPELLIER-LYON", AgenceDepart_Id = "187E54F5-ABA4-4773-97BA-1BADD67167DC", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "22BFC0E0-A7AC-4902-B83D-222F1C86F8EE", Name = "GNV-COVENTRY", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "7C07EFD5-F120-414F-AE42-1040DE26AE2A", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = true });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "23003E71-A800-40F1-916B-0CC1A0D29964", Name = "VA-COVENTRY", AgenceDepart_Id = "064B3930-8B10-4A09-B92A-172876B188D0", AgenceArrivee_Id = "7C07EFD5-F120-414F-AE42-1040DE26AE2A", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "258C2E6D-27FF-49AD-B5CF-72F38B4A0C89", Name = "MARSEILLE-NICE", AgenceDepart_Id = "49E46CAA-2F0B-48D4-98CE-005CEF4EA711", AgenceArrivee_Id = "766E38D2-176E-42C6-9E73-4D45BD6E7918", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "26E59288-7B2E-4CE0-8EEB-FDAE4547C1A4", Name = "AMIENS-GENN", AgenceDepart_Id = "2DC2A55F-5D0C-4A93-9E18-D58E54073BF3", AgenceArrivee_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "2ECD06CA-5198-4B29-AA06-BE54A40842C8", Name = "STRASBOURG-Dijon", AgenceDepart_Id = "4ABB9048-615A-4782-A8F4-0A4D0B93815A", AgenceArrivee_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "30735F57-5A46-446A-B259-1AEEAD97CA90", Name = "LILLE-VA-STRASBOURG", AgenceDepart_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", AgenceArrivee_Id = "8E865411-6B05-4374-8DAC-B68F745E69E5", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "33AAC2B9-CA01-45C6-89D0-A52549B00047", Name = "GNV-REIMS", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "772CEED5-7EA9-4A84-8E2D-47D0C98F52CF", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = true });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "3769C812-1C09-494E-BA36-6DF65D75852C", Name = "AVIGNON NICE", AgenceDepart_Id = "795048EE-EFF6-4774-8B8E-14B614FC2EE2", AgenceArrivee_Id = "766E38D2-176E-42C6-9E73-4D45BD6E7918", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "381885F8-8F5C-4ECB-8AD8-23A5CE444C92", Name = "MARSEILLE-Toulouse", AgenceDepart_Id = "49E46CAA-2F0B-48D4-98CE-005CEF4EA711", AgenceArrivee_Id = "00D4B929-6658-4CD3-83CD-997CF84E5D18", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "391FE825-E76D-4D91-992B-AD799AD592BB", Name = "MOISSY-NANTES", AgenceDepart_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", AgenceArrivee_Id = "766E38D2-176E-42C6-9E73-4D45BD6E7918", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "3970CD07-F009-4969-8476-6FE169D28827", Name = "VA-BRUXELLES", AgenceDepart_Id = "064B3930-8B10-4A09-B92A-172876B188D0", AgenceArrivee_Id = "53958AF0-DE87-4EBD-A61A-35EE6A006475", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "3CC71E35-BB65-4379-A4ED-DBE634CF146E", Name = "CLERMONT - LYON", AgenceDepart_Id = "05DA48FC-E932-43FB-A48C-BC8505ADAF7E", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "4121F18D-725C-4841-A569-EFBCB76621C4", Name = "LILLE-VA-AMIENS", AgenceDepart_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", AgenceArrivee_Id = "A116EB37-14F5-4A51-9ADF-0412BB650DA6", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = true });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "42282278-9FF6-4828-91D3-06FA548AF83D", Name = "DIJON-CORNIER", AgenceDepart_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", AgenceArrivee_Id = "D70F3C75-276E-4489-ABF9-C9D558415C33", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "44E4A9F2-3663-499B-9E38-B78534533D21", Name = "MOISSY-RENNES", AgenceDepart_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", AgenceArrivee_Id = "DC1FC53C-5FC2-49AC-AC29-9F313712C704", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "455E7C01-23EA-4373-8FAD-8DAC4045F882", Name = "DIJON-REIMS", AgenceDepart_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", AgenceArrivee_Id = "772CEED5-7EA9-4A84-8E2D-47D0C98F52CF", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "46DC16C2-37AC-4548-A335-2C1ABC211088", Name = "MOISSY-CHARLEVILLE", AgenceDepart_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", AgenceArrivee_Id = "DA54724F-1FD4-46E2-AAC3-B9DD064BFE80", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "4835DBE8-5E75-4711-A3BF-80F84EDE5190", Name = "NANTES-MOISSY", AgenceDepart_Id = "766E38D2-176E-42C6-9E73-4D45BD6E7918", AgenceArrivee_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "4CF47500-6CF7-43B2-8705-3DE1FBCA8736", Name = "STRASBOURG-MULHOUSE-CHATEAUROUX", AgenceDepart_Id = "4ABB9048-615A-4782-A8F4-0A4D0B93815A", AgenceArrivee_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "4F23923F-D499-422B-B04F-629BFBF72175", Name = "REIMS-MOISSY", AgenceDepart_Id = "772CEED5-7EA9-4A84-8E2D-47D0C98F52CF", AgenceArrivee_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "54E023D6-6252-4F0C-8E14-BC24C88F64B7", Name = "MARSEILLE-AVIGNON", AgenceDepart_Id = "49E46CAA-2F0B-48D4-98CE-005CEF4EA711", AgenceArrivee_Id = "795048EE-EFF6-4774-8B8E-14B614FC2EE2", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "55570925-291E-4376-B6C7-DDE91EA11ADB", Name = "DIJON-70SDTL", AgenceDepart_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", AgenceArrivee_Id = "3664A2EC-17CD-46C2-9A9B-F158E25E6DC1", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "5643394B-EED4-4BC2-B816-0252EEB5659A", Name = "CHÂTEAUROUX-GENNEVILLIERS", AgenceDepart_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", AgenceArrivee_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "5D671C66-00A3-4832-A72F-DB4D5FDD1D7C", Name = "GNV-ORLEANS", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "729BB666-D101-409C-9649-57F193629AC8", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "60C57A3D-1245-49A7-8A48-91FA0D2F6571", Name = "BORDEAUX-TOULOUSE", AgenceDepart_Id = "17C592F1-2CDD-484D-959E-9632076F4BDC", AgenceArrivee_Id = "00D4B929-6658-4CD3-83CD-997CF84E5D18", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "6258F5C9-73B7-47E1-8A2B-51DCC6FB02CB", Name = "DIJON-MULHOUSE", AgenceDepart_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", AgenceArrivee_Id = "BF81D7C8-BC56-4987-8E63-02EEDCE40273", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "642E23FF-4714-44C1-AD2B-1C9525A91A41", Name = "TOULOUSE-MARSEILLE", AgenceDepart_Id = "00D4B929-6658-4CD3-83CD-997CF84E5D18", AgenceArrivee_Id = "49E46CAA-2F0B-48D4-98CE-005CEF4EA711", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "6ACC3D59-1625-4D8F-A386-F33273142512", Name = "TOULOUSE-GENNEVILLIERS", AgenceDepart_Id = "00D4B929-6658-4CD3-83CD-997CF84E5D18", AgenceArrivee_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "6DECC932-1523-4870-AF50-495B8F016929", Name = "DIJON - METZ", AgenceDepart_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", AgenceArrivee_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "73CA37AC-5917-43F0-BA12-F425F0B7B738", Name = "GNV-AMIENS", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "2DC2A55F-5D0C-4A93-9E18-D58E54073BF3", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "7987F125-7199-4664-B77A-B51F0ABB32F3", Name = "TOULOUSE-LYON", AgenceDepart_Id = "00D4B929-6658-4CD3-83CD-997CF84E5D18", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "7A8FE38B-EEBC-45B4-9471-9ED7A542C6CB", Name = "CHÄTEAUROUX-68-STRASBOURG", AgenceDepart_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", AgenceArrivee_Id = "4ABB9048-615A-4782-A8F4-0A4D0B93815A", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "87072F06-0467-4557-85B4-C95964EB1A0A", Name = "ORL-GNV-ORL", AgenceDepart_Id = "729BB666-D101-409C-9649-57F193629AC8", AgenceArrivee_Id = "9D988267-5915-4F20-A4BD-8012EADDAB3E", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "8EDDA87D-EAAB-431F-A35E-EE7C8FE9206E", Name = "AVIGNON-MARSEILLE", AgenceDepart_Id = "795048EE-EFF6-4774-8B8E-14B614FC2EE2", AgenceArrivee_Id = "49E46CAA-2F0B-48D4-98CE-005CEF4EA711", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "940C867A-1798-4748-8E1F-0ACE4782BA57", Name = "MARSEILLE-LYON", AgenceDepart_Id = "49E46CAA-2F0B-48D4-98CE-005CEF4EA711", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "96A7E301-67A1-4D70-BC4A-60AA3B4E9D66", Name = "STRASBOURG-MULHOUSE-VALS-LILLE", AgenceDepart_Id = "4ABB9048-615A-4782-A8F4-0A4D0B93815A", AgenceArrivee_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "97A53EF7-075E-4527-8DFA-F3F62D20638A", Name = "LILLE-GENNEVILLIERS", AgenceDepart_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", AgenceArrivee_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "9E33B2A4-D450-46C6-8EFB-6DE0549D2066", Name = "GNV-VALENCIENNES", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "064B3930-8B10-4A09-B92A-172876B188D0", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "A45F9B0E-3BC8-45AC-9FB2-8D5DF5B6ECAE", Name = "MULHOUSE-MOISSY", AgenceDepart_Id = "BF81D7C8-BC56-4987-8E63-02EEDCE40273", AgenceArrivee_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "A6807D64-20A2-4ACE-A86D-0BB383BCB6FE", Name = "RENNES-MOISSY", AgenceDepart_Id = "DC1FC53C-5FC2-49AC-AC29-9F313712C704", AgenceArrivee_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "A76A54AB-F89C-481C-97A3-1FB335E6DC94", Name = "GNV-ROUEN", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "7631E505-0ACC-4A21-A71D-E33AFAEAAF37", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "A91ED51E-317D-4D91-822D-3EB56880A766", Name = "CHARLEVILLE-REIMS-MOISSY", AgenceDepart_Id = "DA54724F-1FD4-46E2-AAC3-B9DD064BFE80", AgenceArrivee_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "B3858CFE-B97B-4861-99A8-4E1F8D34484C", Name = "AMIENS - VALENCIENNES - LILLE", AgenceDepart_Id = "2DC2A55F-5D0C-4A93-9E18-D58E54073BF3", AgenceArrivee_Id = "3B883446-9B94-4FAA-AA87-28A2A42B3C99", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = true });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "B9A61B0A-BC7F-48D0-ADFA-AC603F0BC407", Name = "CHÂTEAUROUX-BORDEAUX", AgenceDepart_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", AgenceArrivee_Id = "17C592F1-2CDD-484D-959E-9632076F4BDC", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "BF2CCB87-460B-4ED7-987C-1DB103D12365", Name = "ROUEN-GENNEVILLIERS", AgenceDepart_Id = "7631E505-0ACC-4A21-A71D-E33AFAEAAF37", AgenceArrivee_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "C1F3E282-ED78-40ED-B988-A2144F442644", Name = "AMIENS - DIJON - LYON", AgenceDepart_Id = "2DC2A55F-5D0C-4A93-9E18-D58E54073BF3", AgenceArrivee_Id = "2619ECD7-1E59-4294-B61B-B4FF69044C4F", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "C2C5CB92-D807-41B5-BEA1-A2E7CC0F8F67", Name = "BORDEAUX-CHATEAUROUX", AgenceDepart_Id = "17C592F1-2CDD-484D-959E-9632076F4BDC", AgenceArrivee_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "C42837F3-45DE-4054-9EC2-1223DFAABB6C", Name = "RENNES-QUIMPER", AgenceDepart_Id = "DC1FC53C-5FC2-49AC-AC29-9F313712C704", AgenceArrivee_Id = "F68711EF-3DAD-42F9-B431-EDB01D3FE256", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "C6BB65E5-748D-4FB0-BA11-B8B4EAAAEE62", Name = "CLERMONT - TOULOUSE", AgenceDepart_Id = "05DA48FC-E932-43FB-A48C-BC8505ADAF7E", AgenceArrivee_Id = "00D4B929-6658-4CD3-83CD-997CF84E5D18", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "C7556C01-67E2-4D1C-A180-7A74A049CC45", Name = "MULHOUSE-LYON", AgenceDepart_Id = "BF81D7C8-BC56-4987-8E63-02EEDCE40273", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "C984479B-5300-47DF-9129-E86A66770021", Name = "GNV-LYON", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "CB1C3FBA-C52E-44C9-AD07-60A65D6E2FD0", Name = "VA-GENNEVILLIERS", AgenceDepart_Id = "064B3930-8B10-4A09-B92A-172876B188D0", AgenceArrivee_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "CE14D721-C2A5-465C-8458-55B473474537", Name = "MULHOUSE-DIJON", AgenceDepart_Id = "BF81D7C8-BC56-4987-8E63-02EEDCE40273", AgenceArrivee_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "CF858FFA-D2F5-41C7-9781-6E6B78FDD0E2", Name = "NANTES-LYON", AgenceDepart_Id = "766E38D2-176E-42C6-9E73-4D45BD6E7918", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "D0A3362E-6A27-406F-8B93-3589E8DC21B5", Name = "CHÂTEAUROUX-VALENCIE/LILLE", AgenceDepart_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", AgenceArrivee_Id = "AF4E93F8-30F1-4FCC-97B6-2A93D78054E8", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "D0A385A7-2F8C-4833-8361-68F00533F2DC", Name = "DIJON-STRASBOURG", AgenceDepart_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", AgenceArrivee_Id = "4ABB9048-615A-4782-A8F4-0A4D0B93815A", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "D3EAEC1C-4317-40E4-ADCC-D9A07235B0E8", Name = "GRENOBLE-AVIGNON", AgenceDepart_Id = "20A82D4B-9F24-45C0-8532-BBC4CE5EF57A", AgenceArrivee_Id = "795048EE-EFF6-4774-8B8E-14B614FC2EE2", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "D46C006A-CEFD-46F8-974B-947D6D8C872C", Name = "GNV-LILLE", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "DAF50E5A-3EB5-4029-B80F-F78F5B2FC35B", Name = "ORL-DIJON", AgenceDepart_Id = "729BB666-D101-409C-9649-57F193629AC8", AgenceArrivee_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "DC7AD701-C191-40D9-BDC6-8547B0BE5415", Name = "LILLE-VA-METZ", AgenceDepart_Id = "071406AD-A58E-4B8B-A158-89F5953146F9", AgenceArrivee_Id = "F44DE44A-9657-4FCC-A59F-70FECD439F1D", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = true });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "E33BF5CC-8E0E-4CAA-B96F-97B6CCF282C6", Name = "REIMS-DIJON", AgenceDepart_Id = "772CEED5-7EA9-4A84-8E2D-47D0C98F52CF", AgenceArrivee_Id = "C067CB43-93A6-4449-A8AD-43D92B36BC01", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "F129341C-7894-4D9C-8EFB-D390A5FA90F9", Name = "CHÄTEAUROUX-LYON", AgenceDepart_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", AgenceArrivee_Id = "A4124499-49DF-43F2-B0D3-A3D7F3C8E829", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "F77144FF-FDA5-45CC-81D0-AC9621A1328A", Name = "GNV-CHATEAUROUX", AgenceDepart_Id = "D269E0EA-9E9A-43F5-8A0A-3B05396E6F3D", AgenceArrivee_Id = "04A32C17-E75F-430B-AD19-5738E8FCB221", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = false });
            context.TractionDefinitions.AddOrUpdate(new TractionDefinition() { Id = "F7D7C246-59AB-48B2-B34B-39842BBAB256", Name = "VA-OOSTERHOUT", AgenceDepart_Id = "064B3930-8B10-4A09-B92A-172876B188D0", AgenceArrivee_Id = "4DC5F8C6-57DE-4E1C-8308-485AF9CAC568", DaysOfWeekValue = "0,1,2,3,4", IsDeleted = true });
        }

        private void AddAgences(ApplicationDbContext context)
        {
            context.Agences.AddOrUpdate(_agence70SDTL);
            context.Agences.AddOrUpdate(_agenceAmiens);
            context.Agences.AddOrUpdate(_agenceAvignon);
            context.Agences.AddOrUpdate(_agenceBordeaux);
            context.Agences.AddOrUpdate(_agenceBoucBelAir);
            context.Agences.AddOrUpdate(_agenceBruxelles);
            context.Agences.AddOrUpdate(_agenceCharleville);
            context.Agences.AddOrUpdate(_agenceChateauroux);
            context.Agences.AddOrUpdate(_agenceClermontFerrand);
            context.Agences.AddOrUpdate(_agenceCornier);
            context.Agences.AddOrUpdate(_agenceCoventry);
            context.Agences.AddOrUpdate(_agenceDijon);
            context.Agences.AddOrUpdate(_agenceDijonLyon);
            context.Agences.AddOrUpdate(_agenceGennevilliers);
            context.Agences.AddOrUpdate(_agenceGennevilliersOrléans);
            context.Agences.AddOrUpdate(_agenceGrenoble);
            context.Agences.AddOrUpdate(_agenceLille);
            context.Agences.AddOrUpdate(_agenceLyon);
            context.Agences.AddOrUpdate(_agenceMarseille);
            context.Agences.AddOrUpdate(_agenceMetz);
            context.Agences.AddOrUpdate(_agenceMoissy);
            context.Agences.AddOrUpdate(_agenceMontpellier);
            context.Agences.AddOrUpdate(_agenceMulhouse);
            context.Agences.AddOrUpdate(_agenceNantes);
            context.Agences.AddOrUpdate(_agenceNice);
            context.Agences.AddOrUpdate(_agenceOosterhout);
            context.Agences.AddOrUpdate(_agenceOrleans);
            context.Agences.AddOrUpdate(_agenceQuimper);
            context.Agences.AddOrUpdate(_agenceReims);
            context.Agences.AddOrUpdate(_agenceRennes);
            context.Agences.AddOrUpdate(_agenceRouen);
            context.Agences.AddOrUpdate(_agenceStrasbourg);
            context.Agences.AddOrUpdate(_agenceToulouse);
            context.Agences.AddOrUpdate(_agenceTroyes);
            context.Agences.AddOrUpdate(_agenceValenciennes);
            context.Agences.AddOrUpdate(_agenceValenciennesOuLille);
            context.Agences.AddOrUpdate(_agenceValenciennesAmiens);
            context.Agences.AddOrUpdate(_agenceValenciennesChâteauroux);
            context.Agences.AddOrUpdate(_agenceValenciennesLille);
            context.Agences.AddOrUpdate(_agenceValenciennesStrasbourg);
            context.Agences.AddOrUpdate(_agenceVenissieux);
            context.Agences.AddOrUpdate(_agenceVesoul);


            context.Agences.AddOrUpdate(_agenceOyonnax);
            context.Agences.AddOrUpdate(_agencePerpignan);
            context.Agences.AddOrUpdate(_agenceSaintEtienne);
            context.Agences.AddOrUpdate(_agenceStJeanDeLuz);
            context.Agences.AddOrUpdate(_agenceTarbes);
            context.Agences.AddOrUpdate(_agenceTours);

            context.Agences.AddOrUpdate(_agenceBarcelone);
            context.Agences.AddOrUpdate(_agenceBerlin);
            context.Agences.AddOrUpdate(_agenceDartford);
            context.Agences.AddOrUpdate(_agenceDelemont);
            context.Agences.AddOrUpdate(_agenceDourges);
            context.Agences.AddOrUpdate(_agenceFrankfort);
            context.Agences.AddOrUpdate(_agenceFrankfurt);
            context.Agences.AddOrUpdate(_agenceGbrnetwo);
            context.Agences.AddOrUpdate(_agenceGeneve);
            context.Agences.AddOrUpdate(_agenceKatowice);
            context.Agences.AddOrUpdate(_agenceMadrid);
            context.Agences.AddOrUpdate(_agenceManchester);
            context.Agences.AddOrUpdate(_agenceMilano);
            context.Agences.AddOrUpdate(_agenceMiranda);
            context.Agences.AddOrUpdate(_agencePadova);
            context.Agences.AddOrUpdate(_agencePorto);
            context.Agences.AddOrUpdate(_agenceTorino);
            context.Agences.AddOrUpdate(_agenceValencia);
            context.Agences.AddOrUpdate(_agenceVigoPcvepr);
            context.Agences.AddOrUpdate(_agenceWien);
            context.Agences.AddOrUpdate(_agenceWroclaw);
            context.Agences.AddOrUpdate(_agenceWuppertal);

            context.Agences.AddOrUpdate(_agence05Distri);
            context.Agences.AddOrUpdate(_agence15Lacass);
            context.Agences.AddOrUpdate(_agence16Tamtam);
            context.Agences.AddOrUpdate(_agence17Tamtam);
            context.Agences.AddOrUpdate(_agence18Mrci);
            context.Agences.AddOrUpdate(_agence26Blanc);
            context.Agences.AddOrUpdate(_agence34Orchestra);
            context.Agences.AddOrUpdate(_agence42Ziegler);
            context.Agences.AddOrUpdate(_agence43Sanial);
            context.Agences.AddOrUpdate(_agence54Mcl);
            context.Agences.AddOrUpdate(_agence56Stg);
            context.Agences.AddOrUpdate(_agence68Hutchinson);
            context.Agences.AddOrUpdate(_agence83Joyau);
            context.Agences.AddOrUpdate(_agenceMrci18);
            context.Agences.AddOrUpdate(_agenceMrci19);
            context.Agences.AddOrUpdate(_agenceMrci87);
            context.Agences.AddOrUpdate(_agenceVolvoStPriest);
            context.Agences.AddOrUpdate(_agenceWurth);
        }

        private void AddAgenceTypes(ApplicationDbContext context)
        {
            context.AgenceTypes.AddOrUpdate(_agenceTypeGefco);
            context.AgenceTypes.AddOrUpdate(_agenceTypeInternational);
            context.AgenceTypes.AddOrUpdate(_agenceTypeConfrères);
        }

        private void AddRemorqueStatuses(ApplicationDbContext context)
        {
            context.RemorqueStatuses.AddOrUpdate(new RemorqueStatus() { Id = "019A06A2-69D6-48CD-8AC3-B725E49CCB35", CreationDate = DateTime.UtcNow, Name = "Valid", Description = "Valide" });
            context.RemorqueStatuses.AddOrUpdate(new RemorqueStatus() { Id = "3AB28267-78A4-4994-9822-1E2E52595FC0", CreationDate = DateTime.UtcNow, Name = "NotValid", Description = "Not Valid" });
            context.RemorqueStatuses.AddOrUpdate(new RemorqueStatus() { Id = "3ED5B22D-7F89-4B17-99DC-3D7CA5D051B8", CreationDate = DateTime.UtcNow, Name = "ToJustify", Description = "To justify" });
            context.RemorqueStatuses.AddOrUpdate(new RemorqueStatus() { Id = "460BF74D-9D95-4D13-A8FC-E76CA933AD1A", CreationDate = DateTime.UtcNow, Name = "ToBeValidated", Description = "To be validated" });
            context.RemorqueStatuses.AddOrUpdate(new RemorqueStatus() { Id = "62C4E8FD-F31A-4A59-B24D-6669D5620161", CreationDate = DateTime.UtcNow, Name = "PausedAndLocked", Description = "Paused and locked" });
            context.RemorqueStatuses.AddOrUpdate(new RemorqueStatus() { Id = "9706DF5F-85E5-435D-BF18-3C0502F810CF", CreationDate = DateTime.UtcNow, Name = "PausedAndFree", Description = "Paused and free" });
            context.RemorqueStatuses.AddOrUpdate(new RemorqueStatus() { Id = "F516E0DD-830E-4695-986C-5AF2FCB7E8C9", CreationDate = DateTime.UtcNow, Name = "InProgress", Description = "In progress" });
        }

        private void AddResources(ApplicationDbContext context)
        {
            context.Resources.AddOrUpdate(new Resource() { Id = "09289A31-013A-47A7-9E82-36873D753CB4", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.SaisieAutreMotifDPInvite", Value = "Chargement en DP Impossible,\r\nVeuillez indiquer la raison.", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "1B8F3602-3A27-4249-B1FA-68A65CAF2B65", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.SuspendButtonInvite", Value = "Suspendre", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "1F94C9CF-1C37-4433-A409-E7E8032F0CAB", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.StartPageInviteMultiple", Value = " remorques DP sont à charger\r\nsur votre site", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "28598C08-7977-46F8-A25A-DB2612F2CCB1", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.CancelInvite", Value = "Annuler", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "3372572C-6267-415D-B042-032C9B16FD99", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.MotifsLabel", Value = "Motif(s)", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "4471DEA0-BEA3-49EF-971B-F7C59CC156A1", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.TakeSecondPictureButtonLabel", Value = "Photo à fin de chargement", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "4752DC91-337E-4BAB-A52D-05EE23551504", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.TakePictureButtonLabel", Value = "Prendre photo", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "4CA69C32-1A8D-42C4-80E3-49732FEB4013", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.NoResultsMessage", Value = "Aucune remorque trouvée...", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "519E758C-E6E4-4A9C-9CA5-21368C64896E", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.EndImpossibleDeclarationInvite", Value = "Conserver photo et\r\nTerminer déclaration", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "6F985CF1-C8B6-4341-879C-73C772CF1A8B", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.CancelDeclarationInvite", Value = "Vous êtes sur le point de suspendre votre déclaration,\r\nvous pouvez la reprendre à tout moment", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "71ECCCAF-3251-4B14-8053-1B61390A014C", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.DeletePictureButtonLabel", Value = "Supprimer", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "763DF9B4-E04C-4726-A02C-CC15F9023996", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.TractionInvite", Value = "Remorque", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "78FF6D57-A38E-4113-B25D-20780898D3F4", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.ChoosePictureButtonLabel", Value = "Choisir depuis la galerie", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "7DCDEFB9-44D0-4A10-8CC5-7374943C37D3", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.SaisieNbBarresDPCasseesInvite", Value = "Veuillez indiquer le nombre\r\nde barres DP cassées", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "7F2EDB7A-9DA7-4DA3-BCCE-6A8498EEA432", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.ColleagueButtonInvite", Value = "Un collègue peut terminer ma déclaration", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "959515DA-A53B-4D2F-AE51-D4F34E825681", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.StartPageInviteSingle", Value = "1 remorque DP est à charger\r\nsur votre site", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "97E440EA-F37C-44DA-A7EA-7D98A2C17302", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.DeclarationImpossibleInvite", Value = "Chargement en DP Impossible,\r\nVeuillez indiquer les raisons.", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "9A8F4B3B-0E58-441E-8394-144167CA4375", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.PageTitle", Value = "Chargement DP", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "9CD69440-422B-4CD5-BD82-35998A68BD5C", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.ResumeButtonInvite", Value = "Reprendre", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "A1762140-F636-4CCF-9AE2-5DFDB31FDD99", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.TakeFirstPictureButtonLabel", Value = "Photo à moitié du chargement", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "A19E61D2-8C2B-44E4-9F72-E5F71B152942", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.RestartButtonInvite", Value = "Vous êtes sur le point de rependre une déclaration.\r\nVous pouvez aussi la refaire depuis le début.", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "CF6018C3-0DFB-44EE-9DD2-E5FFAEA47EC0", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.ValidationSummary", Value = "Aucune remorque sélectionnée", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "EAF05D6F-C328-4BB8-8B49-ED49470B7368", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.RemorqueNumberLabel", Value = "Remorque N°", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "EC2F3A48-EE01-49D7-A2B9-398253B30C2E", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.TakeNextPictureInvite", Value = "Conserver photo et\r\nprendre nouvelle photo", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "EF06A4A9-D606-46AB-8725-025213842BC7", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.ImpossiblePageInvite", Value = "Chargement en DP impossible", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "EF5D3643-4517-4AE5-B771-D09815DEFE46", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.TakeLoadPictureButtonLabel", Value = "Prendre photo du chargement", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "F3F6CE92-2C86-407A-A27C-D56B6DC43E8C", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.RetakePictureInvite", Value = "Refaire photo", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "F705A3EB-62D5-4F2A-B244-FE209821BF50", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.CantTakePictureButtonLabel", Value = "Chargement en DP impossible", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "F8A1D51D-419D-432F-9125-BB2D4927F7D7", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.AgenceDepartLabel", Value = "Agence de départ", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "FC77B8D8-C949-4E93-8AF4-89C75E338A3B", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.AgenceArriveeLabel", Value = "Agence d'arrivée", Description = "", IsForAll = true, Type = ResourceType.Text });
            context.Resources.AddOrUpdate(new Resource() { Id = "FC958F03-AD42-4B76-9705-29E2C8F90EBE", LastUpdate = DateTime.UtcNow, CreationDate = DateTime.UtcNow, Key = "DoubleDeckPage.NextButtonLabel", Value = "Suivant >", Description = "", IsForAll = true, Type = ResourceType.Text });
        }
        private void AddRegions(ApplicationDbContext context)
        {
            context.Regions.AddOrUpdate(_regionIDF);
            context.Regions.AddOrUpdate(_regionGrandOuest);
            context.Regions.AddOrUpdate(_regionSudEst);
            context.Regions.AddOrUpdate(_regionGrandEst);
        }


        private void AddMotifsNC(ApplicationDbContext context)
        {
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "41A5A1E6-484B-420F-A27D-A2D6AB86DC90", CreationDate = DateTime.UtcNow, Name = "Risque d'évènement accidentel", DisplayOrder = 9 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "4C50979F-3393-45DC-A619-38B65E90A150", CreationDate = DateTime.UtcNow, Name = "Barres DP déteriorées", DisplayOrder = 7 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "6DC717D0-44EE-40AA-BE57-3F234624D79E", CreationDate = DateTime.UtcNow, Name = "Dynamic mal positionné", DisplayOrder = 6 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "98BFDBAB-0DE5-493B-9CA4-1C30E29948D6", CreationDate = DateTime.UtcNow, Name = "Fret renversé", DisplayOrder = 3 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "A1943639-910B-4539-A7B2-44E35A2D6107", CreationDate = DateTime.UtcNow, Name = "Colis en vrac non palettisé", DisplayOrder = 4 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "A9F3EE03-1BE5-4A1F-BFB3-DD14C3A14BA9", CreationDate = DateTime.UtcNow, Name = "Fret gerbé", DisplayOrder = 2 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "B83B4824-CCEE-403A-9B75-CDBE425A2733", CreationDate = DateTime.UtcNow, Name = "Absence de barres DP", DisplayOrder = 8 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "D80D8A51-5BE9-4215-9214-B0BA7F3B9790", CreationDate = DateTime.UtcNow, Name = "Autre", DisplayOrder = 10, IsOther = true });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "D926ED8F-F3A7-41D0-BFA2-0C92D46B7EA1", CreationDate = DateTime.UtcNow, Name = "Fret écrasé", DisplayOrder = 1 });
            context.MotifNCs.AddOrUpdate(new MotifNC() { Id = "EF6C08FC-4BD0-4D04-A96A-1D256C8931F2", CreationDate = DateTime.UtcNow, Name = "ADR gerbé", DisplayOrder = 5 });
        }
        private void AddMotifsDP(ApplicationDbContext context)
        {
            context.MotifDPs.AddOrUpdate(new MotifDP() { Id = "0FB5BD2B-9E80-4CE7-8746-489D014FEEE0", CreationDate = DateTime.UtcNow, Name = "Autre raison", DisplayOrder = 5, NeedPicture = true, IsNbDP = false, IsOther = true });
            context.MotifDPs.AddOrUpdate(new MotifDP() { Id = "42B963B9-4B9A-4447-844D-C6DD47E73BEA", CreationDate = DateTime.UtcNow, Name = "Arrêt technique remorque", DisplayOrder = 4, NeedPicture = false, IsNbDP = false, IsOther = false });
            context.MotifDPs.AddOrUpdate(new MotifDP() { Id = "52402E68-B59C-47C9-B5CC-A6B5820BC284", CreationDate = DateTime.UtcNow, Name = "Fret insuffisant", DisplayOrder = 1, NeedPicture = true, IsNbDP = false, IsOther = false });
            context.MotifDPs.AddOrUpdate(new MotifDP() { Id = "A4782CA6-084D-429C-9F66-3CFEB87D9B42", CreationDate = DateTime.UtcNow, Name = "Fret volumineux", DisplayOrder = 0, NeedPicture = true, IsNbDP = false, IsOther = false });
            context.MotifDPs.AddOrUpdate(new MotifDP() { Id = "C47D8951-0BE9-4DD4-9774-3CA0C64A190B", CreationDate = DateTime.UtcNow, Name = "Barres DP cassées", DisplayOrder = 2, NeedPicture = true, IsNbDP = true, IsOther = false });
            context.MotifDPs.AddOrUpdate(new MotifDP() { Id = "DC7E7B69-A096-4400-B5C1-78D53B250A0A", CreationDate = DateTime.UtcNow, Name = "Remorque annulée", DisplayOrder = 3, NeedPicture = false, IsNbDP = false, IsOther = false });
        }

        private void AddConfigurations(ApplicationDbContext context)
        {
            context.Configurations.AddOrUpdate(new Models.Configuration() { Id = "12093DE0-4848-4856-8D76-C4C59AE910C2", CreationDate = DateTime.UtcNow, Value = "DeclarationTraction", Name = "Mobile.DoubleDeckPage.TractionListItemSortOrder", Description = "" });
            context.Configurations.AddOrUpdate(new Models.Configuration() { Id = "4F53E89F-2769-44BD-B8D0-446443DACE7E", CreationDate = DateTime.UtcNow, Value = "Descending", Name = "Mobile.DoubleDeckPage.DeclarationDateSortOrder", Description = "Ordre de tri par date de Déclaration" });
            context.Configurations.AddOrUpdate(new Models.Configuration() { Id = "6AE561AA-FBA0-4901-A0E0-8B2633AB8D68", CreationDate = DateTime.UtcNow, Value = "5", Name = "NumberOfDaysForPastDeclarations", Description = "Nombre de jours dans le passé pendant lequel remonter les déclarations non effectuées" });
            context.Configurations.AddOrUpdate(new Models.Configuration() { Id = "AE1C938B-E4EB-4762-BB78-BDC2854A36D0", CreationDate = DateTime.UtcNow, Value = "0", Name = "Mobile.MinimumPrefixCharacters", Description = "Nombre de caractères de saisie avant suggestion de Traction" });
        }
        private void AddPages(ApplicationDbContext context)
        {
            context.Pages.AddOrUpdate(new Page() { Id = "034117c8-bd0b-4075-8573-55dc5ca41a54", CreationDate = DateTime.UtcNow, Name = "Dashboard CRM", SortOrder = 2, Icon = "feather icon-bar-chart", Link = "/Home/DashboardCrm", Section_Id = "1fbee189-9cc7-4b8c-a287-66bb6d4382e2", MenuTag = "menu-dash-crm", });
            context.Pages.AddOrUpdate(new Page() { Id = "2374898f-78d8-4583-811b-43328665f4a3", CreationDate = DateTime.UtcNow, Name = "Roles", SortOrder = 2, Icon = "feather icon-shield", Link = "/Roles", Section_Id = "00ed1b86-b088-4c7e-9203-a5b1cea23ca2", MenuTag = "menu-admin-roles", });
            context.Pages.AddOrUpdate(new Page() { Id = "5ce12d5a-03f3-4f5c-bfae-2153caf562ff", CreationDate = DateTime.UtcNow, Name = "Dashboard Analytics", SortOrder = 3, Icon = "feather icon-pie-chart", Link = "/Home/DashboardAnalytics", Section_Id = "1fbee189-9cc7-4b8c-a287-66bb6d4382e2", MenuTag = "menu-dash-analytics", });
            context.Pages.AddOrUpdate(new Page() { Id = "65179d7b-6a46-468f-bffa-9bfc4bcb07f0", CreationDate = DateTime.UtcNow, Name = "Pages", SortOrder = 4, Icon = "feather icon-layers", Link = "/Pages", Section_Id = "00ed1b86-b088-4c7e-9203-a5b1cea23ca2", MenuTag = "menu-admin-pages", });
            context.Pages.AddOrUpdate(new Page() { Id = "b08e3196-0377-482f-82ff-b9062833ede7", CreationDate = DateTime.UtcNow, Name = "Dashboard", SortOrder = 1, Icon = "feather icon-bar-chart-2", Link = "/Home/Dashboard", Section_Id = "1fbee189-9cc7-4b8c-a287-66bb6d4382e2", MenuTag = "menu-dash-default", });
            context.Pages.AddOrUpdate(new Page() { Id = "c11fcf77-c517-4aa4-b751-4445cd7d2b12", CreationDate = DateTime.UtcNow, Name = "Sections", SortOrder = 3, Icon = "feather icon-menu", Link = "/Sections", Section_Id = "00ed1b86-b088-4c7e-9203-a5b1cea23ca2", MenuTag = "menu-admin-sections", });
            context.Pages.AddOrUpdate(new Page() { Id = "e14803c7-c5a1-4bd9-8135-23c154378c00", CreationDate = DateTime.UtcNow, Name = "Users", SortOrder = 1, Icon = "feather icon-users", Link = "/Users", Section_Id = "00ed1b86-b088-4c7e-9203-a5b1cea23ca2", MenuTag = "menu-admin-users", });
            context.Pages.AddOrUpdate(new Page() { Id = "e8fd0c5b-1a31-45fa-bc87-7a0aa82c12a1", CreationDate = DateTime.UtcNow, Name = "National", SortOrder = 0, Icon = "feather icon-bar-chart-2", Link = "/NationalDashboard", Section_Id = "1fbee189-9cc7-4b8c-a287-66bb6d4382e2", MenuTag = "menu-dash-national", });
        }
        private void AddSections(ApplicationDbContext context)
        {
            context.Sections.AddOrUpdate(new Section() { Id = "1fbee189-9cc7-4b8c-a287-66bb6d4382e2", CreationDate = DateTime.UtcNow, SortOrder = 0, Name = "Navigation" });
            context.Sections.AddOrUpdate(new Section() { Id = "00ed1b86-b088-4c7e-9203-a5b1cea23ca2", CreationDate = DateTime.UtcNow, SortOrder = 1, Name = "Administration" });
        }

        private void AddRoles(ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(new IdentityRole("Mobile Users") { Id = "0AAE7034-44F8-4B03-AB8E-AC40E09809D3" });
            context.Roles.AddOrUpdate(new IdentityRole("Agence Users") { Id = "6385E45C-5A7F-4EB7-B96C-D510C48BE792" });
            context.Roles.AddOrUpdate(new IdentityRole("Regional Users") { Id = "9AF0D992-A5C9-46C3-80BC-E576814C67B6" });
            _sAdminRole = new IdentityRole("Super Admin") { Id = "a53f8907-9ee1-414d-b736-be82f3ef3001" };
            context.Roles.AddOrUpdate(_sAdminRole);
            context.Roles.AddOrUpdate(new IdentityRole("Web Users") { Id = "cbc9e82b-d27e-49f2-965e-27d4837087ca" });
            _nationalRole = new IdentityRole("National Users") { Id = "F804AB48-7848-41F0-B89E-2A418D67492B" };
            context.Roles.AddOrUpdate(_nationalRole);
        }
    }
}
