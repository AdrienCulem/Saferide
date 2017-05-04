namespace Saferide.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incidents", "Street", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incidents", "Street");
        }
    }
}
