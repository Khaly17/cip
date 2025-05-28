namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATETractionTractionDefinition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclarationRemorques", "HalfLoadPicture", c => c.String());
            AddColumn("dbo.DeclarationRemorques", "FullLoadPicture", c => c.String());
            AddColumn("dbo.Tractions", "TractionDefinition_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tractions", "TractionDefinition_Id");
            AddForeignKey("dbo.Tractions", "TractionDefinition_Id", "dbo.TractionDefinitions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tractions", "TractionDefinition_Id", "dbo.TractionDefinitions");
            DropIndex("dbo.Tractions", new[] { "TractionDefinition_Id" });
            DropColumn("dbo.Tractions", "TractionDefinition_Id");
            DropColumn("dbo.DeclarationRemorques", "FullLoadPicture");
            DropColumn("dbo.DeclarationRemorques", "HalfLoadPicture");
        }
    }
}
