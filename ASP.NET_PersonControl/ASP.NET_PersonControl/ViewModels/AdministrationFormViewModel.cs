using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class AdministrationFormViewModel
    {
        public List<IdentityRole> Roles { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}