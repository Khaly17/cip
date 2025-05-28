namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEverything2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MotifNCs", "IsOther", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MotifNCs", "IsOther");
        }
    }
}
