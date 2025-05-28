namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTractionIsCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tractions", "IsCreated", c => c.Boolean(nullable: false, defaultValueSql: "(0)"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tractions", "IsCreated");
        }
    }
}
