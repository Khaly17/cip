namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATEUserAgenceTractionDefinition1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TractionDefinitions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
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
            
            AlterColumn("dbo.DeclarationRemorques", "CompletionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TractionDefinitions", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TractionDefinitions", "AgenceDepart_Id", "dbo.Agences");
            DropForeignKey("dbo.TractionDefinitions", "AgenceArrivee_Id", "dbo.Agences");
            DropIndex("dbo.TractionDefinitions", new[] { "CreatedBy_Id" });
            DropIndex("dbo.TractionDefinitions", new[] { "AgenceDepart_Id" });
            DropIndex("dbo.TractionDefinitions", new[] { "AgenceArrivee_Id" });
            AlterColumn("dbo.DeclarationRemorques", "CompletionDate", c => c.DateTime(nullable: false));
            DropTable("dbo.TractionDefinitions");
        }
    }
}
