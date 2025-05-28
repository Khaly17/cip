namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNCBPStatuses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeclarationBonnePratiques", "CurrentStatus_Id", "dbo.BonnePratiqueStatus");
            DropForeignKey("dbo.DeclarationBonnePratiqueStatus", "BonnePratiqueStatus_Key", "dbo.BonnePratiqueStatus");
            DropForeignKey("dbo.DeclarationBonnePratiqueStatus", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationBonnePratiqueStatus", "DeclarationBonnePratique_Id", "dbo.DeclarationBonnePratiques");
            DropForeignKey("dbo.DeclarationNonConformites", "CurrentStatus_Id", "dbo.NonConformiteStatus");
            DropForeignKey("dbo.DeclarationNonConformiteStatus", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationNonConformiteStatus", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites");
            DropForeignKey("dbo.DeclarationNonConformiteStatus", "NonConformiteStatus_Key", "dbo.NonConformiteStatus");
            DropIndex("dbo.DeclarationBonnePratiques", new[] { "CurrentStatus_Id" });
            DropIndex("dbo.DeclarationBonnePratiqueStatus", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationBonnePratiqueStatus", new[] { "BonnePratiqueStatus_Key" });
            DropIndex("dbo.DeclarationBonnePratiqueStatus", new[] { "DeclarationBonnePratique_Id" });
            DropIndex("dbo.DeclarationNonConformites", new[] { "CurrentStatus_Id" });
            DropIndex("dbo.DeclarationNonConformiteStatus", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationNonConformiteStatus", new[] { "DeclarationNonConformite_Id" });
            DropIndex("dbo.DeclarationNonConformiteStatus", new[] { "NonConformiteStatus_Key" });
            AddColumn("dbo.AspNetUsers", "WebUserAgence_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "WebUserAgence_Id");
            AddForeignKey("dbo.AspNetUsers", "WebUserAgence_Id", "dbo.Agences", "Id");
            DropColumn("dbo.DeclarationBonnePratiques", "CurrentStatus_Id");
            DropColumn("dbo.DeclarationNonConformites", "CurrentStatus_Id");
            DropTable("dbo.BonnePratiqueStatus");
            DropTable("dbo.DeclarationBonnePratiqueStatus");
            DropTable("dbo.NonConformiteStatus");
            DropTable("dbo.DeclarationNonConformiteStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DeclarationNonConformiteStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        DeclarationNonConformite_Id = c.String(maxLength: 128),
                        NonConformiteStatus_Key = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NonConformiteStatus",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.DeclarationBonnePratiqueStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                        BonnePratiqueStatus_Key = c.Int(),
                        DeclarationBonnePratique_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BonnePratiqueStatus",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            AddColumn("dbo.DeclarationNonConformites", "CurrentStatus_Id", c => c.Int(nullable: false));
            AddColumn("dbo.DeclarationBonnePratiques", "CurrentStatus_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.AspNetUsers", "WebUserAgence_Id", "dbo.Agences");
            DropIndex("dbo.AspNetUsers", new[] { "WebUserAgence_Id" });
            DropColumn("dbo.AspNetUsers", "WebUserAgence_Id");
            CreateIndex("dbo.DeclarationNonConformiteStatus", "NonConformiteStatus_Key");
            CreateIndex("dbo.DeclarationNonConformiteStatus", "DeclarationNonConformite_Id");
            CreateIndex("dbo.DeclarationNonConformiteStatus", "CreatedBy_Id");
            CreateIndex("dbo.DeclarationNonConformites", "CurrentStatus_Id");
            CreateIndex("dbo.DeclarationBonnePratiqueStatus", "DeclarationBonnePratique_Id");
            CreateIndex("dbo.DeclarationBonnePratiqueStatus", "BonnePratiqueStatus_Key");
            CreateIndex("dbo.DeclarationBonnePratiqueStatus", "CreatedBy_Id");
            CreateIndex("dbo.DeclarationBonnePratiques", "CurrentStatus_Id");
            AddForeignKey("dbo.DeclarationNonConformiteStatus", "NonConformiteStatus_Key", "dbo.NonConformiteStatus", "Key");
            AddForeignKey("dbo.DeclarationNonConformiteStatus", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites", "Id");
            AddForeignKey("dbo.DeclarationNonConformiteStatus", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DeclarationNonConformites", "CurrentStatus_Id", "dbo.NonConformiteStatus", "Key", cascadeDelete: true);
            AddForeignKey("dbo.DeclarationBonnePratiqueStatus", "DeclarationBonnePratique_Id", "dbo.DeclarationBonnePratiques", "Id");
            AddForeignKey("dbo.DeclarationBonnePratiqueStatus", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DeclarationBonnePratiqueStatus", "BonnePratiqueStatus_Key", "dbo.BonnePratiqueStatus", "Key");
            AddForeignKey("dbo.DeclarationBonnePratiques", "CurrentStatus_Id", "dbo.BonnePratiqueStatus", "Key", cascadeDelete: true);
        }
    }
}
