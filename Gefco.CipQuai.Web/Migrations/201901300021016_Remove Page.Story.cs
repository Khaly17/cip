namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RemovePageStory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pages", "Story");
        }

        public override void Down()
        {
            AddColumn("dbo.Pages", "Story", c => c.String());
        }
    }
}
