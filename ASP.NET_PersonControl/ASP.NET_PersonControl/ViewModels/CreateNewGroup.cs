using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class CreateNewGroup
    {
        public string Title { get; set; }

        public string Owner { get; set; }
        public string Description { get; set; }
       // public IEnumerable<Groups> groups { get; set; }
    }
}