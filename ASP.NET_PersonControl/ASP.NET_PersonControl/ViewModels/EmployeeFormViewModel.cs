using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ASP.NET_PersonControl.ViewModels
{
    public class EmployeeFormViewModel
    {
        public string RoleId { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public ApplicationUser user { get; set; }
    }
}