using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using static ASP.NET_PersonControl.Controllers.Api.GroupsController;
using static ASP.NET_PersonControl.Controllers.Api.UsersController;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public struct ProjectAction{
        public string projId { get; set; }
        public string token { get; set; }
        public string userId { get; set; }
    }

    public class ProjectsController : ApiController
    {
        private bool isTokenValid(string token)
        {
            db = new ApplicationDbContext();

            if (db.Tokens.FirstOrDefault(t => t.token == token) != null)
                return true;
            else
                return false;
        }

        public struct ProjectsModel
        {
            public List<Projects> projects { get; set; }
            public List<Customers> customers { get; set; }
        }

        private ApplicationDbContext db { get; set; }

        [AcceptVerbs("Post")]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult GetProjects(ProjectAction projectAction)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(projectAction.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            if(projectAction.userId == null)
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Users id is not correct.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            db = new ApplicationDbContext();
            List<Groups> users = db.Groups.ToList();

            if (users == null) return NotFound(); //Ok(new Dictionary<string, object>() { { "code", HttpStatusCode.NoContent } });

            //List<Projects> projectList = db.Projects.Select(p => p).ToList<Projects>();
            //List<Customers> customersList = new List<Customers>();
            //foreach (int owner_id in projectList.Select(g => g.Customer).ToList())
            //    if (db.Customers.FirstOrDefault(o => o.Id == owner_id) != null && customersList.Contains(db.Customers.FirstOrDefault(o => o.Id == owner_id)) == false)
            //        customersList.Add(db.Customers.FirstOrDefault(o => o.Id == owner_id));

            List<Groups> groupsList = (from gr in db.Groups.ToList()
                                       from ug in db.UsersGroups.ToList()
                                       where gr.Id == ug.GroupId && ug.UserId == projectAction.userId
                                       select gr).Distinct().ToList();
            List<Groups> gWhereUserOwner = (from gr in db.Groups.ToList()
                                            where gr.Owner == projectAction.userId
                                            select gr).Distinct().ToList();
            foreach (Groups g2 in gWhereUserOwner)
                if (groupsList.Contains(g2) == false)
                    groupsList.Add(g2);

            List<Projects> projectsList = (from pj in db.Projects.Where(p => p.isComplite == false).ToList()
                                           from gl in groupsList
                                           from gp in db.ProjectsGroups.ToList()
                                           where pj.Id == gp.ProjId && gp.GroupId == gl.Id
                                           select pj).Distinct().ToList();

            groupsList.Clear();
            foreach (Projects pjId in projectsList)
            {
                List<Groups> grTempl = (from gr in db.Groups.ToList()
                                        from pg in db.ProjectsGroups.ToList()
                                        where pg.GroupId == gr.Id && pjId.Id == pg.ProjId
                                        select gr).Distinct().ToList();
                foreach (Groups group in grTempl)
                    if (!groupsList.Contains(group))
                        groupsList.Add(group);
            }

            Dictionary<int, List<Groups>> dictGroups = new Dictionary<int, List<Groups>>();
            foreach (int id in projectsList.Select(id => id.Id))
            {
                dictGroups.Add(id, (from pg in db.ProjectsGroups.ToList()
                                    from pj in projectsList
                                    from gr in groupsList
                                    where pj.Id == pg.ProjId && pg.GroupId == gr.Id && pg.ProjId == id
                                    select gr).ToList());
            }

            Dictionary<int, Customers> dictCustomers = new Dictionary<int, Customers>();
            foreach (Projects proj in projectsList.Select(p => p))
                dictCustomers.Add(proj.Id, db.Customers.FirstOrDefault(c => c.Id == proj.Customer));

            List<Dictionary<int, Customers>> dictCustomersInList = new List<Dictionary<int, Customers>>();
            dictCustomersInList.Add(dictCustomers);
            List<Dictionary<int, List<Groups>>> dictGroupsInList = new List<Dictionary<int, List<Groups>>>();
            dictGroupsInList.Add(dictGroups);

            //if (projectList.Count <= 0 || customersList.Count < 0)
            //{
            //    result.Add("code", HttpStatusCode.NotAcceptable);
            //    result.Add("projects_count", projectList.Count);
            //    result.Add("customers_count", customersList.Count);
            //    result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
            //    return Ok(result);
            //}

            Dictionary<String, Object> dictRes = new Dictionary<string, object>();
            dictRes.Add("customers", dictCustomersInList);
            dictRes.Add("groups", dictGroupsInList);
            dictRes.Add("projects", projectsList);

            result.Add("code", HttpStatusCode.Accepted);
            result.Add("data", dictRes);
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));



            return Ok(result);
        }

        //[AcceptVerbs("Post")]
        //[ResponseType(typeof(Dictionary<string, object>))]
        //public IHttpActionResult getUsersProjects(User user)
        //{
        //    Dictionary<string, object> result = new Dictionary<string, object>();
        //    result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
        //    if (!isTokenValid(user.token))
        //    {
        //        result.Add("code", HttpStatusCode.ExpectationFailed);
        //        result.Add("message", "Token is not valid.");

        //        return Ok(result);
        //    }


        //    result.Add("code", HttpStatusCode.Accepted);
        //    result.Add("projects_model", new ProjectsModel() { projects = projectList, customers = customersList });
        //    result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

        //    return Ok(result);
        //} 
    }
}