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
            
            var viewModel = new MyTaskForUserViewModel
            {
                Tasks = tasklist
            };
            return View(viewModel);
        }


        public ActionResult UpdateTask(int id)
        {
            _context = new ApplicationDbContext();
            TasksForUser tasksForUser = _context.TasksForUser.FirstOrDefault(c => c.Id == id);
            bool complete = true;
            tasksForUser.isComplite = complete;
            _context.SaveChanges();
            return RedirectToAction("Index", "MyTask");
        }
    }
}