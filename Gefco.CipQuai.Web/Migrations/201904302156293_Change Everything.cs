namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEverything : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BonnePratiqueStatus",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            Sql(@"SET IDENTITY_INSERT [dbo].[BonnePratiqueStatus] ON 
GO
INSERT [dbo].[BonnePratiqueStatus] ([Key], [Value], [Description]) VALUES (1, N'InProgress', N'In progress')
GO
INSERT [dbo].[BonnePratiqueStatus] ([Key], [Value], [Description]) VALUES (2, N'PausedAndFree', N'Paused and free')
GO
INSERT [dbo].[BonnePratiqueStatus] ([Key], [Value], [Description]) VALUES (3, N'PausedAndLocked', N'Paused and locked')
GO
INSERT [dbo].[BonnePratiqueStatus] ([Key], [Value], [Description]) VALUES (4, N'ToBeValidated', N'To be validated')
GO
INSERT [dbo].[BonnePratiqueStatus] ([Key], [Value], [Description]) VALUES (5, N'ToJustify', N'To justify')
GO
INSERT [dbo].[BonnePratiqueStatus] ([Key], [Value], [Description]) VALUES (6, N'NotValid', N'Not Valid')
GO
INSERT [dbo].[BonnePratiqueStatus] ([Key], [Value], [Description]) VALUES (7, N'Valid', N'Valide')
GO
SET IDENTITY_INSERT [dbo].[BonnePratiqueStatus] OFF
GO");
            CreateTable(
                "dbo.DeclarationBonnePratiqueStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        BonnePratiqueStatus_Key = c.Int(),
                        DeclarationBonnePratique_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BonnePratiqueStatus", t => t.BonnePratiqueStatus_Key)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.DeclarationBonnePratiques", t => t.DeclarationBonnePratique_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.BonnePratiqueStatus_Key)
                .Index(t => t.DeclarationBonnePratique_Id);
            
            CreateTable(
                "dbo.NonConformiteStatus",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            Sql(@"SET IDENTITY_INSERT [dbo].[NonConformiteStatus] ON 
GO
INSERT [dbo].[NonConformiteStatus] ([Key], [Value], [Description]) VALUES (1, N'InProgress', N'In progress')
GO
INSERT [dbo].[NonConformiteStatus] ([Key], [Value], [Description]) VALUES (2, N'PausedAndFree', N'Paused and free')
GO
INSERT [dbo].[NonConformiteStatus] ([Key], [Value], [Description]) VALUES (3, N'PausedAndLocked', N'Paused and locked')
GO
INSERT [dbo].[NonConformiteStatus] ([Key], [Value], [Description]) VALUES (4, N'ToBeValidated', N'To be validated')
GO
INSERT [dbo].[NonConformiteStatus] ([Key], [Value], [Description]) VALUES (5, N'ToJustify', N'To justify')
GO
INSERT [dbo].[NonConformiteStatus] ([Key], [Value], [Description]) VALUES (6, N'NotValid', N'Not Valid')
GO
INSERT [dbo].[NonConformiteStatus] ([Key], [Value], [Description]) VALUES (7, N'Valid', N'Valide')
GO
SET IDENTITY_INSERT [dbo].[NonConformiteStatus] OFF
GO");
            CreateTable(
                "dbo.DeclarationNonConformiteStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, defaultValueSql: "convert(nvarchar(128), newid())"),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        DeclarationNonConformite_Id = c.String(maxLength: 128),
                        NonConformiteStatus_Key = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.DeclarationNonConformites", t => t.DeclarationNonConformite_Id)
                .ForeignKey("dbo.NonConformiteStatus", t => t.NonConformiteStatus_Key)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.DeclarationNonConformite_Id)
                .Index(t => t.NonConformiteStatus_Key);

            AddColumn("dbo.DeclarationBonnePratiques", "CurrentStatus_Id", c => c.Int(nullable: false));

            Sql(@"UPDATE dbo.DeclarationBonnePratiques SET CurrentStatus_Id = 1 WHERE CurrentStatus = 'InProgress'");
            Sql(@"UPDATE dbo.DeclarationBonnePratiques SET CurrentStatus_Id = 1 WHERE CurrentStatus = 'Created'");
            Sql(@"UPDATE dbo.DeclarationBonnePratiques SET CurrentStatus_Id = 4 WHERE CurrentStatus = 'ToBeValidated'");

            AddColumn("dbo.DeclarationNonConformites", "CurrentStatus_Id", c => c.Int(nullable: false));

            Sql(@"UPDATE dbo.DeclarationNonConformites SET CurrentStatus_Id = 1 WHERE CurrentStatus = 'InProgress'");
            Sql(@"UPDATE dbo.DeclarationNonConformites SET CurrentStatus_Id = 1 WHERE CurrentStatus = 'Created'");
            Sql(@"UPDATE dbo.DeclarationNonConformites SET CurrentStatus_Id = 4 WHERE CurrentStatus = 'ToBeValidated'");

            CreateIndex("dbo.DeclarationBonnePratiques", "CurrentStatus_Id");
            CreateIndex("dbo.DeclarationNonConformites", "CurrentStatus_Id");
            AddForeignKey("dbo.DeclarationBonnePratiques", "CurrentStatus_Id", "dbo.BonnePratiqueStatus", "Key", cascadeDelete: true);
            AddForeignKey("dbo.DeclarationNonConformites", "CurrentStatus_Id", "dbo.NonConformiteStatus", "Key", cascadeDelete: true);
            DropColumn("dbo.DeclarationBonnePratiques", "CurrentStatus");
            DropColumn("dbo.DeclarationNonConformites", "CurrentStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeclarationNonConformites", "CurrentStatus", c => c.String());
            AddColumn("dbo.DeclarationBonnePratiques", "CurrentStatus", c => c.String());
            DropForeignKey("dbo.DeclarationNonConformiteStatus", "NonConformiteStatus_Key", "dbo.NonConformiteStatus");
            DropForeignKey("dbo.DeclarationNonConformiteStatus", "DeclarationNonConformite_Id", "dbo.DeclarationNonConformites");
            DropForeignKey("dbo.DeclarationNonConformiteStatus", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationNonConformites", "CurrentStatus_Id", "dbo.NonConformiteStatus");
            DropForeignKey("dbo.DeclarationBonnePratiqueStatus", "DeclarationBonnePratique_Id", "dbo.DeclarationBonnePratiques");
            DropForeignKey("dbo.DeclarationBonnePratiqueStatus", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeclarationBonnePratiqueStatus", "BonnePratiqueStatus_Key", "dbo.BonnePratiqueStatus");
            DropForeignKey("dbo.DeclarationBonnePratiques", "CurrentStatus_Id", "dbo.BonnePratiqueStatus");
            DropIndex("dbo.DeclarationNonConformiteStatus", new[] { "NonConformiteStatus_Key" });
            DropIndex("dbo.DeclarationNonConformiteStatus", new[] { "DeclarationNonConformite_Id" });
            DropIndex("dbo.DeclarationNonConformiteStatus", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationNonConformites", new[] { "CurrentStatus_Id" });
            DropIndex("dbo.DeclarationBonnePratiqueStatus", new[] { "DeclarationBonnePratique_Id" });
            DropIndex("dbo.DeclarationBonnePratiqueStatus", new[] { "BonnePratiqueStatus_Key" });
            DropIndex("dbo.DeclarationBonnePratiqueStatus", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeclarationBonnePratiques", new[] { "CurrentStatus_Id" });
            DropColumn("dbo.DeclarationNonConformites", "CurrentStatus_Id");
            DropColumn("dbo.DeclarationBonnePratiques", "CurrentStatus_Id");
            DropColumn("dbo.AgenceTypes", "Description");
            DropTable("dbo.DeclarationNonConformiteStatus");
            DropTable("dbo.NonConformiteStatus");
            DropTable("dbo.DeclarationBonnePratiqueStatus");
            DropTable("dbo.BonnePratiqueStatus");
        }
    }
}
