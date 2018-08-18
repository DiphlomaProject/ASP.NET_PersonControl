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
                if(_context.Users.FirstOrDefault(o => o.Id == owner_id) != null)
                    ownersList.Add(_context.Users.FirstOrDefault(o => o.Id == owner_id));

            var viewModel = new GroupsViewModel{ groups = groupsList, owners = ownersList };

            return View(viewModel);
        }

        public ActionResult groupEdit(int id)
        {
            _context = new ApplicationDbContext();
            Groups group = _context.Groups.FirstOrDefault(gr => gr.Id == id);
            if (group == null)
                return RedirectToAction("Index", "Group");
            
            ApplicationUser curOwner = _context.Users.FirstOrDefault(o => o.Id == group.Owner);
            List<ApplicationUser> users = _context.Users.Select(u => u).ToList();
            //List<ApplicationUser> usersGroup = _context.Users.Select(u => u).ToList();
            //for ( in )
            var usersGroup = (from u in _context.Users.ToList()
                             from gu in _context.UsersGroups.ToList()
                             where gu.GroupId == @group.Id && gu.UserId == u.Id select u).ToList();


            var groupsView = new GroupsViewModel
            {
                group = group,
                owners = users,
                curOwner = curOwner,
                usersOfCurrentGroups = usersGroup
                

            };
            groupsView.SelectedIDArray = groupsView.usersOfCurrentGroups.Select(u=>u.DisplayName).ToArray();
            
            SelectList list = new SelectList(groupsView.SelectedIDArray);
            ViewBag.myList = list;

            return View(groupsView );
        }

        public ActionResult Create()
        {
            _context = new ApplicationDbContext();
            var viewModel = new GroupsViewModel {
                group = new Groups(), owners = _context.Users.Select(c => c).ToList()
            };

            return View(viewModel);
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
        public ActionResult Save(GroupsViewModel groupController)
        {
            _context = new ApplicationDbContext();

            if (groupController.curOwner.Id == null || groupController.group.Title == null || groupController.group.Description == null)
            {
                var viewModel = new GroupsViewModel
                {
                    group = groupController.group,
                    owners = _context.Users.Select(c => c).ToList()
                };

                return View("Create", viewModel);
            }


            if (_context.Groups.FirstOrDefault(g => g.Id == groupController.group.Id) == null)
            {
                groupController.group.Owner = groupController.curOwner.Id;
                _context.Groups.Add(groupController.group);
            }
            else
            {
                Groups groups = _context.Groups.FirstOrDefault(g => g.Id == groupController.group.Id);
                groups.Title = groupController.group.Title;
                groups.Owner = groupController.curOwner.Id;
                groups.Description = groupController.group.Description;
            }
            _context.SaveChanges();
            if(groupController.SelectedIDArray !=null && groupController.SelectedIDArray.Count() > 0)
            {

                _context = new ApplicationDbContext();
                if (_context.Groups.FirstOrDefault(g => g.Id == groupController.group.Id) != null)
                {
                    foreach (string id in groupController.SelectedIDArray)
                        if (_context.Users.FirstOrDefault(u => u.Id == id) != null && _context.UsersGroups.Select(ug => ug).Where(g => g.UserId == id && g.GroupId == groupController.group.Id).Count() == 0)
                            _context.UsersGroups.Add(new UsersGroups() { UserId = id, GroupId = groupController.group.Id });
                    _context.SaveChangesAsync();

                }
            }
            return RedirectToAction("Index", "Group");
        }

        [HttpGet, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            _context = new ApplicationDbContext();
            if(_context.Groups.FirstOrDefault(g => g.Id == id) == null)
                return RedirectToAction("Index", "Group");

            //Groups groups = _context.Groups.FirstOrDefault(g => g.Id == id);

            var projectGroups = from details in _context.ProjectsGroups
                                  where details.GroupId == id
                                  select details;
            if (projectGroups != null)
                _context.ProjectsGroups.RemoveRange(projectGroups);

            var usersGroup = from user in _context.UsersGroups
                             where user.GroupId == id
                             select user;
            if(usersGroup != null)
                _context.UsersGroups.RemoveRange(usersGroup);

            _context.Groups.Remove(_context.Groups.FirstOrDefault(g => g.Id == id));

            _context.SaveChanges();

            return RedirectToAction("Index", "Group");
        }
    }
}