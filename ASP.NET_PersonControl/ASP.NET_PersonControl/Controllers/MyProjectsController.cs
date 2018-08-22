using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize]
    public class MyProjectsController : Controller
    {
        // GET: MyProjects
        public ActionResult Index()
        {
            return View();
        }
    }
}