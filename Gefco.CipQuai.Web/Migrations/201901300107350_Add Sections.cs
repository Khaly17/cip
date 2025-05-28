namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddSections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sections",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    SortOrder = c.Int(nullable: false),
                    Name = c.String(nullable: false),
                    CreationDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Pages", "Section_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Pages", "Section_Id");
            AddForeignKey("dbo.Pages", "Section_Id", "dbo.Sections", "Id");
            DropColumn("dbo.Pages", "Section");
        }

        public override void Down()
        {
            AddColumn("dbo.Pages", "Section", c => c.String());
            DropForeignKey("dbo.Pages", "Section_Id", "dbo.Sections");
            DropIndex("dbo.Pages", new[] { "Section_Id" });
            DropColumn("dbo.Pages", "Section_Id");
            DropTable("dbo.Sections");
        }
    }
}
