namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAppVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AppVersion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AppVersion");
        }
    }
}
