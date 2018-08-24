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
        public IHttpActionResult GetProjects(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(user.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            db = new ApplicationDbContext();
            List<Groups> users = db.Groups.ToList();

            if (users == null) return NotFound(); //Ok(new Dictionary<string, object>() { { "code", HttpStatusCode.NoContent } });

            List<Projects> projectList = db.Projects.Select(p => p).ToList<Projects>();
            List<Customers> customersList = new List<Customers>();
            foreach (int owner_id in projectList.Select(g => g.Customer).ToList())
                if (db.Customers.FirstOrDefault(o => o.Id == owner_id) != null)
                    customersList.Add(db.Customers.FirstOrDefault(o => o.Id == owner_id));

            if (projectList.Count <= 0 || customersList.Count < 0)
            {
                result.Add("code", HttpStatusCode.NotAcceptable);
                result.Add("projects_count", projectList.Count);
                result.Add("customers_count", customersList.Count);
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                return Ok(result);
            }

            result.Add("code", HttpStatusCode.Accepted);
            result.Add("projects_model", new ProjectsModel() { projects = projectList, customers = customersList });
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }
    }
}