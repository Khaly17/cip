namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgenceUserCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agences", "UserCreated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agences", "UserCreated");
        }
    }
}
