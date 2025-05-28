namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPageStory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "Story", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Pages", "Story");
        }
    }
}
