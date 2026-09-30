namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typephoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTypes", "HomePhoto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTypes", "HomePhoto");
        }
    }
}
