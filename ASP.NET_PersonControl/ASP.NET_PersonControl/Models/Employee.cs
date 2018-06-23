using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    [Table("AspNetUsers")] // set like class for a table
    public class Employee
    {
        public string Id { get; set; }
        public string EMAIL { get; set; }
        public bool EMAILCONFIRMED { get; set; }
        public string PHONENUMBER { get; set; }
        public bool PHONENUMBERCONFIRMED { get; set; }
        public bool TWOFACTORENABLED { get; set; }
        public string LOCKOUTENDDATEUTC { get; set; }
        public bool LOCKOUTENABLED { get; set; }
        public int ACCESSFAILEDCOUNT { get; set; }
        public string USERNAME { get; set; }    // first and last name
        public string COUNTRY { get; set; } //ukraine
        public string CITY { get; set; }    // donetsk
        public string ADDRESS { get; set; } // like sobinova 169*/
    }
}