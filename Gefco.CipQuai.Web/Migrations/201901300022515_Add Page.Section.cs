namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPageSection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "Section", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Pages", "Section");
        }
    }
}
