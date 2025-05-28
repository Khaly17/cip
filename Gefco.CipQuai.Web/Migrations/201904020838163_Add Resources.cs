namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResources : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        Key = c.String(),
                        Value = c.String(),
                        LastUpdate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Resources");
        }
    }
}
