namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SUKproductpropertycreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SKUNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SKUNo");
        }
    }
}
