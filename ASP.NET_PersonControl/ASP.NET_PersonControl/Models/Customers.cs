using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    public class Customers
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string ContactPerson { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
    }
}