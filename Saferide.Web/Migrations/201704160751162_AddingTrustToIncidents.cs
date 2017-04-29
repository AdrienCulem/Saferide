namespace Saferide.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTrustToIncidents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incidents", "Trust", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incidents", "Trust");
        }
    }
}
