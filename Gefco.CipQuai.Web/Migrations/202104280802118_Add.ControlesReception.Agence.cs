namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddControlesReceptionAgence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclarationRemorques", "AgenceCreationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DeclarationRemorques", "AgenceCreationId");
            AddForeignKey("dbo.DeclarationRemorques", "AgenceCreationId", "dbo.Agences", "Id");
            DropColumn("dbo.DeclarationRemorques", "IsCr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeclarationRemorques", "IsCr", c => c.Boolean());
            DropForeignKey("dbo.DeclarationRemorques", "AgenceCreationId", "dbo.Agences");
            DropIndex("dbo.DeclarationRemorques", new[] { "AgenceCreationId" });
            DropColumn("dbo.DeclarationRemorques", "AgenceCreationId");
        }
    }
}
