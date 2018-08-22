using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class ProjectsFormViewModel
    {
        // deteils info about project
        public Projects project { get; set; }
        public Customers customer { get; set; }

        // info about all projects
        public IEnumerable<Projects> projectsList { get; set; }
        public IEnumerable<Projects> projects { get; set; }

        public IEnumerable<Customers> customersList { get; set; }
        public IEnumerable<Customers> customers { get; set; }

        public Customers curCustomers {get;set;}
      //  public DateTime dateTime { get; set; }

      //  public IEnumerable<Projects> dataTimeList { get; set; }
        
    }
}