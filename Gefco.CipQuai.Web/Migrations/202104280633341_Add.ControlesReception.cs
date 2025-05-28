namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddControlesReception : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "DeclarationControleReception_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Pictures", "DeclarationControleReception_Id");
            AddForeignKey("dbo.Pictures", "DeclarationControleReception_Id", "dbo.DeclarationRemorques", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "DeclarationControleReception_Id", "dbo.DeclarationRemorques");
            DropIndex("dbo.Pictures", new[] { "DeclarationControleReception_Id" });
            DropColumn("dbo.Pictures", "DeclarationControleReception_Id");
        }
    }
}
