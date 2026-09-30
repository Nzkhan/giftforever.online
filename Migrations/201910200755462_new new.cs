namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnew : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderDetails", "OrderdID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "OrderdID", c => c.Int(nullable: false));
        }
    }
}
