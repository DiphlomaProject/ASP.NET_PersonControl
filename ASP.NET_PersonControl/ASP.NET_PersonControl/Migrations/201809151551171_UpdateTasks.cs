namespace ASP.NET_PersonControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksForGroups", "title", c => c.String());
            AddColumn("dbo.TasksForProjects", "title", c => c.String());
            AddColumn("dbo.TasksForUsers", "title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksForUsers", "title");
            DropColumn("dbo.TasksForProjects", "title");
            DropColumn("dbo.TasksForGroups", "title");
        }
    }
}
