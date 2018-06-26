using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class NewRoleFormModel
    {
        public string RoleNeme { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
