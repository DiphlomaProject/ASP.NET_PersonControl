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
    public class MyProjectsController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> roleManager { get; set; }
        // GET: MyProjects
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
                                            where gr.Owner == User.Identity.GetUserId()
                                            select gr).ToList();
            groupsList.AddRange(gWhereUserOwner);

            List<Projects> projectsList = (from pj in _context.Projects.ToList()
                                           from gl in groupsList
                                           from gp in _context.ProjectsGroups.ToList()
                                           where pj.Id == gp.ProjId && gp.GroupId == gl.Id
                                           select pj).ToList();
            //_context.Groups.Select(g => g).ToList<Groups>();

            List<Customers> customersList = (from c in _context.Customers.ToList()
                                             from pj in projectsList
                                             where c.Id == pj.Customer
                                             select c).ToList();

            List<ApplicationUser> groupsOwners = (from u in _context.Users.ToList()
                                            from pg in _context.ProjectsGroups.ToList()
                                            from pj in projectsList
                                             from gr in groupsList
                                             where pj.Id == pg.ProjId && pg.GroupId == gr.Id && gr.Owner == u.Id
                                             select u).ToList();

            /*List<ApplicationUser> ownersList = new List<ApplicationUser>();
            foreach (String owner_id in groupsList.Select(g => g.Owner).ToList())
                if (_context.Users.FirstOrDefault(o => o.Id == owner_id) != null)
                    ownersList.Add(_context.Users.FirstOrDefault(o => o.Id == owner_id));*/

            return View(new ProjectsFormViewModel(){ projects = projectsList, groupsInProject = groupsList, customersList = customersList, groupsOwners = groupsOwners });
        }
    }
}