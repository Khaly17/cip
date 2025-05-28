namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgenceIsProvenanceNC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agences", "IsProvenanceNC", c => c.Boolean(nullable: false));
            DropColumn("dbo.Agences", "IsProvenanceDP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Agences", "IsProvenanceDP", c => c.Boolean(nullable: false));
            DropColumn("dbo.Agences", "IsProvenanceNC");
        }
    }
}
