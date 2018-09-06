using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Controllers.Support_Classes
{
    public class SingletonManager
    {
        private static SingletonManager instance;
        public string storageAccountName = "aspnetpersoncontrol";
        public string keyOne = "GfiRnxHVXsaluga4L4R0zZOy4Ken4VnF3xM7I66OC263LJ9Sf2BOQgX41+/WpBlA8vMB5aP4wN/Uh00OF4MdXw==";
        private SingletonManager()
        { }

        public static SingletonManager getInstance()
        {
            if (instance == null)
                instance = new SingletonManager();
            return instance;
        }

        
    }
}