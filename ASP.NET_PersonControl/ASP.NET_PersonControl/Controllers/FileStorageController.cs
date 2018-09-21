using ASP.NET_PersonControl.Controllers.Support_Classes;
using ASP.NET_PersonControl.Models;
using ASP.NET_PersonControl.ViewModels;
using Microsoft.AspNet.Identity;
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
    public class FileStorageController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
      
        // GET: MyProjects

        public SingletonManager singleton = SingletonManager.getInstance();
        // GET: FileStorage
        public ActionResult Index()
        {

            /////files

            CloudBlobContainer container = GetCloudBlobContainer();
            string con = container.Name;
            List<string> blobs = new List<string>();
            string userID = User.Identity.GetUserId();
            foreach (IListBlobItem item in container.ListBlobs(useFlatBlobListing: true))
            {

                //if(item.Parent.Container.Name == "8")
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    string[] namePart = blob.Name.Split('/');
                    if (namePart != null && namePart.Count() > 0 && namePart[0] == userID)  
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
            /////files

            var viewModel = new ProjectsFormViewModel
            {
                
                filelist = blobs


            }; 
            return View(viewModel);
          
        }



        [HttpPost]

        public async Task<ActionResult> UploadFiles(HttpPostedFileBase fileData)
        {

           
            string userID = User.Identity.GetUserId();
            string nameOfStorage = "userstorage";
            StorageCredentials storageCredentials = new StorageCredentials(singleton.storageAccountName, singleton.keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();



            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(nameOfStorage);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            _context = new ApplicationDbContext();
            CloudBlobDirectory cloudBlobDirectory = cloudBlobContainer.GetDirectoryReference(userID);
            CloudBlockBlob cloudBlockBlob = cloudBlobDirectory.GetBlockBlobReference(fileData.FileName);
            await cloudBlockBlob.UploadFromStreamAsync(fileData.InputStream);
            return Content("Success");
            
         
        }

   

        private CloudBlobContainer GetCloudBlobContainer()
        {

            StorageCredentials storageCredentials = new StorageCredentials(singleton.storageAccountName, singleton.keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("xxxx");

            CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("userstorage");

            return c1;

        }

        public ActionResult GetFileFromBlob(string id)
        {

            MemoryStream ms = new MemoryStream();

            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("xxxxx");
            StorageCredentials storageCredentials = new StorageCredentials(singleton.storageAccountName, singleton.keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("userstorage");

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

        public ActionResult DeleteFileFromBlob(string id)
        {

            MemoryStream ms = new MemoryStream();

            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("xxxxx");
            StorageCredentials storageCredentials = new StorageCredentials(singleton.storageAccountName, singleton.keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("userstorage");

            if (c1.Exists())
            {
                CloudBlob file = c1.GetBlobReference(id);
                file.Delete();

            }
            
            return Redirect("/FileStorage/Index/");
        }
    }
}
