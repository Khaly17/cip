namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATEUserAgenceTractionDefinition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AgenceUsers", "Agence_Id", "dbo.Agences");
            DropForeignKey("dbo.AgenceUsers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AgenceUsers", new[] { "Agence_Id" });
            DropIndex("dbo.AgenceUsers", new[] { "User_Id" });
            AddColumn("dbo.AspNetUsers", "MobileUserAgence_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Agence_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.DeclarationRemorques", "CompletionDate", c => c.DateTime());
            AddColumn("dbo.Tractions", "DueDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.AspNetUsers", "MobileUserAgence_Id");
            CreateIndex("dbo.AspNetUsers", "Agence_Id");
            AddForeignKey("dbo.AspNetUsers", "MobileUserAgence_Id", "dbo.Agences", "Id");
            AddForeignKey("dbo.AspNetUsers", "Agence_Id", "dbo.Agences", "Id");
            DropTable("dbo.AgenceUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AgenceUsers",
                c => new
                    {
                        Agence_Id = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Agence_Id, t.User_Id });
            
            DropForeignKey("dbo.AspNetUsers", "Agence_Id", "dbo.Agences");
            DropForeignKey("dbo.AspNetUsers", "MobileUserAgence_Id", "dbo.Agences");
            DropIndex("dbo.AspNetUsers", new[] { "Agence_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "MobileUserAgence_Id" });
            DropColumn("dbo.Tractions", "DueDate");
            DropColumn("dbo.DeclarationRemorques", "CompletionDate");
            DropColumn("dbo.AspNetUsers", "Agence_Id");
            DropColumn("dbo.AspNetUsers", "MobileUserAgence_Id");
            CreateIndex("dbo.AgenceUsers", "User_Id");
            CreateIndex("dbo.AgenceUsers", "Agence_Id");
            AddForeignKey("dbo.AgenceUsers", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AgenceUsers", "Agence_Id", "dbo.Agences", "Id", cascadeDelete: true);
        }
    }
}
