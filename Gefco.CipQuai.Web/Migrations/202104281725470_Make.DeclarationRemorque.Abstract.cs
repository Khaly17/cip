namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDeclarationRemorqueAbstract : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pictures", "DeclarationRemorque_Id", "dbo.DeclarationRemorques");
            AddColumn("dbo.Pictures", "DeclarationRemorque_Id1", c => c.String(maxLength: 128));
            CreateIndex("dbo.Pictures", "DeclarationRemorque_Id1");
            AddForeignKey("dbo.Pictures", "DeclarationRemorque_Id1", "dbo.DeclarationRemorques", "Id");
            Sql("UPDATE dbo.Pictures SET DeclarationRemorque_Id1 = DeclarationRemorque_Id");
            Sql("UPDATE dbo.DeclarationRemorques SET Discriminator = 'DeclarationSimplePlancher' WHERE Discriminator = 'DeclarationRemorque'");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "DeclarationRemorque_Id1", "dbo.DeclarationRemorques");
            DropIndex("dbo.Pictures", new[] { "DeclarationRemorque_Id1" });
            DropColumn("dbo.Pictures", "DeclarationRemorque_Id1");
            AddForeignKey("dbo.Pictures", "DeclarationRemorque_Id", "dbo.DeclarationRemorques", "Id");
        }
    }
}
