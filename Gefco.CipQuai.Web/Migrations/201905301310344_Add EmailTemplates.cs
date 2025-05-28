namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailTemplates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                    Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "(newid())"),
                    Object = c.String(),
                        Content = c.String(),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                    CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailTemplates", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.EmailTemplates", new[] { "CreatedBy_Id" });
            DropTable("dbo.EmailTemplates");
        }
    }
}
