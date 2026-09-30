namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignkeys : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrderDetails", "OrderID");
            CreateIndex("dbo.OrderDetails", "ProductsID");
            AddForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetails", "ProductsID", "dbo.Products", "ID", cascadeDelete: true);
            DropColumn("dbo.Carts", "RoleID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Carts", "RoleID", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderDetails", "ProductsID", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
            DropIndex("dbo.OrderDetails", new[] { "ProductsID" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
        }
    }
}
