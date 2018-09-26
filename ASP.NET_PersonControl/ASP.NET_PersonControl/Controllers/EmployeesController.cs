using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ASP.NET_PersonControl.Models;
using ASP.NET_PersonControl.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class EmployeesController : Controller
    { 
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> roleManager { get; set; }

        public EmployeesController()
        {
            _context = new ApplicationDbContext();

            // Создать хранилище ролей
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());

            // Создать менеджер ролей
            roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        // GET: Employees
        //public ActionResult Index()
        //{
        //    Создать контекст для работы с БД
        //    var dbContext = new ApplicationDbContext();

        //    var viewModel = new AdministrationFormViewModel
        //    {
        //        Roles = roleManager.Roles.ToList(),     // получить список ролей
        //        Users = dbContext.Users.ToList()        // получить список пользователей
        //    };

        //    var userRoles = _context.Roles.Include(r => r.Users).ToList(); // get all roles where we have user on position
        //    foreach (ApplicationUser user in viewModel.Users)
        //        user.RoleNames = (from r in userRoles
        //                          from u in r.Users
        //                          where u.UserId == user.Id
        //                          select r.Name).ToList();

        //    return View(viewModel);
        //}


        public ActionResult SearchingEmpl (string searching)
        {
            // Создать контекст для работы с БД
            var dbContext = new ApplicationDbContext();


            var viewModel = new AdministrationFormViewModel
            {
                Roles = roleManager.Roles.ToList(),     // получить список ролей
                /*Users = dbContext.Users.ToList()*/
                Users = dbContext.Users.Where(x => x.Email.Contains(searching) || searching == null).ToList()
                // получить список пользователей
            };
            var userRoles = _context.Roles.Include(r => r.Users).ToList(); // get all roles where we have user on position
            foreach (ApplicationUser user in viewModel.Users)
                user.RoleNames = (from r in userRoles
                                  from u in r.Users
                                  where u.UserId == user.Id
                                  select r.Name).ToList();

            return View(viewModel);
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
                return RedirectToAction("Index");

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
                return RedirectToAction("Index");

        }

        public ActionResult DeteilsInfo(string id)
        {
            if (id == null)
                return RedirectToAction("Index");

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
                return RedirectToAction("Index");

        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            var empl = new ApplicationUser();

            var viewModel = new EmployeeFormViewModel
            {
                user = empl,
                Roles = _context.Roles.ToList()
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

        public ActionResult CreateRoleView()
        {
            var viewModel = new NewRoleFormModel
            {
                Roles = _context.Roles.ToList()
            };

            return View(viewModel);
        }

        // GET: /Administration/New
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("NewRole")]
        public async Task<ActionResult> CreateNewRole(NewRoleFormModel role)
        {
            // Создание новой роли
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            await roleManager.CreateAsync(new IdentityRole(role.RoleNeme));

            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("DeleteRole")]
        public async Task<ActionResult> DeleteRoleConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // Удаление роли
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = await roleManager.FindByIdAsync(id);
                await roleManager.DeleteAsync(role);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(ApplicationUser user, EmployeeFormViewModel employeeForm)
        {

            if (!ModelState.IsValid)
            {
                var viewModel = new EmployeeFormViewModel
                {
                    user = user,
                    Roles = _context.Roles.ToList()
                };

                return View("Create", viewModel);
            }

            _context = new ApplicationDbContext();

            if (user.Id == null || _context.Users.SingleOrDefault(u => u.Id == user.Id) == null)
            {
                user.Id = (_context.Users.Count() + 1).ToString();
                user.UserName = user.Email;
                //user.PasswordHash = 
                //_context.Users.Add(user);
                //UserManager.Create(user, user.Email);
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
                userInDB.UserName = user.Email;
                userInDB.Country = user.Country;
                userInDB.City = user.City;
                userInDB.Address = user.Address;
                userInDB.DisplayName = user.DisplayName;
            }


            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));


            if (_context.Users.SingleOrDefault(u => u.Id == user.Id) == null)
            {
                var result = UserManager.Create(user, user.Email);
                if (!result.Succeeded)
                {
                    var viewModel = new EmployeeFormViewModel
                    {
                        user = user,
                        Roles = _context.Roles.ToList()
                    };

                    return View("Create", viewModel);
                }
            }

            if (user.Id != null && employeeForm.RoleId != null)
            {
                _context = new ApplicationDbContext();
                // Создать хранилище ролей
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                // Создать менеджер ролей
                roleManager = new RoleManager<IdentityRole>(roleStore);

               
                //remove from old roles 
                var roles = UserManager.GetRoles(user.Id);
                UserManager.RemoveFromRoles(user.Id, roles.ToArray());
                //add role
                UserManager.AddToRole(user.Id, _context.Roles.SingleOrDefault(r => r.Id == employeeForm.RoleId).Name);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }//catch
            string id = User.Identity.GetUserId();
            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
            Session["DisplayName"] = employee.DisplayName;

            return RedirectToAction("Index", "Employees");
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            var empl = _context.Users.SingleOrDefault(c => c.Id == id);
            //if (empl != null)
            //{


               // AspNetUserRoles userRoles = _context.userRoles.SingleOrDefault(c => c.USERID == empl.Id);
                var viewModel = new EmployeeFormViewModel()
                {
                    user = empl,
                    Roles = _context.Roles.ToList()
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
        [HttpGet, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // Удаление пользователя
                var _context = new ApplicationDbContext();
                var store = new UserStore<ApplicationUser>(_context);
                var _userManager = new ApplicationUserManager(store);

                var user = await _userManager.FindByIdAsync(id);
                var logins = user.Logins;
                var rolesForUser = await _userManager.GetRolesAsync(id);

                // Открытие транзакции для комплексного удаления
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Удалить логин пользователя
                    foreach (var login in logins.ToList())
                    {
                        await _userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    // Удалить пользователя из ролей
                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await _userManager.RemoveFromRoleAsync(user.Id, item);
                        }
                    }

                    // delete reference user - group
                    var usersGroup = from userForDel in _context.UsersGroups
                                     where userForDel.UserId == id
                                     select userForDel;
                    if (usersGroup != null)
                        _context.UsersGroups.RemoveRange(usersGroup);

                    // Удаление пользователя
                    await _userManager.DeleteAsync(user);

                    // Фиксация транзакции удаления
                    transaction.Commit();

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var bookInDB = _context.Users.SingleOrDefault(c => c.Id == id);
                if (bookInDB != null)
                {
                    _context.Users.Remove(bookInDB);
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
