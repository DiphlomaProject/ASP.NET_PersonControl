using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ASP.NET_PersonControl.Models
{
    //[Table("AspNetUsers")] // set like class for a table
    public class AspNetUsers
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        [StringLength(30)]
        public string EMAIL { get; set; }
        public bool EMAILCONFIRMED { get; set; }
        public string PHONENUMBER { get; set; }
        public bool PHONENUMBERCONFIRMED { get; set; }
        public bool TWOFACTORENABLED { get; set; }
        public DateTime LOCKOUTENDDATEUTC;
        void SetLOCKOUTENDDATEUTC(DateTime date)
        {
            if (date == null)
                LOCKOUTENDDATEUTC = DateTime.Parse("1/1/1970 00:00 AM");
            else
                LOCKOUTENDDATEUTC = date;
        }

        public bool LOCKOUTENABLED { get; set; }
        public int ACCESSFAILEDCOUNT { get; set; }
        public string USERNAME { get; set; }    // first and last name

        public string COUNTRY { get; set; } //ukraine
        public string CITY { get; set; }    // donetsk
        public string ADDRESS { get; set; } // like sobinova 169*/

        public string role;
        public string roleId;
    }
}