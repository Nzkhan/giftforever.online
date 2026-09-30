namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userandAdrees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserDetails", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDetails", "Address");
            DropColumn("dbo.Addresses", "IsActive");
        }
    }
}
