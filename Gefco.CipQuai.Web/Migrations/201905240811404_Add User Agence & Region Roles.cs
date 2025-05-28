namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAgenceRegionRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessRoles",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.UserAgenceRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql:"(newid())"),
                        User_Id = c.String(maxLength: 128),
                        AgenceRole_Id = c.Int(nullable: false),
                        Agence_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agences", t => t.Agence_Id, cascadeDelete: true)
                .ForeignKey("dbo.BusinessRoles", t => t.AgenceRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.AgenceRole_Id)
                .Index(t => t.Agence_Id);
            
            CreateTable(
                "dbo.UserRegionRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql:"(newid())"),
                        User_Id = c.String(maxLength: 128),
                        RegionRole_Id = c.Int(nullable: false),
                        Region_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id, cascadeDelete: true)
                .ForeignKey("dbo.BusinessRoles", t => t.RegionRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.RegionRole_Id)
                .Index(t => t.Region_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRegionRoles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserRegionRoles", "RegionRole_Id", "dbo.BusinessRoles");
            DropForeignKey("dbo.UserRegionRoles", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.UserAgenceRoles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserAgenceRoles", "AgenceRole_Id", "dbo.BusinessRoles");
            DropForeignKey("dbo.UserAgenceRoles", "Agence_Id", "dbo.Agences");
            DropIndex("dbo.UserRegionRoles", new[] { "Region_Id" });
            DropIndex("dbo.UserRegionRoles", new[] { "RegionRole_Id" });
            DropIndex("dbo.UserRegionRoles", new[] { "User_Id" });
            DropIndex("dbo.UserAgenceRoles", new[] { "Agence_Id" });
            DropIndex("dbo.UserAgenceRoles", new[] { "AgenceRole_Id" });
            DropIndex("dbo.UserAgenceRoles", new[] { "User_Id" });
            DropTable("dbo.UserRegionRoles");
            DropTable("dbo.UserAgenceRoles");
            DropTable("dbo.BusinessRoles");
        }
    }
}
