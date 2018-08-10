using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class CreateNewGroup
    {
        
        public Groups group { get; set; }

        public IEnumerable<ApplicationUser> users { get; set; }

    }
}