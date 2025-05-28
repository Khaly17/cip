namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPageRoles : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetRoles", new[] { "Page_Id" });
            DropForeignKey("dbo.Page", "Page_Id", "dbo.AspNetRoles");
            CreateTable(
                "dbo.PageRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    CreationDate = c.DateTime(nullable: false),
                    Page_Id = c.String(maxLength: 128),
                    Role_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id)
                .ForeignKey("dbo.Pages", t => t.Page_Id)
                .Index(t => t.Page_Id)
                .Index(t => t.Role_Id);

            //DropColumn("dbo.AspNetRoles", "Page_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Page_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PageRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.PageRoles", "Page_Id", "dbo.Pages");
            DropIndex("dbo.PageRoles", new[] { "Role_Id" });
            DropIndex("dbo.PageRoles", new[] { "Page_Id" });
            DropTable("dbo.PageRoles");
            CreateIndex("dbo.AspNetRoles", "Page_Id");
        }
    }
}
