using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ASP.NET_PersonControl.Models;

namespace ASP.NET_PersonControl.Controllers
{
    public class EmployeesController : Controller
    {
        EmployeesContext employeesContext = new EmployeesContext(); // cennect to data base;

        // GET: Employees
        public ActionResult Index()
        {
            List<AspNetUsers> employees = employeesContext.employeesDBContext.ToList<AspNetUsers>();
            return View();
        }

        // GET: Employees/Details/5
        //http://localhost:60071/Employees/Details/7673810d-e10e-4ad0-a8ba-ac6ca89e40cf
        //http://localhost:60071/Employees/Details/5262d911-d7cd-4478-b8ce-40a1adb7b229
        public ActionResult Details(string id)
        {
            if(id == null)
                return RedirectToAction("Index");

            AspNetUsers employee = employeesContext.employeesDBContext.Single(emp => emp.Id == id);
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
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

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employees/Delete/5
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
    }
}
