using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ASP.NET_PersonControl.ViewModels
{
    public class EmployeeFormViewModel
    {
        public IEnumerable<AspNetRoles> roles { get; set; }

        public AspNetUsers user { get; set; }
    }
}