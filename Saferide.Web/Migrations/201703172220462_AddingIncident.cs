namespace Saferide.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIncident : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        IncidentId = c.Int(nullable: false, identity: true),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IncidentId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Incidents", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Incidents", new[] { "UserId" });
            DropTable("dbo.Incidents");
        }
    }
}
