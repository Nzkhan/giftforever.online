namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedproducttypefromproduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "TypeID", "dbo.ProductTypes");
            DropIndex("dbo.Products", new[] { "TypeID" });
            CreateTable(
                "dbo.ProductProductTypeMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        ProductTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductProductTypeMappings", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductProductTypeMappings", new[] { "ProductID" });
            DropTable("dbo.ProductProductTypeMappings");
            CreateIndex("dbo.Products", "TypeID");
            AddForeignKey("dbo.Products", "TypeID", "dbo.ProductTypes", "ID", cascadeDelete: true);
        }
    }
}
