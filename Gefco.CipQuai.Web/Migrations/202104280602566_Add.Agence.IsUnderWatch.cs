namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgenceIsUnderWatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agences", "IsUnderWatch", c => c.Boolean(nullable: false, defaultValueSql:"(0)"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agences", "IsUnderWatch");
        }
    }
}
