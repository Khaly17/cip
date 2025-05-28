namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATETractionDefinitionDaysOfWeekValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TractionDefinitions", "DaysOfWeekValue", c => c.String());
            DropColumn("dbo.TractionDefinitions", "DaysOfWeekInternal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TractionDefinitions", "DaysOfWeekInternal", c => c.String());
            DropColumn("dbo.TractionDefinitions", "DaysOfWeekValue");
        }
    }
}
