using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_PersonControl.Models;
using ASP.NET_PersonControl.ViewModels;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Serialization.JsonNet;
using FireSharp.Serialization.ServiceStack;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ASP.NET_PersonControl.Controllers
{
    
    public class HomeController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret= "uACmEbQdfgzBc8a8bnTskK9310sjPgus3PStEsUk",
            BasePath= "https://personscontrol.firebaseio.com/"
        };

        IFirebaseClient client;
        private ApplicationDbContext _context;

        public ActionResult Index()
        {
            _context = new ApplicationDbContext();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string id = User.Identity.GetUserId();

                ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
                if(employee == null)
                    return RedirectToAction("Login", "Account");


                Session["DisplayName"] = employee.DisplayName;
                //get user image
                string storageAccountName = "aspnetpersoncontrol";
                string keyOne = "GfiRnxHVXsaluga4L4R0zZOy4Ken4VnF3xM7I66OC263LJ9Sf2BOQgX41+/WpBlA8vMB5aP4wN/Uh00OF4MdXw==";
                string nameOfStorage = "storage";
                StorageCredentials storageCredentials = new StorageCredentials(storageAccountName, keyOne);
                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                //get users folder
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(nameOfStorage);
                //get users folder
                string userFolder = ((ApplicationUser)_context.Users.SingleOrDefault(u => u.Id == id)).Email;
                CloudBlobDirectory cloudBlobDirectory = cloudBlobContainer.GetDirectoryReference(userFolder);
                //add file to sub dir
                CloudBlockBlob cloudBlockBlob = cloudBlobDirectory.GetBlockBlobReference("AccountImage.jpg");
                try
                {
                    cloudBlockBlob.FetchAttributes();
                    long fileByteLength = cloudBlockBlob.Properties.Length;
                    employee.img = new byte[fileByteLength];
                    for (int i = 0; i < fileByteLength; i++)
                    {
                        employee.img[i] = 0x20;
                    }
                    cloudBlockBlob.DownloadToByteArray(employee.img, 0);
                }
                catch
                {
                    employee.img = new byte[8];
                }
                Session["Image"] = Convert.ToBase64String(employee.img);


            }


            //employee.DisplayName = profileVM.DisplayName;
            client = new FireSharp.FirebaseClient(config);
            if(client!=null)
            {
               // TempData["msg"] = "<script>alert('Connect to DB_FB succesfully');</script>";
            }
            /// to Admin
            List<TasksForUser> tasklist = (from gr in _context.TasksForUser.ToList() where gr.fromUserId == User.Identity.GetUserId() select gr).ToList();
            List<TasksForGroups> tasklistGroups = (from gr in _context.TasksForGroups.ToList() where gr.fromUserId == User.Identity.GetUserId() select gr).ToList();
            List<TasksForProjects> tasklistProjects = (from gr in _context.TasksForProjects.ToList() where gr.fromUserId == User.Identity.GetUserId() select gr).ToList();
            var CountgetTaksAdmin = tasklist.Count() + tasklistGroups.Count() + tasklistProjects.Count();
            //to Employees
            List<TasksForUser> tasklistEmp = (from gr in _context.TasksForUser.ToList() where gr.toUserId == User.Identity.GetUserId() select gr).ToList();
            var CountgetTaskEml = tasklistEmp.Count();

            //
            List<Projects> projectComplete = (from gr in _context.Projects.ToList() where gr.isComplite == true select gr).ToList();
            int price = 0;
            foreach (Projects pj in projectComplete)
                if (pj.PriceInDollars != 0 && pj.isComplite == true)
                    price += pj.PriceInDollars;

            var viewModel = new StatViewModel
            {

                Employees = _context.Users.Select(c => c).ToList(),
                countGetTaskAdmin = CountgetTaksAdmin,
                projectsList = projectComplete,
                countGetTaskEmpl = CountgetTaskEml,
                priceCompliteTotal = price
            };


            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}