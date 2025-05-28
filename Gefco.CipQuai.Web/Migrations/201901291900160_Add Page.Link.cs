namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPageLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "Icon", c => c.String());
            AddColumn("dbo.Pages", "Link", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Pages", "Link");
            DropColumn("dbo.Pages", "Icon");
        }
    }
}
