namespace Baganova.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createddatatypeofblogchange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogs", "CreatedDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogs", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
