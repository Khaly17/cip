namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeclarationRemorquesStatuses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeclarationRemorques", "Status_Id", "dbo.DeclarationRemorqueStatus");
            DropIndex("dbo.DeclarationRemorques", new[] { "Status_Id" });
            CreateTable(
                "dbo.RemorqueStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            AddColumn("dbo.DeclarationRemorques", "CurrentStatus_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.DeclarationRemorqueStatus", "DeclarationRemorque_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.DeclarationRemorqueStatus", "RemorqueStatus_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DeclarationRemorques", "CurrentStatus_Id");
            CreateIndex("dbo.DeclarationRemorqueStatus", "DeclarationRemorque_Id");
            CreateIndex("dbo.DeclarationRemorqueStatus", "RemorqueStatus_Id");
            AddForeignKey("dbo.DeclarationRemorques", "CurrentStatus_Id", "dbo.RemorqueStatus", "Id");
            AddForeignKey("dbo.DeclarationRemorqueStatus", "DeclarationRemorque_Id", "dbo.DeclarationRemorques", "Id");
            AddForeignKey("dbo.DeclarationRemorqueStatus", "RemorqueStatus_Id", "dbo.RemorqueStatus", "Id");
            DropColumn("dbo.DeclarationRemorques", "Status_Id");
            DropColumn("dbo.DeclarationRemorqueStatus", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeclarationRemorqueStatus", "Name", c => c.String(nullable: false));
            AddColumn("dbo.DeclarationRemorques", "Status_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.DeclarationRemorqueStatus", "RemorqueStatus_Id", "dbo.RemorqueStatus");
            DropForeignKey("dbo.DeclarationRemorqueStatus", "DeclarationRemorque_Id", "dbo.DeclarationRemorques");
            DropForeignKey("dbo.DeclarationRemorques", "CurrentStatus_Id", "dbo.RemorqueStatus");
            DropForeignKey("dbo.RemorqueStatus", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DeclarationRemorqueStatus", new[] { "RemorqueStatus_Id" });
            DropIndex("dbo.DeclarationRemorqueStatus", new[] { "DeclarationRemorque_Id" });
            DropIndex("dbo.RemorqueStatus", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationRemorques", new[] { "CurrentStatus_Id" });
            DropColumn("dbo.DeclarationRemorqueStatus", "RemorqueStatus_Id");
            DropColumn("dbo.DeclarationRemorqueStatus", "DeclarationRemorque_Id");
            DropColumn("dbo.DeclarationRemorques", "CurrentStatus_Id");
            DropTable("dbo.RemorqueStatus");
            CreateIndex("dbo.DeclarationRemorques", "Status_Id");
            AddForeignKey("dbo.DeclarationRemorques", "Status_Id", "dbo.DeclarationRemorqueStatus", "Id");
        }
    }
}
