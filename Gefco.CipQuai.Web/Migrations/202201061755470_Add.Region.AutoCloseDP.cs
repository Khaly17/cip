namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegionAutoCloseDP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Regions", "AutoCloseDP", c => c.Boolean(nullable: false, defaultValueSql:"(1)"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Regions", "AutoCloseDP");
        }
    }
}
