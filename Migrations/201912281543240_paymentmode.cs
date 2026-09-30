namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentmode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaymentMode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaymentMode");
        }
    }
}
