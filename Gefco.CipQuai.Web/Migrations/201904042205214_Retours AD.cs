namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetoursAD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agences",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        IsStart = c.Boolean(nullable: false),
                        IsEnd = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Region_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.ResourceRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Resource_Id = c.String(maxLength: 128),
                        Role_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Resources", t => t.Resource_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Resource_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.DeclarationRemorques",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        IsDPUsed = c.Boolean(),
                        NbDPCassées = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Remorque_Id = c.String(maxLength: 128),
                        Status_Id = c.String(maxLength: 128),
                        Traction_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Remorques", t => t.Remorque_Id)
                .ForeignKey("dbo.DeclarationRemorqueStatus", t => t.Status_Id)
                .ForeignKey("dbo.Tractions", t => t.Traction_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Remorque_Id)
                .Index(t => t.Status_Id)
                .Index(t => t.Traction_Id);
            
            CreateTable(
                "dbo.MotifDPs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.Remorques",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        NuméroRemorque = c.String(),
                        IsDoublePlancher = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.DeclarationRemorqueStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.Tractions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        NumeroBorderau = c.String(),
                        IdVoyage = c.String(),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        AgenceArrivee_Id = c.String(maxLength: 128),
                        AgenceDepart_Id = c.String(maxLength: 128),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agences", t => t.AgenceArrivee_Id)
                .ForeignKey("dbo.Agences", t => t.AgenceDepart_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.AgenceArrivee_Id)
                .Index(t => t.AgenceDepart_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.MotifNCs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.ProvenanceDPs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        IsGefcoFrance = c.Boolean(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        IsClient = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.ResourceUsers",
                c => new
                    {
                        Resource_Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        User_Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                    })
                .PrimaryKey(t => new { t.Resource_Id, t.User_Id })
                .ForeignKey("dbo.Resources", t => t.Resource_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Resource_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AgenceUsers",
                c => new
                    {
                        Agence_Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        User_Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                    })
                .PrimaryKey(t => new { t.Agence_Id, t.User_Id })
                .ForeignKey("dbo.Agences", t => t.Agence_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Agence_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.DeclarationDoublePlancher_MotifDp",
                c => new
                    {
                        DeclarationDoublePlancher_Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        MotifDp_Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                    })
                .PrimaryKey(t => new { t.DeclarationDoublePlancher_Id, t.MotifDp_Id })
                .ForeignKey("dbo.DeclarationRemorques", t => t.DeclarationDoublePlancher_Id, cascadeDelete: true)
                .ForeignKey("dbo.MotifDPs", t => t.MotifDp_Id, cascadeDelete: true)
                .Index(t => t.DeclarationDoublePlancher_Id)
                .Index(t => t.MotifDp_Id);
            
            AddColumn("dbo.Devices", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Devices", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Pages", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pages", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.PageRoles", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.PageRoles", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Sections", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sections", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Resources", "IsForAll", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resources", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Resources", "ValueAsBytes", c => c.Binary());
            AddColumn("dbo.Resources", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resources", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Pin", c => c.String());
            CreateIndex("dbo.Resources", "CreatedBy_Id");
            CreateIndex("dbo.Devices", "CreatedBy_Id");
            CreateIndex("dbo.Pages", "CreatedBy_Id");
            CreateIndex("dbo.PageRoles", "CreatedBy_Id");
            CreateIndex("dbo.Sections", "CreatedBy_Id");
            AddForeignKey("dbo.Resources", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Devices", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Pages", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PageRoles", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Sections", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProvenanceDPs", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sections", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PageRoles", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pages", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MotifNCs", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Devices", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationRemorques", "Traction_Id", "dbo.Tractions");
            DropForeignKey("dbo.DeclarationRemorques", "Status_Id", "dbo.DeclarationRemorqueStatus");
            DropForeignKey("dbo.DeclarationRemorques", "Remorque_Id", "dbo.Remorques");
            DropForeignKey("dbo.DeclarationRemorques", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tractions", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tractions", "AgenceDepart_Id", "dbo.Agences");
            DropForeignKey("dbo.Tractions", "AgenceArrivee_Id", "dbo.Agences");
            DropForeignKey("dbo.DeclarationRemorqueStatus", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Remorques", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationDoublePlancher_MotifDp", "MotifDp_Id", "dbo.MotifDPs");
            DropForeignKey("dbo.DeclarationDoublePlancher_MotifDp", "DeclarationDoublePlancher_Id", "dbo.DeclarationRemorques");
            DropForeignKey("dbo.MotifDPs", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AgenceUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AgenceUsers", "Agence_Id", "dbo.Agences");
            DropForeignKey("dbo.Agences", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.Regions", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Agences", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ResourceUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ResourceUsers", "Resource_Id", "dbo.Resources");
            DropForeignKey("dbo.ResourceRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.ResourceRoles", "Resource_Id", "dbo.Resources");
            DropForeignKey("dbo.ResourceRoles", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Resources", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DeclarationDoublePlancher_MotifDp", new[] { "MotifDp_Id" });
            DropIndex("dbo.DeclarationDoublePlancher_MotifDp", new[] { "DeclarationDoublePlancher_Id" });
            DropIndex("dbo.AgenceUsers", new[] { "User_Id" });
            DropIndex("dbo.AgenceUsers", new[] { "Agence_Id" });
            DropIndex("dbo.ResourceUsers", new[] { "User_Id" });
            DropIndex("dbo.ResourceUsers", new[] { "Resource_Id" });
            DropIndex("dbo.ProvenanceDPs", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Sections", new[] { "CreatedBy_Id" });
            DropIndex("dbo.PageRoles", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Pages", new[] { "CreatedBy_Id" });
            DropIndex("dbo.MotifNCs", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Devices", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Tractions", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Tractions", new[] { "AgenceDepart_Id" });
            DropIndex("dbo.Tractions", new[] { "AgenceArrivee_Id" });
            DropIndex("dbo.DeclarationRemorqueStatus", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Remorques", new[] { "CreatedBy_Id" });
            DropIndex("dbo.MotifDPs", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationRemorques", new[] { "Traction_Id" });
            DropIndex("dbo.DeclarationRemorques", new[] { "Status_Id" });
            DropIndex("dbo.DeclarationRemorques", new[] { "Remorque_Id" });
            DropIndex("dbo.DeclarationRemorques", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Regions", new[] { "CreatedBy_Id" });
            DropIndex("dbo.ResourceRoles", new[] { "Role_Id" });
            DropIndex("dbo.ResourceRoles", new[] { "Resource_Id" });
            DropIndex("dbo.ResourceRoles", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Resources", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Agences", new[] { "Region_Id" });
            DropIndex("dbo.Agences", new[] { "CreatedBy_Id" });
            DropColumn("dbo.AspNetUsers", "Pin");
            DropColumn("dbo.Resources", "CreatedBy_Id");
            DropColumn("dbo.Resources", "IsDeleted");
            DropColumn("dbo.Resources", "ValueAsBytes");
            DropColumn("dbo.Resources", "Type");
            DropColumn("dbo.Resources", "IsForAll");
            DropColumn("dbo.Sections", "CreatedBy_Id");
            DropColumn("dbo.Sections", "IsDeleted");
            DropColumn("dbo.PageRoles", "CreatedBy_Id");
            DropColumn("dbo.PageRoles", "IsDeleted");
            DropColumn("dbo.Pages", "CreatedBy_Id");
            DropColumn("dbo.Pages", "IsDeleted");
            DropColumn("dbo.Devices", "CreatedBy_Id");
            DropColumn("dbo.Devices", "IsDeleted");
            DropTable("dbo.DeclarationDoublePlancher_MotifDp");
            DropTable("dbo.AgenceUsers");
            DropTable("dbo.ResourceUsers");
            DropTable("dbo.ProvenanceDPs");
            DropTable("dbo.MotifNCs");
            DropTable("dbo.Tractions");
            DropTable("dbo.DeclarationRemorqueStatus");
            DropTable("dbo.Remorques");
            DropTable("dbo.MotifDPs");
            DropTable("dbo.DeclarationRemorques");
            DropTable("dbo.Regions");
            DropTable("dbo.ResourceRoles");
            DropTable("dbo.Agences");
        }
    }
}
