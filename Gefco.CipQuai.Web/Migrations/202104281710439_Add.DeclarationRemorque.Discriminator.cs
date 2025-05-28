namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationRemorqueDiscriminator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclarationRemorques", "IsCr", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeclarationRemorques", "IsCr");
        }
    }
}
