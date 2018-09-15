using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class TaskForGroupsViewModel
    {
        public TasksForGroups taskForGroups { get; set; }

        public ApplicationUser user { get; set; }
        public Groups group { get; set; }
        public IEnumerable<Groups> groups { get; set; }
      
    }
}