using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class TaskForProjectsViewModel
    {
        //public Tasf taskForGroups { get; set; }

        public TasksForProjects tasksForProjects {get; set;}
        public ApplicationUser user { get; set; }

        public Projects project { get; set; }

        public IEnumerable<Projects> projects { get; set; }
        
    }
}