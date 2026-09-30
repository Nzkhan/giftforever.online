namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class instock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "InStock", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "InStock");
        }
    }
}
