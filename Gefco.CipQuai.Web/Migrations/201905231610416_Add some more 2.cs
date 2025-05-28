namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsomemore2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DeclarationBonnePratiques", "CompletionDate");
            DropColumn("dbo.DeclarationNonConformites", "CompletionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeclarationNonConformites", "CompletionDate", c => c.DateTime());
            AddColumn("dbo.DeclarationBonnePratiques", "CompletionDate", c => c.DateTime());
        }
    }
}
