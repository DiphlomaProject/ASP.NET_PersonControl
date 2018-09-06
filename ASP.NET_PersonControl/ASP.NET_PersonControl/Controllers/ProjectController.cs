using ASP.NET_PersonControl.Models;
using ASP.NET_PersonControl.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class ProjectController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> RoleManager { get; set; }
        // GET: Project
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();

            List<Projects> projectsList = _context.Projects.Select(p => p).ToList<Projects>();
            /*List<Customers> customersList = (from c in _context.Customers.ToList()
                                             from pj in _context.Projects.ToList()
                                             where c.Id == pj.Customer
                                             select c).ToList();*/
            List<Customers> customersList = new List<Customers>();
            foreach (int custID in projectsList.Select(pjId => pjId.Customer))
                if (_context.Customers.ToList().FirstOrDefault(gId => gId.Id == custID) != null)
                    customersList.Add(_context.Customers.ToList().FirstOrDefault(gId => gId.Id == custID));

            var viewModel = new ProjectsFormViewModel()
            {
                projects = projectsList,
                customers = customersList
            };
          
            
            return View(viewModel);
            
        }

        public ActionResult Edit(int id)
        {
            _context = new ApplicationDbContext();
            Session["id"] = id;
            Projects project = _context.Projects.FirstOrDefault(gr => gr.Id == id);
            if (project == null)
                return RedirectToAction("Index", "Project");

            Customers customer = _context.Customers.FirstOrDefault(c => c.Id == project.Customer);
            List<Customers> customersList = _context.Customers.Select(c => c).ToList<Customers>();
            List<Groups> groupList = _context.Groups.Select(g => g).ToList<Groups>();
            var projectGroup = (from u in _context.Groups.ToList()
                              from gu in _context.ProjectsGroups.ToList()
                              where gu.ProjId == @project.Id && gu.GroupId == u.Id
                              select u).ToList();
            var viewModel = new ProjectsFormViewModel
            {
                project = project,customer = customer,
                customers = customersList,
                groups = groupList,
                groupsInProject = projectGroup


            };
            viewModel.SelectedIDArray = viewModel.groupsInProject.Select(u => u.Id.ToString()).ToArray();
            return View(viewModel);
        }

        public ActionResult Create()
        {
            _context = new ApplicationDbContext();
            List<Customers> customersList = _context.Customers.Select(c => c).ToList<Customers>();
            List<Groups> groupList = _context.Groups.Select(g => g).ToList<Groups>();
            var viewModel = new ProjectsFormViewModel
            {
                project = new Projects(),
                customers = customersList,
                groups = groupList
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


            _context.ProjectsGroups.RemoveRange(_context.ProjectsGroups.Select(ug => ug).Where(ug => ug.ProjId == projectController.project.Id).ToList());
            _context.SaveChanges();
            if (projectController.SelectedIDArray != null && projectController.SelectedIDArray.Count() > 0)
            {
                _context = new ApplicationDbContext();
                if (_context.Projects.FirstOrDefault(g => g.Id == projectController.project.Id) != null)
                {
                    foreach (string id in projectController.SelectedIDArray)
                        if (_context.Groups.FirstOrDefault(u => u.Id.ToString() == id) != null && _context.ProjectsGroups.Select(ug => ug).Where(g => g.GroupId.ToString() == id && g.ProjId == projectController.project.Id).Count() == 0)
                            _context.ProjectsGroups.Add(new ProjectsGroups() { ProjId = projectController.project.Id, GroupId =  Convert.ToInt32(id) });
                    _context.SaveChanges();
                }
            }
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

        public ActionResult DeteilCustomer(int id)
        {
            _context = new ApplicationDbContext();
            Customers customer = _context.Customers.FirstOrDefault(gr => gr.Id == id);
            if (customer == null)
                return RedirectToAction("Index", "Project");

            var viewModel = new CustomersFormViewModel
            {
                customer = customer
            };

            return View(viewModel);
        }



        [HttpPost]
      
        public async Task<ActionResult> UploadFiles(HttpPostedFileBase fileData)
        {
          
            int sessionData = (int)Session["id"];
            string projectID = Convert.ToString(sessionData);
            
            string storageAccountName = "aspnetpersoncontrol";
            string keyOne = "GfiRnxHVXsaluga4L4R0zZOy4Ken4VnF3xM7I66OC263LJ9Sf2BOQgX41+/WpBlA8vMB5aP4wN/Uh00OF4MdXw==";
            string nameOfStorage = "project";
            StorageCredentials storageCredentials = new StorageCredentials(storageAccountName, keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

       

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(nameOfStorage);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            _context = new ApplicationDbContext();
            CloudBlobDirectory cloudBlobDirectory = cloudBlobContainer.GetDirectoryReference(projectID);
            CloudBlockBlob cloudBlockBlob = cloudBlobDirectory.GetBlockBlobReference(fileData.FileName);
            await cloudBlockBlob.UploadFromStreamAsync(fileData.InputStream);
            return Content("Success"); 

        }
    }
}
    
