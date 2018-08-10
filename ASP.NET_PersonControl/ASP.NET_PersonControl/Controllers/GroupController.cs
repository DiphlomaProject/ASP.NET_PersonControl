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
            //string id = User.Identity.GetUserId();
            //ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id); // id of current user
            //List<int> usersGroupsId = _context.UsersGroups.Where(ug => ug.UserId == id).Select(u => u.GroupId).ToList<int>(); // groups id's
            List<Groups> groupsList = _context.Groups.Select(g => g).ToList<Groups>();
            List<ApplicationUser> ownersList = new List<ApplicationUser>();
            foreach(String owner_id in groupsList.Select(g => g.Owner).ToList())
                ownersList.Add(_context.Users.FirstOrDefault(o => o.Id == owner_id) ?? new ApplicationUser());

            var viewModel = new GroupsViewModel{ groups = groupsList, owners = ownersList };

            return View(viewModel);
        }

        public ActionResult groupEdit(int id)
        {
            _context = new ApplicationDbContext();
            List<Groups> groupsList = _context.Groups.Select(g => g).Where(i => i.Id == id).ToList<Groups>();
            List<ApplicationUser> ownersList = new List<ApplicationUser>();
            foreach (String owner_id in groupsList.Select(g => g.Owner).ToList())
                ownersList.Add(_context.Users.FirstOrDefault(o => o.Id == owner_id) ?? new ApplicationUser());

            return View( new GroupsViewModel { groups = groupsList, owners = ownersList } );
        }

        public ActionResult Create()
        {
            
            var viewModel = new CreateNewGroup {
            };

            return View("Create", viewModel);
        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save( CreateNewGroup groupController)
        {

            if (!ModelState.IsValid)
            {
                var viewModel = new GroupController
                {
                   //grou = _context.Roles.ToList()
                };

                return View("Create", viewModel);
            }

            return RedirectToAction("Index", "Group");
        }
        }
}