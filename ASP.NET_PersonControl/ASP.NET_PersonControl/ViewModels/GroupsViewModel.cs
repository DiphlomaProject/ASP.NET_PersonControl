using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class GroupsViewModel
    {
        public IEnumerable<Groups> groups { get; set; }
        public ApplicationUser user { get; set; }
    }
}