namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMotifsDisplayOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclarationRemorques", "AutreMotifDP", c => c.String());
            AddColumn("dbo.MotifDPs", "DisplayOrder", c => c.Int(nullable: false));
            AddColumn("dbo.MotifNCs", "DisplayOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MotifNCs", "DisplayOrder");
            DropColumn("dbo.MotifDPs", "DisplayOrder");
            DropColumn("dbo.DeclarationRemorques", "AutreMotifDP");
        }
    }
}
