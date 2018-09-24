using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class StatViewModel
    {

        public IEnumerable<ApplicationUser> Employees { get; set; }

        public TasksForGroups taskForGroups { get; set; }

        public TasksForProjects tasksForProjects { get; set; }

        public TasksForUser tasksForUser { get; set; }

        public IEnumerable<Projects> projectsList { get; set; }

        public int countGetTaskAdmin { get; set; }
        public int countGetTaskEmpl { get; set; }
        public int priceCompliteTotal { get; set; }
    }
}