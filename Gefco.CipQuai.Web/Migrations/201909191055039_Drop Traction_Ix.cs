namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropTraction_Ix : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DeclarationRemorques", new[] { "Traction_Id" });
        }

        public override void Down()
        {
        }
    }
}
