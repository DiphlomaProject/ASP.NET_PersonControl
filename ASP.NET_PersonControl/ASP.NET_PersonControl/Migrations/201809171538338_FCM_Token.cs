namespace ASP.NET_PersonControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FCM_Token : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FCMToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FCMToken");
        }
    }
}
