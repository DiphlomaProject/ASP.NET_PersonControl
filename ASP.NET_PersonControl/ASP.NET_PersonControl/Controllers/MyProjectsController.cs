using ASP.NET_PersonControl.Models;
using ASP.NET_PersonControl.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [HttpPost]
        public ActionResult Upload(List<HttpPostedFileBase> fileData)
        {
            string path = Server.MapPath("~/dragDroptest/");
            foreach (HttpPostedFileBase postedFile in fileData)
            {
                if (postedFile != null)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(path + fileName);
                }
            }

            return Content("Success");
        }
    }
}