namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderdetailsorderid2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "OrderdID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "OrderdID");
        }
    }
}
