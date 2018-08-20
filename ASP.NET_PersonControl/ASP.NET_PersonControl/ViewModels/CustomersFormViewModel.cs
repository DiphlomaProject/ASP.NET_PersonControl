using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class CustomersFormViewModel
    {
        // ditails infro about customer
        public Customers customer { get; set; }
        public IEnumerable<Projects> projects { get; set; }

        // info about all projects
        public IEnumerable<Customers> customersList { get; set; }
        public IEnumerable<Customers> customers { get; set; }
    }
}