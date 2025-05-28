namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationNonConformite_MotifNC : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MotifNCs", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites");
            DropIndex("dbo.MotifNCs", new[] { "DeclarationNonConformite_Id" });
            CreateTable(
                "dbo.DeclarationNonConformite_MotifNC",
                c => new
                    {
                        DeclarationNonConformite_Id = c.String(nullable: false, maxLength: 128),
                        MotifNC_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DeclarationNonConformite_Id, t.MotifNC_Id })
                .ForeignKey("dbo.DeclarationNonConformites", t => t.DeclarationNonConformite_Id, cascadeDelete: true)
                .ForeignKey("dbo.MotifNCs", t => t.MotifNC_Id, cascadeDelete: true)
                .Index(t => t.DeclarationNonConformite_Id)
                .Index(t => t.MotifNC_Id);
            
            DropColumn("dbo.MotifNCs", "DeclarationNonConformite_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MotifNCs", "DeclarationNonConformite_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.DeclarationNonConformite_MotifNC", "MotifNC_Id", "dbo.MotifNCs");
            DropForeignKey("dbo.DeclarationNonConformite_MotifNC", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites");
            DropIndex("dbo.DeclarationNonConformite_MotifNC", new[] { "MotifNC_Id" });
            DropIndex("dbo.DeclarationNonConformite_MotifNC", new[] { "DeclarationNonConformite_Id" });
            DropTable("dbo.DeclarationNonConformite_MotifNC");
            CreateIndex("dbo.MotifNCs", "DeclarationNonConformite_Id");
            AddForeignKey("dbo.MotifNCs", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites", "Id");
        }
    }
}
