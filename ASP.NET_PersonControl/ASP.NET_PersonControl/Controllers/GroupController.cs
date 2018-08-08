using ASP.NET_PersonControl.Models;
using ASP.NET_PersonControl.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ASP.NET_PersonControl.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> roleManager { get; set; }
        // GET: Group
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();
            string id = User.Identity.GetUserId();
            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id); // id of current user
            List<int> usersGroupsId = _context.UsersGroups.Where(ug => ug.UserId == id).Select(u => u.GroupId).ToList<int>(); // groups id's
            List<Groups> groupsList = _context.Groups.Select(g => g).ToList<Groups>();

            var viewModel = new GroupsViewModel{ groups = groupsList, user = employee };

            return View(viewModel);
        }
    }
}