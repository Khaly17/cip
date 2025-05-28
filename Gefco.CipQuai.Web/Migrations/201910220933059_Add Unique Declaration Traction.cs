namespace Gefco.CipQuai.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueDeclarationTraction : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format(@"CREATE UNIQUE NONCLUSTERED INDEX {0} ON {1}({2}) WHERE {2} IS NOT NULL;", "IXU_Traction_Id", "dbo.DeclarationRemorques", "Traction_Id"));
        }
        
        public override void Down()
        {
            DropIndex("dbo.DeclarationRemorques", "IXU_Traction_Id");
        }
    }
}
