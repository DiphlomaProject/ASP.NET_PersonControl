﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_PersonControl.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index500()
        {
            return View();
        }

        public ActionResult IndexPhone()
        {
            //return Redirect("personcontrol://");
            return View();
        }
    }
}