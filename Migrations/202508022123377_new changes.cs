namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newchanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductAttributeValues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        AttributeID = c.Int(nullable: false),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductAttributes", t => t.AttributeID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.AttributeID);
            
            CreateTable(
                "dbo.ProductAttributes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AttributeName = c.String(nullable: false),
                        AttributeType = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        IsFilterable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Products", "WholesalePrice", c => c.Double());
            AddColumn("dbo.Products", "MinimumOrderQuantity", c => c.Int());
            AddColumn("dbo.Products", "Fragrance", c => c.String());
            AddColumn("dbo.Products", "IsFeatured", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "IsNewArrival", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "IsBestseller", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "HowToUse", c => c.String());
            AddColumn("dbo.ProductTypes", "ParentTypeID", c => c.Int());
            AddColumn("dbo.ProductTypes", "DisplayOrder", c => c.Int(nullable: false));
            AddColumn("dbo.ProductTypes", "ShowInNavigation", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ProductTypes", "ParentTypeID");
            AddForeignKey("dbo.ProductTypes", "ParentTypeID", "dbo.ProductTypes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductTypes", "ParentTypeID", "dbo.ProductTypes");
            DropForeignKey("dbo.ProductAttributeValues", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductAttributeValues", "AttributeID", "dbo.ProductAttributes");
            DropIndex("dbo.ProductTypes", new[] { "ParentTypeID" });
            DropIndex("dbo.ProductAttributeValues", new[] { "AttributeID" });
            DropIndex("dbo.ProductAttributeValues", new[] { "ProductID" });
            DropColumn("dbo.ProductTypes", "ShowInNavigation");
            DropColumn("dbo.ProductTypes", "DisplayOrder");
            DropColumn("dbo.ProductTypes", "ParentTypeID");
            DropColumn("dbo.Products", "HowToUse");
            DropColumn("dbo.Products", "IsBestseller");
            DropColumn("dbo.Products", "IsNewArrival");
            DropColumn("dbo.Products", "IsFeatured");
            DropColumn("dbo.Products", "Fragrance");
            DropColumn("dbo.Products", "MinimumOrderQuantity");
            DropColumn("dbo.Products", "WholesalePrice");
            DropTable("dbo.ProductAttributes");
            DropTable("dbo.ProductAttributeValues");
        }
    }
}
