using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public int Customer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PriceInDollars { get; set; }
        public bool isComplite { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime UntilTime { get; set; }
    }
}