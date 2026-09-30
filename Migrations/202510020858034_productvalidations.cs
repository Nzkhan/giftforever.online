namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productvalidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Products", "ProductDetails", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Products", "SKUNo", c => c.String(maxLength: 50));
            AlterColumn("dbo.Products", "ProductDescription", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductDescription", c => c.String());
            AlterColumn("dbo.Products", "SKUNo", c => c.String());
            AlterColumn("dbo.Products", "ProductDetails", c => c.String());
            AlterColumn("dbo.Products", "ProductName", c => c.String());
        }
    }
}
