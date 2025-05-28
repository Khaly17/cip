namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPagesPageRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pages",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                    Name = c.String(nullable: false),
                    CreationDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AspNetRoles", "Page_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetRoles", "Page_Id");
            AddForeignKey("dbo.AspNetRoles", "Page_Id", "dbo.Pages", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetRoles", "Page_Id", "dbo.Pages");
            DropIndex("dbo.AspNetRoles", new[] { "Page_Id" });
            DropColumn("dbo.AspNetRoles", "Page_Id");
            DropTable("dbo.Pages");
        }
    }
}
