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
using ASP.NET_PersonControl.Controllers.Api;
using static ASP.NET_PersonControl.Controllers.Api.UsersController;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class TasksController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> RoleManager { get; set; }
        public ApplicationUser userCur { get; set; }

        public SingletonManager singleton = SingletonManager.getInstance();

        FirebaseCMController firebase = new FirebaseCMController();
        // GET: Tasks
        public ActionResult Index()
        {


            _context = new ApplicationDbContext();

            string id = User.Identity.GetUserId();

            List<TasksForUser> tasklist = (from gr in _context.TasksForUser.ToList() where gr.fromUserId == User.Identity.GetUserId() select gr).ToList();
            foreach (TasksForUser taskForUser in tasklist)
                if (taskForUser.toUserId != null)
                    taskForUser.userTo = _context.Users.FirstOrDefault(user => user.Id == taskForUser.toUserId.ToString());

            List<TasksForGroups> tasklistGroups = (from gr in _context.TasksForGroups.ToList() where gr.fromUserId == User.Identity.GetUserId() select gr).ToList();
            foreach (TasksForGroups groupTask in tasklistGroups)
                if (_context.Groups.FirstOrDefault(p => p.Id == groupTask.toGroupId) != null)
                    groupTask.groupName = _context.Groups.FirstOrDefault(p => p.Id == groupTask.toGroupId).Title;

            List<TasksForProjects> tasklistProject = (from gr in _context.TasksForProjects.ToList() where gr.fromUserId == User.Identity.GetUserId() select gr).ToList();
            foreach (TasksForProjects projTask in tasklistProject)
                if (_context.Projects.FirstOrDefault(p => p.Id == projTask.toProjectId) != null)
                    projTask.projectName = _context.Projects.FirstOrDefault(p => p.Id == projTask.toProjectId).Title;

            var viewModel = new TaskAdminViewModel
            {
                Tasks = tasklist,
                taskGroups = tasklistGroups,
                taskProjects = tasklistProject
            };
            return View(viewModel);

            

           
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
                string id = User.Identity.GetUserId();

                ApplicationUser employeeFrom = _context.Users.SingleOrDefault(emp => emp.Id == id);
                string Title = employeeFrom.DisplayName;
                ApplicationUser employeeTo = _context.Users.SingleOrDefault(emp => emp.Id == tasksForUserController.userTo.Id);
               
                if(employeeTo.FCMToken != null)
                {
                    var FCMToken = employeeTo.FCMToken;
                    string token = FCMToken.ToString();
                    string TouserId = tasksForUserController.userTo.Id;
                    string Message = tasksForUserController.tasksForUser.title;

                    if (_context.TasksForUser.Add(result) != null)
                    {

                        firebase.FirebaseNotification(token, TouserId, Title, Message);
                    }
                    _context.TasksForUser.Add(result);
                }
                {
                    _context.TasksForUser.Add(result);
                }
                


            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Tasks");
        }

        public ActionResult DeleteTaskUser(int id)
        {
            _context = new ApplicationDbContext();
            if (_context.TasksForUser.FirstOrDefault(c => c.Id == id) == null)
                return RedirectToAction("Index", "Tasks");

            _context.TasksForUser.Remove(_context.TasksForUser.FirstOrDefault(c => c.Id == id));

            _context.SaveChanges();

            return RedirectToAction("Index", "Tasks");



        }
        public ActionResult CreateTaskForGroups()
        {
            _context = new ApplicationDbContext();


            string id = User.Identity.GetUserId();

            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
            List<Groups> groupList = _context.Groups.Select(g => g).ToList<Groups>();
            var viewModel = new TaskForGroupsViewModel
            {   taskForGroups = new TasksForGroups(),
                user = employee,
                groups = groupList
            };
 

            return View(viewModel);

        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult CreateTaskForGroups(FormCollection collection)
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


      

        public ActionResult SaveGroupTask(TaskForGroupsViewModel taskForGroupsViewModel)
        {
            _context = new ApplicationDbContext();
            string id = User.Identity.GetUserId();
            if (taskForGroupsViewModel.taskForGroups.description == null || taskForGroupsViewModel.taskForGroups.dateTimeEnd == null || taskForGroupsViewModel.taskForGroups.dateTimeBegin == null)
            {
                ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
                List<Groups> groupList = _context.Groups.Select(g => g).ToList<Groups>();
                var viewModel = new TaskForGroupsViewModel
                {
                    taskForGroups = new TasksForGroups(),
                    user = employee,
                    groups = groupList
                };
                return View("CreateTaskForGroups", viewModel);
            }
            if (_context.TasksForGroups.FirstOrDefault(c => c.Id == taskForGroupsViewModel.taskForGroups.Id) == null)
            {
                string AdminID = User.Identity.GetUserId();
                taskForGroupsViewModel.taskForGroups.fromUserId = taskForGroupsViewModel.user.Id;
                taskForGroupsViewModel.taskForGroups.toGroupId = taskForGroupsViewModel.group.Id;
                List<UsersGroups> userGroups = (from gr in _context.UsersGroups.ToList() where gr.GroupId == taskForGroupsViewModel.taskForGroups.toGroupId select gr).ToList();
                foreach(UsersGroups userID in userGroups)
                {
                    if (userID.Id.ToString() != null)
                    {
                        ApplicationUser employeeFrom = _context.Users.SingleOrDefault(emp => emp.Id == AdminID);
                        string Title = employeeFrom.DisplayName;
                        ApplicationUser employeeTo = _context.Users.SingleOrDefault(emp => emp.Id == userID.UserId.ToString());

                        if (employeeTo.FCMToken != null)
                        {
                            var FCMToken = employeeTo.FCMToken;
                            string token = FCMToken.ToString();
                            string TouserId = employeeTo.Id;
                            string Message = taskForGroupsViewModel.taskForGroups.title;
                            firebase.FirebaseNotification(token, TouserId, Title, Message);
                           
                        }
                    }

                    var result = taskForGroupsViewModel.taskForGroups;
                    _context.TasksForGroups.Add(result);

                }
                // List<UsersGroups> userGroups = _context.UsersGroups.Select(us => us.GroupId == taskForGroupsViewModel.taskForGroups.Id).ToList<UsersGroups>();

                //foreach (TasksForGroups groupTask in taskGroups)
                //{
                //    if (groupTask.fromUserId != null)
                //        groupTask.userFrom = _context.Users.FirstOrDefault(user => user.Id == groupTask.fromUserId.ToString());
                //}
                //tasksForUserController.tasksForUser.fromUserId = tasksForUserController.user.Id;
                ////groupController.group.Owner = groupController.curOwner.Id;
                //tasksForUserController.tasksForUser.toUserId = tasksForUserController.userTo.Id;
                //var result = tasksForUserController.tasksForUser;
                //string id = User.Identity.GetUserId();

                //ApplicationUser employeeFrom = _context.Users.SingleOrDefault(emp => emp.Id == id);
                //string Title = employeeFrom.DisplayName;
                //ApplicationUser employeeTo = _context.Users.SingleOrDefault(emp => emp.Id == tasksForUserController.userTo.Id);

                //if (employeeTo.FCMToken != null)
                //{
                //    var FCMToken = employeeTo.FCMToken;
                //    string token = FCMToken.ToString();
                //    string TouserId = tasksForUserController.userTo.Id;
                //    string Message = tasksForUserController.tasksForUser.title;

                //    if (_context.TasksForUser.Add(result) != null)
                //    {

                //        firebase.FirebaseNotification(token, TouserId, Title, Message);
                //    }
                //    _context.TasksForUser.Add(result);
                //}
                //{
                //    _context.TasksForUser.Add(result);
                //}

                
               
            }
         

            //_context.ProjectsGroups.RemoveRange(_context.ProjectsGroups.Select(ug => ug).Where(ug => ug.ProjId == projectController.project.Id).ToList());
            _context.SaveChanges();
            return RedirectToAction("Index", "Tasks");
        }

        public ActionResult DeleteTaskGroups(int id)
        {
            _context = new ApplicationDbContext();
            if (_context.TasksForGroups.FirstOrDefault(c => c.Id == id) == null)
                return RedirectToAction("Index", "Tasks");

            _context.TasksForGroups.Remove(_context.TasksForGroups.FirstOrDefault(c => c.Id == id));

            _context.SaveChanges();

            return RedirectToAction("Index", "Tasks");



        }

        public ActionResult CreateTaskForProjects()
        {

            _context = new ApplicationDbContext();


            string id = User.Identity.GetUserId();

            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
            List<Projects> projectList = _context.Projects.Select(g => g).ToList<Projects>();
            var viewModel = new TaskForProjectsViewModel
            {
                tasksForProjects   = new TasksForProjects(),
                user = employee,
                projects = projectList
            };

            return View(viewModel);

        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult CreateTaskForProjects(FormCollection collection)
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


        public ActionResult SaveProjectTask(TaskForProjectsViewModel taskForProjectsViewModel)
        {
            _context = new ApplicationDbContext();
            string id = User.Identity.GetUserId();
            if (taskForProjectsViewModel.tasksForProjects.description == null || taskForProjectsViewModel.tasksForProjects.dateTimeEnd == null || taskForProjectsViewModel.tasksForProjects.dateTimeBegin == null)
            {
                ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
                List<Groups> groupList = _context.Groups.Select(g => g).ToList<Groups>();
                var viewModel = new TaskForGroupsViewModel
                {
                    taskForGroups = new TasksForGroups(),
                    user = employee,
                    groups = groupList
                };
                return View("CreateTaskForProjects", viewModel);
            }
            if (_context.TasksForProjects.FirstOrDefault(c => c.Id == taskForProjectsViewModel.tasksForProjects.Id) == null)
            {


                taskForProjectsViewModel.tasksForProjects.fromUserId = taskForProjectsViewModel.user.Id;
                taskForProjectsViewModel.tasksForProjects.toProjectId = taskForProjectsViewModel.project.Id;
                var result = taskForProjectsViewModel.tasksForProjects;
                _context.TasksForProjects.Add(result);
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
            return RedirectToAction("Index", "Tasks");
        }

        public ActionResult DeleteTaskProject(int id)
        {
            _context = new ApplicationDbContext();
            if (_context.TasksForProjects.FirstOrDefault(c => c.Id == id) == null)
                return RedirectToAction("Index", "Tasks");

            _context.TasksForProjects.Remove(_context.TasksForProjects.FirstOrDefault(c => c.Id == id));

            _context.SaveChanges();

            return RedirectToAction("Index", "Tasks");



        }
    }
}