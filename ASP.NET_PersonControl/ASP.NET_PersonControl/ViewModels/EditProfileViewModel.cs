using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class EditProfileViewModel
    {
        public ApplicationUser user { get; set; }
        public string imgUpdate { get; set; }
    }
}