using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    public class Groups
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
    }
}