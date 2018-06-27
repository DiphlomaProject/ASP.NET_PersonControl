using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public byte isReaded { get; set; }
        public string notifName { get; set; }
        public string notifDescription { get; set; }
	    public DateTime notifData { get; set; } 

    }
}