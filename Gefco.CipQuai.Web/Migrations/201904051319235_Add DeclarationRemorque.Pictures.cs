namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationRemorquePictures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        PicturePath = c.String(),
                        PictureType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                        DeclarationRemorque_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.DeclarationRemorques", t => t.DeclarationRemorque_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.DeclarationRemorque_Id);
            
            DropColumn("dbo.DeclarationRemorques", "HalfLoadPicture");
            DropColumn("dbo.DeclarationRemorques", "FullLoadPicture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeclarationRemorques", "FullLoadPicture", c => c.String());
            AddColumn("dbo.DeclarationRemorques", "HalfLoadPicture", c => c.String());
            DropForeignKey("dbo.Pictures", "DeclarationRemorque_Id", "dbo.DeclarationRemorques");
            DropForeignKey("dbo.Pictures", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Pictures", new[] { "DeclarationRemorque_Id" });
            DropIndex("dbo.Pictures", new[] { "CreatedBy_Id" });
            DropTable("dbo.Pictures");
        }
    }
}
