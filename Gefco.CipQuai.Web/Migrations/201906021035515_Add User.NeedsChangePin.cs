namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNeedsChangePin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NeedsChangePin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NeedsChangePin");
        }
    }
}
