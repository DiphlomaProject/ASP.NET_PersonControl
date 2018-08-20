namespace ASP.NET_PersonControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Token_Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.String(),
                        token = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tokens");
        }
    }
}
