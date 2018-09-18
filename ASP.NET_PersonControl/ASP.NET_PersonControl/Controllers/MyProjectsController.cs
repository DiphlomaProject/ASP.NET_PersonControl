using ASP.NET_PersonControl.Controllers.Support_Classes;
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
    [Authorize]
    public class MyProjectsController : Controller
    {
        public ApplicationDbContext _context { get; set; } // cennect to data base;
        public RoleManager<IdentityRole> roleManager { get; set; }
        // GET: MyProjects

        public SingletonManager singleton = SingletonManager.getInstance();
        public ActionResult Index()
        {
            _context = new ApplicationDbContext();

            //UsersGroups curUser = _context.Users User.Identity.GetUserId
            string curUserID = User.Identity.GetUserId();

            List<Groups> groupsList = (from gr in _context.Groups.ToList()
                                       from ug in _context.UsersGroups.ToList()
                                       where gr.Id == ug.GroupId && ug.UserId == User.Identity.GetUserId()
                                       select gr).Distinct().ToList();
            List<Groups> gWhereUserOwner = (from gr in _context.Groups.ToList()
                                            where gr.Owner == User.Identity.GetUserId()
                                            select gr).Distinct().ToList();
            foreach(Groups g2 in gWhereUserOwner)
                    if(groupsList.Contains(g2) == false)
                            groupsList.Add(g2);

             List<Projects> projectsList = (from pj in _context.Projects.ToList()
                                            from gl in groupsList
                                            from gp in _context.ProjectsGroups.ToList()
                                            where pj.Id == gp.ProjId && gp.GroupId == gl.Id
                                            select pj).Distinct().ToList();

            groupsList.Clear();
            foreach (Projects pjId in projectsList) {
                List<Groups> grTempl = (from gr in _context.Groups.ToList()
                             from pg in _context.ProjectsGroups.ToList()
                             where pg.GroupId == gr.Id && pjId.Id == pg.ProjId
                             select gr).Distinct().ToList();
                foreach (Groups group in grTempl)
                    if (!groupsList.Contains(group))
                        groupsList.Add(group); 
            }

            Dictionary<int, List<Groups>> dictGroups = new Dictionary<int, List<Groups>>();
            foreach (int id in projectsList.Select(id => id.Id))
            {
                dictGroups.Add(id, (from pg in _context.ProjectsGroups.ToList()
                                    from pj in projectsList
                                    from gr in groupsList
                                    where pj.Id == pg.ProjId && pg.GroupId == gr.Id && pg.ProjId == id
                                    select gr).ToList());
            }
            //Dictionary<int, ApplicationUser> dictGroupsOwners = new Dictionary<int, ApplicationUser>();
            //foreach (int id in projectsList.Select(id => id.Id))
            //{
            //    dictGroups.Add(id, (from pg in _context.ProjectsGroups.ToList()
            //                        from pj in projectsList
            //                        from gr in groupsList
            //                        where pj.Id == pg.ProjId && pg.GroupId == gr.Id && pg.ProjId == id
            //                        select gr).ToList());

            //    for (int i = 0; i < dictGroups[id].Count; i++)
            //    {
            //        Groups group = dictGroups[id][i];
            //        if (dictGroupsOwners.ContainsKey(group.Id) == false)
            //            dictGroupsOwners.Add(group.Id, _context.Users.FirstOrDefault(u => u.Id == group.Owner));
            //    }
            //}

            Dictionary<int, Customers> dictCustomers = new Dictionary<int, Customers>();
            foreach (Projects proj in projectsList.Select(p => p))
                dictCustomers.Add(proj.Id, _context.Customers.FirstOrDefault(c => c.Id == proj.Customer));

            List<Dictionary<int, Customers>> dictCustomersInList = new List<Dictionary<int, Customers>>();
            dictCustomersInList.Add(dictCustomers);
            List<Dictionary<int, List<Groups>>> dictGroupsInList = new List<Dictionary<int, List<Groups>>>();
            dictGroupsInList.Add(dictGroups);

            return View(new ProjectsFormViewModel(){ projectsList = projectsList, groupsInProjectList = dictGroupsInList, customersList = dictCustomersInList });
        }
        public ActionResult ProjectDitail(int id)
        {
            _context = new ApplicationDbContext();
            Session["id"] = id;
            Projects project = _context.Projects.FirstOrDefault(gr => gr.Id == id);
            if (project == null)
                return RedirectToAction("Index", "MyProject");

            Customers customer = _context.Customers.FirstOrDefault(c => c.Id == project.Customer);
            List<Customers> customersList = _context.Customers.Select(c => c).ToList<Customers>();
            List<Groups> groupList = _context.Groups.Select(g => g).ToList<Groups>();
            var projectGroup = (from u in _context.Groups.ToList()
                                from gu in _context.ProjectsGroups.ToList()
                                where gu.ProjId == @project.Id && gu.GroupId == u.Id
                                select u).ToList();
            /////files

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
                    if (namePart != null && namePart.Count() > 0 && namePart[0] == projectID)
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
                project = project,
                customer = customer,
                customers = customersList,
                groups = groupList,
                groupsInProject = projectGroup,
                filelist = blobs


            };
            viewModel.SelectedIDArray = viewModel.groupsInProject.Select(u => u.Id.ToString()).ToArray();
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

            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("xxxxx");
            StorageCredentials storageCredentials = new StorageCredentials(singleton.storageAccountName, singleton.keyOne);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("project");

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
            CloudBlobContainer c1 = BlobClient.GetContainerReference("project");

            if (c1.Exists())
            {
                CloudBlob file = c1.GetBlobReference(id);
                file.Delete();
                
            }
            int sessionData = (int)Session["id"];
            return Redirect("/MyProjects/ProjectDitail/" + sessionData);
        }
    }
}
