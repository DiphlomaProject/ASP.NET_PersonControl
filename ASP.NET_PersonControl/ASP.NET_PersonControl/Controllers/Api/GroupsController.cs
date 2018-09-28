using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ASP.NET_PersonControl.Models;
using static ASP.NET_PersonControl.Controllers.Api.UsersController;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public class GroupsController : ApiController
    {
        private bool isTokenValid(string token)
        {
            db = new ApplicationDbContext();

            if (db.Tokens.FirstOrDefault(t => t.token == token) != null)
                return true;
            else
                return false;
        }

        public struct GroupsModel{
            public List<ApplicationUser> owners { get; set; }
            public List<Groups> groups { get; set; }
        }

        private ApplicationDbContext db { get; set; }

        // GET api/<controller>
        [AcceptVerbs("Post")]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult GetGroups(User user)
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

           
            string curUserID = user.id;

            List<Groups> groupsList = (from gr in db.Groups.ToList()
                                       from ug in db.UsersGroups.ToList()
                                       where gr.Id == ug.GroupId && ug.UserId == curUserID
                                       select gr).ToList();
            List<Groups> gWhereUserOwner = (from gr in db.Groups.ToList()
                                            where gr.Owner == curUserID
                                            select gr).ToList();
            groupsList.AddRange(gWhereUserOwner);
            


            List<ApplicationUser> ownersList = new List<ApplicationUser>();
            foreach (String owner_id in groupsList.Select(g => g.Owner).ToList())
                if (db.Users.FirstOrDefault(o => o.Id == owner_id) != null)
                    ownersList.Add(db.Users.FirstOrDefault(o => o.Id == owner_id));

            //OLD CODE
            // List<Groups> groupsList = db.Groups.Select(g => g).ToList<Groups>();
            //_context.Groups.Select(g => g).ToList<Groups>();
            //List<ApplicationUser> ownersList = new List<ApplicationUser>();
            //foreach (String owner_id in groupsList.Select(g => g.Owner).ToList())
            //    if (db.Users.FirstOrDefault(o => o.Id == owner_id) != null)
            //        ownersList.Add(db.Users.FirstOrDefault(o => o.Id == owner_id));
            //OLD CODE
            if (groupsList.Count <= 0 || ownersList.Count <0)
            {
                result.Add("code", HttpStatusCode.NotAcceptable);
                result.Add("groups_count", groupsList.Count);
                result.Add("owners_count", ownersList.Count);
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                return Ok(result);
            }

            result.Add("code", HttpStatusCode.Accepted);
            result.Add("groups_model", new GroupsModel() { groups = groupsList, owners = ownersList });
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }
    }
}