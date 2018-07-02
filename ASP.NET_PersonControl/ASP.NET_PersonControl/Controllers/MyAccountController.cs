using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Threading.Tasks;
using System.Drawing;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize]
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
        public ActionResult Save(ApplicationUser user)
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            string storageAccountName = "aspnetpersoncontrol";
            string keyOne = "GfiRnxHVXsaluga4L4R0zZOy4Ken4VnF3xM7I66OC263LJ9Sf2BOQgX41+/WpBlA8vMB5aP4wN/Uh00OF4MdXw==";
            string nameOfStorage = "storage";
            StorageCredentials storageCredentials = new StorageCredentials(storageAccountName, keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            //get users folder
           
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(nameOfStorage);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            //get users folder
            _context = new ApplicationDbContext();
            string id = User.Identity.GetUserId();
            string userFolder = ((ApplicationUser)_context.Users.SingleOrDefault(u => u.Id == id)).Email;
            CloudBlobDirectory cloudBlobDirectory = cloudBlobContainer.GetDirectoryReference(userFolder);
            //add file to sub dir
            CloudBlockBlob cloudBlockBlob = cloudBlobDirectory.GetBlockBlobReference("AccountImage.jpg");
            await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);

            return RedirectToAction("Index", "MyAccount");
        }
    }
}
