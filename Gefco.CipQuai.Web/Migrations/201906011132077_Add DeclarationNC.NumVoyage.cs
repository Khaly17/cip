namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationNCNumVoyage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclarationNonConformites", "NumVoyage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeclarationNonConformites", "NumVoyage");
        }
    }
}
