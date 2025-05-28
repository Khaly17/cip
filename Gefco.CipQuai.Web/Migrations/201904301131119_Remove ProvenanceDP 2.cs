namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProvenanceDP2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeclarationNonConformites", "AgenceConcernée_Id", "dbo.Agences");
            DropIndex("dbo.DeclarationNonConformites", new[] { "AgenceConcernée_Id" });
            DropColumn("dbo.DeclarationNonConformites", "AgenceConcernée_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeclarationNonConformites", "AgenceConcernée_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DeclarationNonConformites", "AgenceConcernée_Id");
            AddForeignKey("dbo.DeclarationNonConformites", "AgenceConcernée_Id", "dbo.Agences", "Id");
        }
    }
}
