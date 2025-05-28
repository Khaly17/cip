namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConfigurationDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "Description", c => c.String());
            AddColumn("dbo.Configurations", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Configurations", "Description");
            DropColumn("dbo.Resources", "Description");
        }
    }
}
