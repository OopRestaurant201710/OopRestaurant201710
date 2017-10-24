namespace OopRestaurant201710.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvalidationtoTableandLocation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tables", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Tables", new[] { "Location_Id" });
            AlterColumn("dbo.Tables", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Tables", "Location_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Locations", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Tables", "Location_Id");
            AddForeignKey("dbo.Tables", "Location_Id", "dbo.Locations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Tables", new[] { "Location_Id" });
            AlterColumn("dbo.Locations", "Name", c => c.String());
            AlterColumn("dbo.Tables", "Location_Id", c => c.Int());
            AlterColumn("dbo.Tables", "Name", c => c.String());
            CreateIndex("dbo.Tables", "Location_Id");
            AddForeignKey("dbo.Tables", "Location_Id", "dbo.Locations", "Id");
        }
    }
}
