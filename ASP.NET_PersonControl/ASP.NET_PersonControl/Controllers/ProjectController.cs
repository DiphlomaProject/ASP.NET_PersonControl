﻿using ASP.NET_PersonControl.Models;
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

            List<Projects> projectsList = _context.Projects.Select(p => p).ToList<Projects>();
            List<Customers> customersList = _context.Customers.Select(c => c).ToList<Customers>();
            var viewModel = new ProjectsFormViewModel()
            {
                projects = projectsList,
                customers = customersList,

            };
            viewModel.customers.Select(p => p.Company).ToList();
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
            List<Customers> customersList = _context.Customers.Select(c => c).ToList<Customers>();
            var viewModel = new ProjectsFormViewModel
            {
                project = new Projects(),
                customers = customersList
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
            if (projectController.project.Description == null || projectController.project.BeginTime == null || projectController.project.UntilTime ==  null || projectController.project.Title == null)
            {
                var viewModel = new ProjectsFormViewModel
                {
                    project = projectController.project,customer = projectController.customer

                };
                return View("Create", viewModel);
            }
            if (_context.Projects.FirstOrDefault(c => c.Id == projectController.project.Id) == null)
            {
                projectController.project.Customer = projectController.customer.Id;
                var result = projectController.project;
                
                _context.Projects.Add(result);
            }
            else
            {
                Projects projects = _context.Projects.FirstOrDefault(c => c.Id == projectController.project.Id);
                
                projects.Customer = projectController.customer.Id;
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
    