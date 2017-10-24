namespace OopRestaurant201710.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLocationdatabasetableandTableLocationproperty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsOutdoor = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tables", "Location_Id", c => c.Int());
            CreateIndex("dbo.Tables", "Location_Id");
            AddForeignKey("dbo.Tables", "Location_Id", "dbo.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Tables", new[] { "Location_Id" });
            DropColumn("dbo.Tables", "Location_Id");
            DropTable("dbo.Locations");
        }
    }
}
