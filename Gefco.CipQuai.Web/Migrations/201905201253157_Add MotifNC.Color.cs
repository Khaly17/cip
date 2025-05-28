namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMotifNCColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MotifNCs", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MotifNCs", "Color");
        }
    }
}
