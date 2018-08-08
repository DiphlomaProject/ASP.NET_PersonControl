using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class GroupsViewModel
    {
        public ApplicationUser user { get; set; }
        //public IEnumerable<UsersGroups> usersOfCurrentGroups { get; set; }
        public IEnumerable<Groups> groups { get; set; }
        //public IEnumerable<Projects> projectsOfCurrentGroups { get; set; }
        //public IEnumerable<FilesGroups> filesOfGroups { get; set; }
        //pulic IEnumerable<Files> files { get; set; }
    }
}