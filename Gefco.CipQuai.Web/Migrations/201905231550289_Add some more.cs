namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsomemore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Agence_Id", "dbo.Agences");
            DropIndex("dbo.AspNetUsers", new[] { "Agence_Id" });
            DropColumn("dbo.AspNetUsers", "Agence_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Agence_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Agence_Id");
            AddForeignKey("dbo.AspNetUsers", "Agence_Id", "dbo.Agences", "Id");
        }
    }
}
