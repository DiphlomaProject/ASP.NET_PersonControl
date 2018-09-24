namespace ASP.NET_PersonControl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksForGroups", "userFrom_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.TasksForProjects", "userFrom_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.TasksForUsers", "userFrom_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.TasksForUsers", "userTo_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.TasksForGroups", "userFrom_Id");
            CreateIndex("dbo.TasksForProjects", "userFrom_Id");
            CreateIndex("dbo.TasksForUsers", "userFrom_Id");
            CreateIndex("dbo.TasksForUsers", "userTo_Id");
            AddForeignKey("dbo.TasksForGroups", "userFrom_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TasksForProjects", "userFrom_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TasksForUsers", "userFrom_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TasksForUsers", "userTo_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TasksForUsers", "userTo_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TasksForUsers", "userFrom_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TasksForProjects", "userFrom_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TasksForGroups", "userFrom_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TasksForUsers", new[] { "userTo_Id" });
            DropIndex("dbo.TasksForUsers", new[] { "userFrom_Id" });
            DropIndex("dbo.TasksForProjects", new[] { "userFrom_Id" });
            DropIndex("dbo.TasksForGroups", new[] { "userFrom_Id" });
            DropColumn("dbo.TasksForUsers", "userTo_Id");
            DropColumn("dbo.TasksForUsers", "userFrom_Id");
            DropColumn("dbo.TasksForProjects", "userFrom_Id");
            DropColumn("dbo.TasksForGroups", "userFrom_Id");
        }
    }
}
