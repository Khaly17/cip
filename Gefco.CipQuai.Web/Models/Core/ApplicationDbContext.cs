using System.Data.Entity;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gefco.CipQuai.Web.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Resource>()
                .HasMany(s => s.Users)
                .WithMany(c => c.Resources)
                .Map(cs =>
                {
                    cs.MapLeftKey("Resource_Id");
                    cs.MapRightKey("User_Id");
                    cs.ToTable("ResourceUsers");
                });

            modelBuilder.Entity<DeclarationDoublePlancher>()
                .HasMany(s => s.MotifDps)
                .WithMany(c => c.DeclarationDoublePlanchers)
                .Map(cs =>
                {
                    cs.MapLeftKey("DeclarationDoublePlancher_Id");
                    cs.MapRightKey("MotifDp_Id");
                    cs.ToTable("DeclarationDoublePlancher_MotifDp");
                });
            modelBuilder.Entity<DeclarationNonConformite>()
                .HasMany(s => s.MotifNCs)
                .WithMany(c => c.DeclarationNonConformites)
                .Map(cs =>
                {
                    cs.MapLeftKey("DeclarationNonConformite_Id");
                    cs.MapRightKey("MotifNC_Id");
                    cs.ToTable("DeclarationNonConformite_MotifNC");
                });
            
        }
        
        public bool IsDisposed { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Agence> Agences { get; set; }
        public DbSet<AgenceType> AgenceTypes { get; set; }
        public DbSet<TractionDefinition> TractionDefinitions { get; set; }
        public DbSet<Traction> Tractions { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<TempPicture> TempPictures { get; set; }
        public DbSet<DeclarationDoublePlancher> DeclarationDoublePlanchers { get; set; }
        public DbSet<DeclarationSimplePlancher> DeclarationSimplePlanchers { get; set; }
        public DbSet<DeclarationControleReception> DeclarationControleReceptions { get; set; }
        public DbSet<RemorqueStatus> RemorqueStatuses { get; set; }
        public DbSet<DeclarationRemorqueStatus> DeclarationRemorqueStatuses { get; set; }
        public DbSet<DeclarationNonConformite> DeclarationNonConformites { get; set; }
        public DbSet<DeclarationNcStatus> DeclarationNcStatuses { get; set; }
        public DbSet<DeclarationBonnePratique> DeclarationBonnePratiques { get; set; }
        public DbSet<MotifNC> MotifNCs { get; set; }
        public DbSet<MotifDP> MotifDPs { get; set; }

        public DbSet<Remorque> Remorques { get; set; }
        public DbSet<BusinessRole> BusinessRoles { get; set; }
        public DbSet<AgenceRole> AgenceRoles { get; set; }
        public DbSet<RegionRole> RegionRoles { get; set; }
        public DbSet<NationalRole> NationalRoles { get; set; }
        public DbSet<UserAgenceRole> UserAgenceRoles { get; set; }
        public DbSet<UserRegionRole> UserRegionRoles { get; set; }
        public DbSet<UserNationalRole> UserNationalRoles { get; set; }

    }
}