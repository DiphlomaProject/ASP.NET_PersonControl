namespace ASP.NET_PersonControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAspNetUsers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String(maxLength: 50));
        }
    }
}
