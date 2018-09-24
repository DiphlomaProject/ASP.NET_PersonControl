namespace ASP.NET_PersonControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksForGroups", "groupName", c => c.String());
            AddColumn("dbo.TasksForProjects", "projectName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksForProjects", "projectName");
            DropColumn("dbo.TasksForGroups", "groupName");
        }
    }
}
