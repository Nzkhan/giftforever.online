namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedprodtypemapfromproduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductProductTypeMappings", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductProductTypeMappings", new[] { "ProductID" });
            DropTable("dbo.ProductProductTypeMappings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductProductTypeMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        ProductTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.ProductProductTypeMappings", "ProductID");
            AddForeignKey("dbo.ProductProductTypeMappings", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
        }
    }
}
