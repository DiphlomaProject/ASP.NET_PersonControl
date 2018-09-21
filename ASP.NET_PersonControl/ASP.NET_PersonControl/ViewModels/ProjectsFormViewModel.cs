using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class ProjectsFormViewModel
    {
        // deteils info about project
        public Projects project { get; set; }
        public Customers customer { get; set; }

        // info about all projects
        public IEnumerable<Projects> projectsList { get; set; } // for my projects
        public IEnumerable<Projects> projects { get; set; }

        public IEnumerable<Dictionary<int, Customers>> customersList { get; set; } // for my projects
        public IEnumerable<Customers> customers { get; set; }

        public Groups group { get; set; }
        public IEnumerable<Groups> groups { get; set; }
        public IEnumerable<ApplicationUser> groupsOwners { get; set; }
        public IEnumerable<Dictionary<int, ApplicationUser>> groupsOwnersList { get; set; } // for my projects

        [NotMapped]
        public string[] SelectedIDArray { get; set; }

        public IEnumerable<Groups> groupsInProject {get;set; }
        public IEnumerable<Dictionary<int, List<Groups>>> groupsInProjectList { get; set; } // for my projects

        public List<string> filelist { get; set; }
        public List<string> fileinfo { get; set; }



    }
}