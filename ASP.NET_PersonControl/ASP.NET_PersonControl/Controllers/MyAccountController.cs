using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_PersonControl.Controllers
{
    public class MyAccountController : Controller
    {
        
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> roleManager { get; set; }

        public MyAccountController()
        {
            _context = new ApplicationDbContext();

            // Создать хранилище ролей
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());

            // Создать менеджер ролей
            roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        // GET: MyAccount
        public ActionResult Index()
        {
            if (User.Identity.GetUserId() == null)
                return RedirectToAction("Index", "Manage");

            string id = User.Identity.GetUserId();

            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
            if (employee != null)
            {
                employee.RoleNames = (from r in roleManager.Roles.ToList()
                                      from u in r.Users
                                      where u.UserId == employee.Id
                                      select r.Name).ToList();
                return View(employee);
            }
            else
                return RedirectToAction("Index", "Manage");
        }

        // GET: MyAccount/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MyAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyAccount/Create
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

        // GET: MyAccount/Edit/5
        public ActionResult Edit(string id)
        {
            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
            if (employee != null)
            {
                employee.RoleNames = (from r in roleManager.Roles.ToList()
                                      from u in r.Users
                                      where u.UserId == employee.Id
                                      select r.Name).ToList();
                return View(employee);
            }
            else
                return RedirectToAction("Edit", "Manage");
        }

        // POST: MyAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MyAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MyAccount/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(ApplicationUser user, ApplicationUser employeeForm)
        {
            if (user.Id == null)
            {
                user.Id = (_context.Users.Count() + 1).ToString();
                //user.PasswordHash = 
                _context.Users.Add(user);
            }
            else
            {
                var userInDB = _context.Users.SingleOrDefault(c => c.Id == user.Id);

                if (userInDB == null)
                    userInDB = new ApplicationUser();

                userInDB.Email = user.Email;
                userInDB.EmailConfirmed = user.EmailConfirmed;
                userInDB.PhoneNumber = user.PhoneNumber;
                userInDB.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                userInDB.TwoFactorEnabled = user.TwoFactorEnabled;
                userInDB.LockoutEndDateUtc = user.LockoutEndDateUtc;
                userInDB.LockoutEnabled = user.LockoutEnabled;
                userInDB.AccessFailedCount = user.AccessFailedCount;
                userInDB.UserName = user.UserName;
                userInDB.Country = user.Country;
                userInDB.City = user.City;
                userInDB.Address = user.Address;
            }


            _context.SaveChanges();

            return RedirectToAction("Index", "MyAccount");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            /*if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(path);
            }
            */
            return RedirectToAction("Edit", "MyAccount");
        }
    }
}
