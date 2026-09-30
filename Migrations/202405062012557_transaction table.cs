namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactiontable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionDetails",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Status = c.String(),
                        ReceiptURL = c.String(),
                        PaymentModel = c.String(),
                        PaymentIntentId = c.String(),
                        PaymentMethod = c.String(),
                        ChargeId = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                        PaymentType = c.String(),
                        BalanceTransactionId = c.String(),
                        UserEmail = c.String(),
                        UserName = c.String(),
                        UserphoneNumber = c.String(),
                        UserCountry = c.String(),
                        UserCity = c.String(),
                        UserState = c.String(),
                        Currency = c.String(),
                        PaymentFailedTransactionId = c.String(),
                        PaymentFailedCode = c.String(),
                        PaymentFailedMessage = c.String(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            AddColumn("dbo.Orders", "TransactionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionDetails", "OrderID", "dbo.Orders");
            DropIndex("dbo.TransactionDetails", new[] { "OrderID" });
            DropColumn("dbo.Orders", "TransactionId");
            DropTable("dbo.TransactionDetails");
        }
    }
}
