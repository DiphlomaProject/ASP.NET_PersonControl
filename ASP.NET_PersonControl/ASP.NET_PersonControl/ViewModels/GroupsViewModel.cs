using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_PersonControl.ViewModels
{
    public class GroupsViewModel
    {
        public Groups group { get; set; }   // need for edit & create group
        public ApplicationUser curOwner { get; set; }   // need for edit & create group
        public IEnumerable<ApplicationUser> owners { get; set; } // need for edit & create group

        public IEnumerable<Groups> groups { get; set; }     // need for show all groups
        public IEnumerable<ApplicationUser> usersOfCurrentGroups { get; set; }
        //public IEnumerable<Projects> projectsOfCurrentGroups { get; set; }
        //public IEnumerable<FilesGroups> filesOfGroups { get; set; }
        //pulic IEnumerable<Files> files { get; set; }

       // public IEnumerable<ApplicationUser> SelectedIDArray { get; set; }
      public string SelectedIDs { get; set; }
       // public IEnumerable<MultiSelectList> people { get; set; }
        [NotMapped]
        public string[] SelectedIDArray { get; set; }
    }
}