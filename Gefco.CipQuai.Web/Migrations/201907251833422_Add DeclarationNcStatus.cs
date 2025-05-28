namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationNcStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeclarationNcStatus",
                c => new
                    {
                    Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "(newid())"),
                    Description = c.String(),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                    CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            AddColumn("dbo.DeclarationNonConformites", "CurrentStatus_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DeclarationNonConformites", "CurrentStatus_Id");
            AddForeignKey("dbo.DeclarationNonConformites", "CurrentStatus_Id", "dbo.DeclarationNcStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeclarationNonConformites", "CurrentStatus_Id", "dbo.DeclarationNcStatus");
            DropForeignKey("dbo.DeclarationNcStatus", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DeclarationNcStatus", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationNonConformites", new[] { "CurrentStatus_Id" });
            DropColumn("dbo.DeclarationNonConformites", "CurrentStatus_Id");
            DropTable("dbo.DeclarationNcStatus");
        }
    }
}
