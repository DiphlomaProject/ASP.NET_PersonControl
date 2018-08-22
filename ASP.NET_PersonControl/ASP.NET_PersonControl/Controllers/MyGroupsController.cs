using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize]
    public class MyGroupsController : Controller
    {
        // GET: MyGroups
        public ActionResult Index()
        {

            return View();
        }
    }
}