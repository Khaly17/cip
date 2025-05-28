namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplaceAgenceAgenceTypetoEnumType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgenceTypes",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable:false),
                        Description = c.String(nullable:false),
                    })
                .PrimaryKey(t => t.Key);

            Sql(@"SET IDENTITY_INSERT [dbo].[AgenceTypes] ON 
GO
INSERT [dbo].[AgenceTypes] ([Key], [Value], [Description]) VALUES (1, N'Gefco France', N'Gefco France')
GO
INSERT [dbo].[AgenceTypes] ([Key], [Value], [Description]) VALUES (2, N'International', N'International')
GO
INSERT [dbo].[AgenceTypes] ([Key], [Value], [Description]) VALUES (3, N'Confrères', N'Confrères')
GO
SET IDENTITY_INSERT [dbo].[AgenceTypes] OFF
GO");
            AddColumn("dbo.Agences", "AgenceType_Id", c => c.Int(nullable: false, defaultValueSql:"1"));
            CreateIndex("dbo.Agences", "AgenceType_Id");
            AddForeignKey("dbo.Agences", "AgenceType_Id", "dbo.AgenceTypes", "Key", cascadeDelete: true);
            DropColumn("dbo.Agences", "AgenceType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Agences", "AgenceType", c => c.Int(nullable: false));
            DropForeignKey("dbo.Agences", "AgenceType_Id", "dbo.AgenceTypes");
            DropIndex("dbo.Agences", new[] { "AgenceType_Id" });
            DropColumn("dbo.Agences", "AgenceType_Id");
            DropTable("dbo.AgenceTypes");
        }
    }
}
