namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDevices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        DeviceType = c.Int(nullable: false),
                        DeviceToken = c.String(),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql:"GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Pages", "CreationDate", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.PageRoles", "CreationDate", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Sections", "CreationDate", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sections", "CreationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PageRoles", "CreationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Pages", "CreationDate", c => c.DateTime(nullable: false));
            DropTable("dbo.Devices");
        }
    }
}
