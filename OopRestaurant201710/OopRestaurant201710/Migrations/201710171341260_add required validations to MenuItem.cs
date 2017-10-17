namespace OopRestaurant201710.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrequiredvalidationstoMenuItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories");
            DropIndex("dbo.MenuItems", new[] { "Category_Id" });
            AlterColumn("dbo.MenuItems", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.MenuItems", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.MenuItems", "Category_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.MenuItems", "Category_Id");
            AddForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories");
            DropIndex("dbo.MenuItems", new[] { "Category_Id" });
            AlterColumn("dbo.MenuItems", "Category_Id", c => c.Int());
            AlterColumn("dbo.MenuItems", "Description", c => c.String());
            AlterColumn("dbo.MenuItems", "Name", c => c.String());
            CreateIndex("dbo.MenuItems", "Category_Id");
            AddForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
