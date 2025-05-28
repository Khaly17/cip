namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProvenanceDP : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeclarationNonConformites", "AgenceConcernée_Id", "dbo.ProvenanceDPs");
            DropForeignKey("dbo.ProvenanceDPs", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProvenanceDPs", new[] { "CreatedBy_Id" });
            AddColumn("dbo.Agences", "AgenceType", c => c.Int(nullable: false));
            AddColumn("dbo.Agences", "OtherName", c => c.String());
            DropTable("dbo.ProvenanceDPs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProvenanceDPs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsGefcoFrance = c.Boolean(nullable: false),
                        IsInternational = c.Boolean(nullable: false),
                        IsClient = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Agences", "OtherName");
            DropColumn("dbo.Agences", "AgenceType");
            CreateIndex("dbo.ProvenanceDPs", "CreatedBy_Id");
            AddForeignKey("dbo.ProvenanceDPs", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
