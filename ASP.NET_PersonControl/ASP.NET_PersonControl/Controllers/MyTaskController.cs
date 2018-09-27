using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_PersonControl.ViewModels;
namespace ASP.NET_PersonControl.Controllers
{
    public class MyTaskController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        // GET: MyTask
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();

            string id = User.Identity.GetUserId();
            
            List<TasksForUser> tasklist = (from gr in _context.TasksForUser.ToList() where gr.toUserId == User.Identity.GetUserId() select gr).ToList();

            foreach(TasksForUser taskForUser in tasklist)
            {
                if (taskForUser.toUserId != null)
                    taskForUser.userTo = _context.Users.FirstOrDefault(user => user.Id == taskForUser.toUserId.ToString());
                if(taskForUser.fromUserId != null)
                    taskForUser.userFrom = _context.Users.FirstOrDefault(user => user.Id == taskForUser.fromUserId.ToString());
            }

            // group taks 
            string curUserID = User.Identity.GetUserId();
            List<Groups> groupsList = (from gr in _context.Groups.ToList()
                                       from ug in _context.UsersGroups.ToList()
                                       where gr.Id == ug.GroupId && ug.UserId == User.Identity.GetUserId()
                                       select gr).ToList();
            List<Groups> gWhereUserOwner = (from gr in _context.Groups.ToList()
                                            where gr.Owner == User.Identity.GetUserId()
                                            select gr).ToList();
            //groupsList.AddRange(gWhereUserOwner);
            foreach (Groups g2 in gWhereUserOwner)
                if (groupsList.Contains(g2) == false)
                    groupsList.Add(g2);

            List<TasksForGroups> taskGroups = (from gr in groupsList
                                               from task in _context.TasksForGroups.ToList()
                                               where task.toGroupId == gr.Id select task).ToList();

            groupsList.Clear();
            foreach (TasksForGroups groupTask in taskGroups)
            {
                if (groupTask.fromUserId != null)
                {
                    groupTask.userFrom = _context.Users.FirstOrDefault(user => user.Id == groupTask.fromUserId.ToString());
                    groupsList.Add(_context.Groups.FirstOrDefault(g => g.Id == groupTask.toGroupId));
                }
            }

            //project tasks
            List<Projects> projectsList = (from pj in _context.Projects.ToList()
                                           from gl in groupsList
                                           from gp in _context.ProjectsGroups.ToList()
                                           where pj.Id == gp.ProjId && gp.GroupId == gl.Id
                                           select pj).Distinct().ToList();

            List<TasksForProjects> taskProjects = (from pj in projectsList
                                                   from task in _context.TasksForProjects.ToList()
                                                   where task.toProjectId == pj.Id select task).ToList();
            foreach (TasksForProjects projTask in taskProjects)
            {
                if (projTask.fromUserId != null)
                    projTask.userFrom = _context.Users.FirstOrDefault(user => user.Id == projTask.fromUserId.ToString());
            }

            var viewModel = new MyTaskForUserViewModel
            {
                Tasks = tasklist,
                taskGroups = taskGroups,
                groups = groupsList,
                taskProjects = taskProjects,
                curUserId = User.Identity.GetUserId()
            };
            return View(viewModel);
        }
        public ActionResult UpdateTaskPesonal(int id)
        {
            _context = new ApplicationDbContext();
            TasksForUser tasksForUser = _context.TasksForUser.FirstOrDefault(c => c.Id == id);
            bool complete = true;
            tasksForUser.isComplite = complete;
            _context.SaveChanges();
            return RedirectToAction("Index", "MyTask");
        }
        public ActionResult UpdateTaskGroup(int id)
        {
            _context = new ApplicationDbContext();
            TasksForGroups tasksForUser = _context.TasksForGroups.FirstOrDefault(c => c.Id == id);
            bool complete = true;
            tasksForUser.isComplite = complete;
            _context.SaveChanges();
            return RedirectToAction("Index", "MyTask");
        }
        public ActionResult UpdateTaskProjects(int id)
        {
            _context = new ApplicationDbContext();
            TasksForProjects tasksForUser = _context.TasksForProjects.FirstOrDefault(c => c.Id == id);
            bool complete = true;
            tasksForUser.isComplite = complete;
            _context.SaveChanges();
            return RedirectToAction("Index", "MyTask");
        }
    }
}