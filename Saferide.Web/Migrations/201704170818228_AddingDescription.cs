namespace Saferide.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incidents", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incidents", "Description");
        }
    }
}
