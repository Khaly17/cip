namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPageSortOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "SortOrder", c => c.Int(nullable: false, defaultValue: 0));
        }

        public override void Down()
        {
            DropColumn("dbo.Pages", "SortOrder");
        }
    }
}
