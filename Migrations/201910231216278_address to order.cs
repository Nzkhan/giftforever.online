namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addresstoorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "AddressID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "AddressID");
        }
    }
}
