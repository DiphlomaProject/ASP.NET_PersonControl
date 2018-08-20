
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
    public class CustomersController : Controller
    {
        // GET: Customers
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> roleManager { get; set; }
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();
            List<Customers> customersList = _context.Customers.Select(c => c).ToList<Customers>();
            var viewModel = new CustomersFormViewModel()
            {
                customers = customersList

            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            _context = new ApplicationDbContext();
            Customers customer = _context.Customers.FirstOrDefault(gr => gr.Id == id);
            if (customer == null)
                return RedirectToAction("Index", "Customers");

            var viewModel = new CustomersFormViewModel
            {
                customer = customer
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            _context = new ApplicationDbContext();
            var viewModel = new CustomersFormViewModel
            {
                customer = new Customers()
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
        public ActionResult Save(CustomersFormViewModel customerController)
        {
            _context = new ApplicationDbContext();
            if (customerController.customer.ContactPerson == null || customerController.customer.Phone == null || customerController.customer.Description == null)
            {
                var viewModel = new CustomersFormViewModel
                {
                    customer = customerController.customer

                };
                return View("Create", viewModel);
            }
            if (_context.Customers.FirstOrDefault(c => c.Id == customerController.customer.Id) == null)
            {
                _context.Customers.Add(customerController.customer);
            }
            else
            {
                Customers customers = _context.Customers.FirstOrDefault(c => c.Id == customerController.customer.Id);
                customers.Company = customerController.customer.Company;
                customers.ContactPerson = customerController.customer.ContactPerson;
                customers.Position = customerController.customer.Position;
                customers.Phone = customerController.customer.Phone;
                customers.Description = customerController.customer.Description;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }


        [HttpGet, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            _context = new ApplicationDbContext();
            if (_context.Customers.FirstOrDefault(c => c.Id == id) == null)
                return RedirectToAction("Index", "Customers");

            _context.Customers.Remove(_context.Customers.FirstOrDefault(c => c.Id == id));

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
    }
}