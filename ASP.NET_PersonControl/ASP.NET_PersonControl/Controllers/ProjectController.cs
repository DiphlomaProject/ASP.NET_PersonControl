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
    public class ProjectController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> RoleManager { get; set; }
        // GET: Project
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();

            //List<Customers> customersList = _context.Customers.Select(c => c).ToList<Customers>();
            List<Projects> projectsList = _context.Projects.Select(p => p).ToList<Projects>();
            var viewModel = new ProjectsFormViewModel()
            {
               /* customers = customersList,*/ projects = projectsList

            };
            return View(viewModel);
            
        }

        public ActionResult Edit(int id)
        {
            _context = new ApplicationDbContext();
            Projects project = _context.Projects.FirstOrDefault(gr => gr.Id == id);
            if (project == null)
                return RedirectToAction("Index", "Customers");

            var viewModel = new ProjectsFormViewModel
            {
                project = project
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            _context = new ApplicationDbContext();
            var viewModel = new ProjectsFormViewModel
            {
                project = new Projects()
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
        public ActionResult Save(ProjectsFormViewModel projectController)
        {
            _context = new ApplicationDbContext();
            if (projectController.project.Description == null)
            {
                var viewModel = new ProjectsFormViewModel
                {
                    project = projectController.project

                };
                return View("Create", viewModel);
            }
            if (_context.Projects.FirstOrDefault(c => c.Id == projectController.project.Id) == null)
            {
                _context.Projects.Add(projectController.project);
            }
            else
            {
                Projects projects = _context.Projects.FirstOrDefault(c => c.Id == projectController.project.Id);
                projects.Customer = projectController.project.Customer;
                projects.Title = projectController.project.Title;
                projects.Description = projectController.project.Description;
                projects.PriceInDollars = projectController.project.PriceInDollars;
                projects.isComplite = projectController.project.isComplite;
                projects.BeginTime = projectController.project.BeginTime;
                projects.UntilTime = projectController.project.UntilTime;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Project");
        }


        [HttpGet, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            _context = new ApplicationDbContext();
            if (_context.Projects.FirstOrDefault(c => c.Id == id) == null)
                return RedirectToAction("Index", "Project");

            _context.Projects.Remove(_context.Projects.FirstOrDefault(c => c.Id == id));

            _context.SaveChanges();

            return RedirectToAction("Index", "Project");
        }
    }
}
    
