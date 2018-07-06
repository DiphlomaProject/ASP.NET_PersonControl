using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_PersonControl.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;

        // GET: Notification
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();
            string id = User.Identity.GetUserId();
            ApplicationUser employee = _context.Users.SingleOrDefault(emp => emp.Id == id);
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
    }
}