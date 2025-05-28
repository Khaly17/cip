namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNationalRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserNationalRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql:"(newid())"),
                        User_Id = c.String(maxLength: 128),
                        NationalRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessRoles", t => t.NationalRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.NationalRole_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNationalRoles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNationalRoles", "NationalRole_Id", "dbo.BusinessRoles");
            DropIndex("dbo.UserNationalRoles", new[] { "NationalRole_Id" });
            DropIndex("dbo.UserNationalRoles", new[] { "User_Id" });
            DropTable("dbo.UserNationalRoles");
        }
    }
}
