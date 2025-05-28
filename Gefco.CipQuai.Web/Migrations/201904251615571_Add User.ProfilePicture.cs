namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProfilePicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfilePicture_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Pictures", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ProfilePicture_Id");
            CreateIndex("dbo.Pictures", "ApplicationUser_Id");
            AddForeignKey("dbo.Pictures", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "ProfilePicture_Id", "dbo.Pictures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ProfilePicture_Id", "dbo.Pictures");
            DropForeignKey("dbo.Pictures", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Pictures", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ProfilePicture_Id" });
            DropColumn("dbo.Pictures", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "ProfilePicture_Id");
        }
    }
}
