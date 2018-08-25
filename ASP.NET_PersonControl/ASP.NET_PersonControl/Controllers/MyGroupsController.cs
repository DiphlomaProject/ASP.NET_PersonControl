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
    [Authorize]
    public class MyGroupsController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> roleManager { get; set; }
        // GET: MyGroups
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();

            //UsersGroups curUser = _context.Users User.Identity.GetUserId
            string curUserID = User.Identity.GetUserId();

            List<Groups> groupsList = (from gr in _context.Groups.ToList()
                                       from ug in _context.UsersGroups.ToList()
                                       where gr.Id == ug.GroupId && ug.UserId == User.Identity.GetUserId()
                                       select gr).ToList();
            List<Groups> gWhereUserOwner = (from gr in _context.Groups.ToList()
                                            where gr.Owner == User.Identity.GetUserId() select gr).ToList();
            groupsList.AddRange(gWhereUserOwner);
                //_context.Groups.Select(g => g).ToList<Groups>();


            List<ApplicationUser> ownersList = new List<ApplicationUser>();
            foreach (String owner_id in groupsList.Select(g => g.Owner).ToList())
                if (_context.Users.FirstOrDefault(o => o.Id == owner_id) != null)
                    ownersList.Add(_context.Users.FirstOrDefault(o => o.Id == owner_id));


            var viewModel = new GroupsViewModel { groups = groupsList, owners = ownersList };

            return View(viewModel);
        }

        public ActionResult GroupDitail(int id)
        {
            _context = new ApplicationDbContext();
            Groups group = _context.Groups.FirstOrDefault(gr => gr.Id == id);
            if (group == null)
                return RedirectToAction("Index", "Group");

            ApplicationUser curOwner = _context.Users.FirstOrDefault(o => o.Id == group.Owner);
            List<ApplicationUser> users = _context.Users.Select(u => u).ToList();
            var usersGroup = (from u in _context.Users.ToList()
                              from gu in _context.UsersGroups.ToList()
                              where gu.GroupId == @group.Id && gu.UserId == u.Id
                              select u).ToList();


            var groupsView = new GroupsViewModel
            {
                group = group,
                owners = users,
                curOwner = curOwner,
                usersOfCurrentGroups = usersGroup,


            };
            groupsView.SelectedIDArray = groupsView.usersOfCurrentGroups.Select(u => u.Id).ToArray();
            return View(groupsView);
        }
    }
}