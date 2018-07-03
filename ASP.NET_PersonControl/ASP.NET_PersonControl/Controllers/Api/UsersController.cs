using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace ASP.NET_PersonControl.Controllers.Api
{
    //[Authorize]
    public class UsersController : ApiController
    {
        private ApplicationDbContext db { get; set; }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>
        [HttpPost]
        [AcceptVerbs("Post")]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult GetUsers()
        {
            db = new ApplicationDbContext();
            List<ApplicationUser> users = db.Users.ToList();

            if (users == null) return NotFound(); //Ok(new Dictionary<string, object>() { { "code", HttpStatusCode.NoContent } });

            var Roles = db.Roles.Include(r => r.Users).ToList(); // get all roles where we have user on position
            foreach (ApplicationUser user in users)
                user.RoleNames = (from r in Roles
                                  from u in r.Users
                                  where u.UserId == user.Id
                                  select r.Name).ToList();  // get roles, select role from Roles where Roles.userId == user.Id (in asp.net Roles have array-property "Users",
                                                            // 1 item of "Users" = Dictionary<UserId, RoleId>. It's like table AspNetUserRoles.)

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("code", HttpStatusCode.Accepted);
            result.Add("users", users);

            return Ok(result);
        }
        
        public bool isUserExist(string email)
        {
            db = new ApplicationDbContext();
            bool res = db.Users.Where(e => e.Email == email).Count() >= 1;
            return res;
        }

        [HttpPost]
        public IHttpActionResult DeleteUser([FromBody]string Id)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            // TODO: Add delete logic here
            db = new ApplicationDbContext();
            var employee = db.Users.SingleOrDefault(c => c.Id == Id);
            if (employee != null)
            {
                db.Users.Remove(employee);
                db.SaveChanges();
                result.Add("code", HttpStatusCode.Accepted);
            }
            else
            {
                result.Add("code", HttpStatusCode.NotFound);
            }
           
            return Ok(result);
        }


        [HttpGet]
        [AcceptVerbs("Get", "Post")]
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete([FromBody]string id)
        {
        }
    }
}