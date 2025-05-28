namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATETractionDefinitionDaysOfWeek : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TractionDefinitions", "DaysOfWeekInternal", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TractionDefinitions", "DaysOfWeekInternal");
        }
    }
}
