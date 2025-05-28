namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemorqueStatusDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RemorqueStatus", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RemorqueStatus", "Description");
        }
    }
}
