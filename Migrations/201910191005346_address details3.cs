namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addressdetails3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        address = c.String(),
                        UserDetailsID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDetails", t => t.UserDetailsID, cascadeDelete: true)
                .Index(t => t.UserDetailsID);
            
            DropColumn("dbo.UserDetails", "State");
            DropColumn("dbo.UserDetails", "City");
            DropColumn("dbo.UserDetails", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDetails", "Address", c => c.String());
            AddColumn("dbo.UserDetails", "City", c => c.String());
            AddColumn("dbo.UserDetails", "State", c => c.String());
            DropForeignKey("dbo.Addresses", "UserDetailsID", "dbo.UserDetails");
            DropIndex("dbo.Addresses", new[] { "UserDetailsID" });
            DropTable("dbo.Addresses");
        }
    }
}
