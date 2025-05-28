namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConfiguration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        Value = c.String(),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            AddColumn("dbo.DeclarationRemorques", "CurrentWorkflowStep", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Configurations", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Configurations", new[] { "CreatedBy_Id" });
            DropColumn("dbo.DeclarationRemorques", "CurrentWorkflowStep");
            DropTable("dbo.Configurations");
        }
    }
}
