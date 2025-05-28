namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationRemorqueAutreAgenceArrivee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclarationRemorques", "AutreAgenceArrivee", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeclarationRemorques", "AutreAgenceArrivee");
        }
    }
}
