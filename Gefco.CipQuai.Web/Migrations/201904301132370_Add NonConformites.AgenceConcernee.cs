namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNonConformitesAgenceConcernee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeclarationNonConformites", "AgenceConcernée_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DeclarationNonConformites", "AgenceConcernée_Id");
            AddForeignKey("dbo.DeclarationNonConformites", "AgenceConcernée_Id", "dbo.Agences", "Id");
            Sql(@"
  UPDATE [Gefco.CipQuai.Web].[dbo].[DeclarationNonConformites] SET AgenceConcernée_Id = a.Id
  FROM [Gefco.CipQuai.Web].[dbo].[DeclarationNonConformites] nc
  INNER join dbo.Agences a on nc.AgenceConcernee = a.Name");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeclarationNonConformites", "AgenceConcernée_Id", "dbo.Agences");
            DropIndex("dbo.DeclarationNonConformites", new[] { "AgenceConcernée_Id" });
            DropColumn("dbo.DeclarationNonConformites", "AgenceConcernée_Id");
        }
    }
}
