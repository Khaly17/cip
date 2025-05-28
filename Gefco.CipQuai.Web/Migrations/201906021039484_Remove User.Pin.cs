namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserPin : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Pin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Pin", c => c.String());
        }
    }
}
