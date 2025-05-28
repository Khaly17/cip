namespace Gefco.CipQuai.Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddDeclarationBonnePratique : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeclarationBonnePratiques",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                    AgentConcerné_Id = c.String(maxLength: 128),
                    AutreAgentConcerné = c.String(),
                    CurrentStatus = c.String(),
                    CompletionDate = c.DateTime(),
                    CurrentWorkflowStep = c.Int(nullable: false),
                    Description = c.String(),
                    IsDeleted = c.Boolean(nullable: false, defaultValueSql: "0"),
                    CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AgentConcerné_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.AgentConcerné_Id)
                .Index(t => t.CreatedBy_Id);

            AddColumn("dbo.Pictures", "DeclarationBonnePratique_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Pictures", "DeclarationBonnePratique_Id");
            AddForeignKey("dbo.Pictures", "DeclarationBonnePratique_Id", "dbo.DeclarationBonnePratiques", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "DeclarationBonnePratique_Id", "dbo.DeclarationBonnePratiques");
            DropForeignKey("dbo.DeclarationBonnePratiques", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationBonnePratiques", "AgentConcerné_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Pictures", new[] { "DeclarationBonnePratique_Id" });
            DropIndex("dbo.DeclarationBonnePratiques", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationBonnePratiques", new[] { "AgentConcerné_Id" });
            DropColumn("dbo.Pictures", "DeclarationBonnePratique_Id");
            DropTable("dbo.DeclarationBonnePratiques");
        }
    }
}
