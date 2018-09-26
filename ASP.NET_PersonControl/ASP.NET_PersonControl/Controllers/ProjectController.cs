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
using ASP.NET_PersonControl.Controllers.Support_Classes;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Web.Security;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class ProjectController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> RoleManager { get; set; }
        

        public SingletonManager singleton = SingletonManager.getInstance();
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

          //  ListBlobs();
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
            string nameOfStorage = "project";
            StorageCredentials storageCredentials = new StorageCredentials(singleton.storageAccountName, singleton.keyOne);
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


        public void ListBlobs()
        {
            CloudBlobContainer container = GetCloudBlobContainer();
            string con = container.Name;
            List<string> blobs = new List<string>();
            int sessionData = (int)Session["id"];
            string projectID = Convert.ToString(sessionData);
            foreach (IListBlobItem item in container.ListBlobs(useFlatBlobListing: true))
            {
               
                //if(item.Parent.Container.Name == "8")
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    string[] namePart = blob.Name.Split('/');
                    if(namePart!=null && namePart.Count() > 0 && namePart[0] == projectID )
                    blobs.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    blobs.Add(dir.Uri.ToString());
                }
            }
//            @model List<string>
 

// < h2 > List blobs </ h2 >
    

//    < ul >
//        @foreach(var item in Model)
//    {
//    < li >  < a href = "/home/GetFileFromBlob/?id=@item" > @item </ a ></ li >
//    }
//</ ul >

            //return View(blobs);
        }

        private CloudBlobContainer GetCloudBlobContainer()
        {
            
            StorageCredentials storageCredentials = new StorageCredentials(singleton.storageAccountName, singleton.keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("xxxx");

            CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("project");
            
            return c1;

        }

        public ActionResult GetFileFromBlob(string id)
        {

            MemoryStream ms = new MemoryStream();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("xxxxx");

            CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("mycontainer");

            if (c1.Exists())
            {
                CloudBlob file = c1.GetBlobReference(id);

                if (file.Exists())
                {
                    file.DownloadToStreamAsync(ms);
                    Stream blobStream = file.OpenReadAsync().Result;
                    return File(blobStream, file.Properties.ContentType, file.Name);
                }
                else
                {
                    return Content("File does not exist");
                }
            }
            else
            {
                return Content("Dir does not exist");
            }
        }


        public ActionResult Archive()
        {
            _context = new ApplicationDbContext();

            List<Projects> projectsList = _context.Projects.Select(p => p).ToList<Projects>();
            List<Customers> customersList = new List<Customers>();
            foreach (int custID in projectsList.Select(pjId => pjId.Customer))
                if (_context.Customers.ToList().FirstOrDefault(gId => gId.Id == custID) != null)
                    customersList.Add(_context.Customers.ToList().FirstOrDefault(gId => gId.Id == custID));
            var viewModel = new ProjectsFormViewModel()
            {
                projects = projectsList,
                customers = customersList
            };

            //  ListBlobs();
            return View(viewModel);

        }
    }
}
    
