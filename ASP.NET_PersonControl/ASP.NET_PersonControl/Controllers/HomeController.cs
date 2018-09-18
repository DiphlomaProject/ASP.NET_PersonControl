using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Serialization.JsonNet;
using FireSharp.Serialization.ServiceStack;

namespace ASP.NET_PersonControl.Controllers
{
    public class HomeController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "uACmEbQdfgzBc8a8bnTskK9310sjPgus3PStEsUk",
            BasePath= "https://personscontrol.firebaseio.com/"
        };

        IFirebaseClient client;
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            if(client!=null)
            {
                TempData["msg"] = "<script>alert('Connect to DB_FB succesfully');</script>";
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}