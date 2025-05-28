namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dunno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tractions", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tractions", "CancelReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tractions", "CancelReason");
            DropColumn("dbo.Tractions", "IsCancelled");
        }
    }
}
