using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ASP.NET_PersonControl.Models;
using ASP.NET_PersonControl.ViewModels;

namespace ASP.NET_PersonControl.Controllers
{
   
    public class EmployeesController : Controller
    { public static AspNetUsers usersID = new AspNetUsers();
        public EmployeesContext _context { get; set; } // cennect to data base;

        public EmployeesController()
        {
            _context = new EmployeesContext();
        }

        // GET: Employees
        public ActionResult Index()
        {
            List<AspNetUsers> employees = _context.employeesDBContext.ToList<AspNetUsers>();
            List<AspNetRoles> roles = _context.roles.ToList();
            List<AspNetUserRoles> userRoles = _context.userRoles.ToList();
            //AspNetUserRoles userRoles = _context.userRoles.SingleOrDefault(c => c.USERID == "ce2f2694-a66e-453a-8555-be3d1191b19d");

            //read employees' role
            foreach (AspNetUserRoles userToRole in userRoles)
                if (employees.SingleOrDefault(u => u.Id == userToRole.USERID) != null && roles.SingleOrDefault(r => r.Id == userToRole.ROLEID) != null)
                {
                    employees.SingleOrDefault(u => u.Id == userToRole.USERID).role = roles.SingleOrDefault(r => r.Id == userToRole.ROLEID).NAME;
                    employees.SingleOrDefault(u => u.Id == userToRole.USERID).roleId = roles.SingleOrDefault(r => r.Id == userToRole.ROLEID).Id;
                }

            return View(employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if(id == null)
                return RedirectToAction("Index");

            AspNetUsers employee = _context.employeesDBContext.SingleOrDefault(emp => emp.Id == id);
            if(employee != null)
                return View(employee);
            else
                return RedirectToAction("Index");
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            var empl = new AspNetUsers();

            var viewModel = new EmployeeFormViewModel
            {
                user = empl,
                roles = _context.roles.ToList()
            };

            return View("Create", viewModel);
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
        public ActionResult Save(AspNetUsers user)
        {

            if (!ModelState.IsValid)
            {
                var viewModel = new EmployeeFormViewModel
                {
                    user = user,
                    roles = _context.roles.ToList()
                };

                return View("Create", viewModel);
            }

            if (user.Id == null)
            {
                user.Id = (_context.employeesDBContext.Count() + 1).ToString();
                _context.employeesDBContext.Add(user);
            }
            else
            {
                var userInDB = _context.employeesDBContext.SingleOrDefault(c => c.Id == user.Id);

                if (userInDB == null)
                    userInDB = new AspNetUsers();

                userInDB.EMAIL = user.EMAIL;
                userInDB.EMAILCONFIRMED = user.EMAILCONFIRMED;
                userInDB.PHONENUMBER = user.PHONENUMBER;
                userInDB.PHONENUMBERCONFIRMED = user.PHONENUMBERCONFIRMED;
                userInDB.TWOFACTORENABLED = user.TWOFACTORENABLED;
                userInDB.LOCKOUTENDDATEUTC = user.LOCKOUTENDDATEUTC;
                userInDB.LOCKOUTENABLED = user.LOCKOUTENABLED;
                userInDB.ACCESSFAILEDCOUNT = user.ACCESSFAILEDCOUNT;
                userInDB.USERNAME = user.USERNAME;
                userInDB.COUNTRY = user.COUNTRY;
                userInDB.CITY = user.CITY;
                userInDB.ADDRESS = user.ADDRESS;
            }

            if (user.roleId != null)
                _context.userRoles.Add(new AspNetUserRoles() { ROLEID = user.roleId, USERID = user.Id });

            _context.SaveChanges();

            return RedirectToAction("Index", "Employees");
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            var tempinfo = new  AspNetUsers();
            var empl = _context.employeesDBContext.SingleOrDefault(c => c.Id == id);
            //if (empl != null)
            //{


               // AspNetUserRoles userRoles = _context.userRoles.SingleOrDefault(c => c.USERID == empl.Id);
                var viewModel = new EmployeeFormViewModel()
                {
                    user = empl
                };
            //}
            return View("Edit", viewModel);
        }

        // POST: Employees/Edit/5
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

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            var empl = _context.employeesDBContext.SingleOrDefault(c => c.Id == id);
            if (empl != null)
            {
                _context.employeesDBContext.Remove(empl);

                AspNetUserRoles userRoles = _context.userRoles.SingleOrDefault(c => c.USERID == empl.Id);
                if (userRoles != null)
                        _context.employeesDBContext.Remove(empl);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Employees");
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var bookInDB = _context.employeesDBContext.SingleOrDefault(c => c.Id == id);
                if (bookInDB != null)
                {
                    _context.employeesDBContext.Remove(bookInDB);
                }

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
