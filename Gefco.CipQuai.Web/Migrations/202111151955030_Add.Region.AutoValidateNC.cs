namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegionAutoValidateNC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Regions", "AutoValidateNC", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Regions", "AutoValidateNC");
        }
    }
}
