namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTempPictures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPictures",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PicturePath = c.String(),
                        PictureType = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(),
                        DeclarationRemorque_Id = c.String(),
                        DeclarationControleReception_Id = c.String(),
                        DeclarationBonnePratique_Id = c.String(),
                        DeclarationNonConformite_Id = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempPictures", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TempPictures", new[] { "CreatedBy_Id" });
            DropTable("dbo.TempPictures");
        }
    }
}
