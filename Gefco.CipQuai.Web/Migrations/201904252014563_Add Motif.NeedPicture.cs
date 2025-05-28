namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMotifNeedPicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MotifDPs", "NeedPicture", c => c.Boolean(nullable: false, defaultValueSql:"(1)"));
            AddColumn("dbo.MotifDPs", "IsNbDP", c => c.Boolean(nullable: false, defaultValueSql: "(0)"));
            AddColumn("dbo.MotifDPs", "IsOther", c => c.Boolean(nullable: false, defaultValueSql: "(0)"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MotifDPs", "IsOther");
            DropColumn("dbo.MotifDPs", "IsNbDP");
            DropColumn("dbo.MotifDPs", "NeedPicture");
        }
    }
}
