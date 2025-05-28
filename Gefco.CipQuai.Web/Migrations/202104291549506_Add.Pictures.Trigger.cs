namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPicturesTrigger : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Pictures SET DeclarationRemorque_Id1 = DeclarationRemorque_Id");
            Sql("CREATE TRIGGER UpdatePictureId ON dbo.Pictures AFTER INSERT AS BEGIN SET NOCOUNT ON; UPDATE Pictures SET DeclarationRemorque_Id1 = DeclarationRemorque_Id END");
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER IF EXISTS UpdatePictureId");
        }
    }
}
