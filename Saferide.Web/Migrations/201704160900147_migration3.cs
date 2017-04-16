namespace Saferide.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Incidents", "Trust", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Incidents", "Trust", c => c.Int(nullable: false));
        }
    }
}
