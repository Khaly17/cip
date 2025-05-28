namespace Gefco.CipQuai.Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddDeclarationNonConformite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeclarationNonConformites",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                    AgenceConcernée_Id = c.String(maxLength: 128),
                    CurrentStatus = c.String(),
                    CompletionDate = c.DateTime(),
                    CurrentWorkflowStep = c.Int(nullable: false),
                    AutreMotifNC = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedBy_Id = c.String(maxLength: 128),
                    CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProvenanceDPs", t => t.AgenceConcernée_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.AgenceConcernée_Id)
                .Index(t => t.CreatedBy_Id);

            AddColumn("dbo.Pictures", "DeclarationNonConformite_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.MotifNCs", "DeclarationNonConformite_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Pictures", "DeclarationNonConformite_Id");
            CreateIndex("dbo.MotifNCs", "DeclarationNonConformite_Id");
            AddForeignKey("dbo.MotifNCs", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites", "Id");
            AddForeignKey("dbo.Pictures", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites");
            DropForeignKey("dbo.MotifNCs", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites");
            DropForeignKey("dbo.DeclarationNonConformites", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationNonConformites", "AgenceConcernée_Id", "dbo.ProvenanceDPs");
            DropIndex("dbo.MotifNCs", new[] { "DeclarationNonConformite_Id" });
            DropIndex("dbo.DeclarationNonConformites", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationNonConformites", new[] { "AgenceConcernée_Id" });
            DropIndex("dbo.Pictures", new[] { "DeclarationNonConformite_Id" });
            DropColumn("dbo.MotifNCs", "DeclarationNonConformite_Id");
            DropColumn("dbo.Pictures", "DeclarationNonConformite_Id");
            DropTable("dbo.DeclarationNonConformites");
        }
    }
}
