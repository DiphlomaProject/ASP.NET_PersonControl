using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class MyTaskForUserViewModel
    {
        public TasksForUser tasksForUser { get; set; }
        public IEnumerable<TasksForUser> Tasks { get; set; }

        public TasksForGroups tasksForGroups { get; set; }
        public IEnumerable<TasksForGroups> taskGroups { get; set; }
        public TasksForProjects tasksForProjects { get; set; }
        public IEnumerable<TasksForProjects> taskProjects { get; set; }

        public IEnumerable<Groups> groups { get; set; }



    }
}