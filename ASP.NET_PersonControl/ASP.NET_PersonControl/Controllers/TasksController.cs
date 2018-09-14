using ASP.NET_PersonControl.Controllers.Support_Classes;
using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_PersonControl.ViewModels;
namespace ASP.NET_PersonControl.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class TasksController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> RoleManager { get; set; }
        public ApplicationUser userCur { get; set; }

        public SingletonManager singleton = SingletonManager.getInstance();
        // GET: Tasks
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {    
        _context = new ApplicationDbContext();
           
           
            string id = User.Identity.GetUserId();

            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
            var viewModel = new TasksForUserViewModel
            {
                tasksForUser = new TasksForUser(),
                user = employee,
                toUser = _context.Users.Select(c => c).ToList()
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


        public ActionResult SavePersonTask(TasksForUserViewModel tasksForUserController)
        {
            _context = new ApplicationDbContext();
            if (tasksForUserController.tasksForUser.description == null  || tasksForUserController.tasksForUser.dateTimeEnd == null || tasksForUserController.tasksForUser.dateTimeBegin == null)
            {
                string id = User.Identity.GetUserId();

                ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
                var viewModel = new TasksForUserViewModel

                {
                    tasksForUser = new TasksForUser(),
                    user = employee,
                    toUser = _context.Users.Select(c => c).ToList()

                };
                return View("Create", viewModel);
            }
            if (_context.TasksForUser.FirstOrDefault(c => c.Id == tasksForUserController.tasksForUser.Id) == null)
            {
                
                tasksForUserController.tasksForUser.fromUserId = tasksForUserController.user.Id;
                //groupController.group.Owner = groupController.curOwner.Id;
                tasksForUserController.tasksForUser.toUserId = tasksForUserController.userTo.Id;
                var result = tasksForUserController.tasksForUser;
                _context.TasksForUser.Add(result);
            }
            else
            {
                //Projects projects = _context.Projects.FirstOrDefault(c => c.Id == projectController.project.Id);
                //TasksForUser tasksForUser = _context.TasksForUser.FirstOrDefault(c=>c.Id == tasksForUser.Id)
                //projects.Customer = projectController.customer.Id;
                //projects.Title = projectController.project.Title;
                //projects.Description = projectController.project.Description;
                //projects.PriceInDollars = projectController.project.PriceInDollars;
                //projects.isComplite = projectController.project.isComplite;
                //projects.BeginTime = projectController.project.BeginTime;
                //projects.UntilTime = projectController.project.UntilTime;
            }


            //_context.ProjectsGroups.RemoveRange(_context.ProjectsGroups.Select(ug => ug).Where(ug => ug.ProjId == projectController.project.Id).ToList());
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}