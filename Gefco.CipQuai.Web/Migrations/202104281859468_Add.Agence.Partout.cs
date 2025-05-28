namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgencePartout : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DeclarationRemorques", name: "AgenceCreationId", newName: "AgenceId");
            RenameIndex(table: "dbo.DeclarationRemorques", name: "IX_AgenceCreationId", newName: "IX_AgenceId");
            AddColumn("dbo.DeclarationBonnePratiques", "AgenceId", c => c.String(maxLength: 128));
            AddColumn("dbo.DeclarationNonConformites", "AgenceId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DeclarationBonnePratiques", "AgenceId");
            CreateIndex("dbo.DeclarationNonConformites", "AgenceId");
            AddForeignKey("dbo.DeclarationBonnePratiques", "AgenceId", "dbo.Agences", "Id");
            AddForeignKey("dbo.DeclarationNonConformites", "AgenceId", "dbo.Agences", "Id");
            Sql("UPDATE DeclarationRemorques SET AgenceId = a.MobileUserAgence_id FROM DeclarationRemorques d inner join AspNetUsers a on d.CreatedBy_Id = a.Id");
            Sql("UPDATE DeclarationBonnePratiques SET AgenceId = a.MobileUserAgence_id FROM DeclarationBonnePratiques d inner join AspNetUsers a on d.CreatedBy_Id = a.Id");
            Sql("UPDATE DeclarationNonConformites SET AgenceId = a.MobileUserAgence_id FROM DeclarationNonConformites d inner join AspNetUsers a on d.CreatedBy_Id = a.Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeclarationNonConformites", "AgenceId", "dbo.Agences");
            DropForeignKey("dbo.DeclarationBonnePratiques", "AgenceId", "dbo.Agences");
            DropIndex("dbo.DeclarationNonConformites", new[] { "AgenceId" });
            DropIndex("dbo.DeclarationBonnePratiques", new[] { "AgenceId" });
            DropColumn("dbo.DeclarationNonConformites", "AgenceId");
            DropColumn("dbo.DeclarationBonnePratiques", "AgenceId");
            RenameIndex(table: "dbo.DeclarationRemorques", name: "IX_AgenceId", newName: "IX_AgenceCreationId");
            RenameColumn(table: "dbo.DeclarationRemorques", name: "AgenceId", newName: "AgenceCreationId");
        }
    }
}
