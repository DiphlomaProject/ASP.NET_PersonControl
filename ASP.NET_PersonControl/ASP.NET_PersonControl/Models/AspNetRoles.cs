using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    //[Table("AspNetRoles")] // set like class for a table
    public class AspNetRoles
    {
        public string Id { get; set; }
        public string NAME { get; set; }

     
    }
}