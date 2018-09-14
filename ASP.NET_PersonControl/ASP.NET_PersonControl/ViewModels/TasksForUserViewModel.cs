using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class TasksForUserViewModel
    {
        public TasksForUser tasksForUser { get; set; }
        public ApplicationUser user { get; set; }
        public ApplicationUser userTo { get; set; }
        public IEnumerable<ApplicationUser> toUser { get; set; } // need for edit & create group
        [NotMapped]
        public string[] SelectedIDArray { get; set; }
    }
}