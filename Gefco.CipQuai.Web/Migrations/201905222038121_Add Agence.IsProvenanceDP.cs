namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgenceIsProvenanceDP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agences", "IsProvenanceDP", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agences", "IsProvenanceDP");
        }
    }
}
