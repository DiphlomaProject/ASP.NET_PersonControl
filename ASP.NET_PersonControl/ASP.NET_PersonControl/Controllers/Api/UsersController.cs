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

            return Ok(users);
        }

        [HttpPost]
        public bool isUserExist([FromBody]string email)
        {
            db = new ApplicationDbContext();
            return db.Users.Select(u => u.Email == email).Count() == 1;
        }

        [HttpPost]
        [AcceptVerbs("Post")]
        public HttpResponseMessage GetRoles()
        {
            db = new ApplicationDbContext();
            try
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("code", HttpStatusCode.Accepted);
                result.Add("roles", db.Roles.ToList());

                var jsonSerialiser = new JavaScriptSerializer();
                var jsonData = JsonConvert.SerializeObject(result);

                var resp = new HttpResponseMessage()
                {
                    Content = new StringContent(jsonData),
                    StatusCode = HttpStatusCode.Accepted,
                    Version = new Version()
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }
            catch (Exception ex)
            {

                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("exception", ex);

                var jsonSerialiser = new JavaScriptSerializer();
                var jsonData = JsonConvert.SerializeObject(result);

                var resp = new HttpResponseMessage()
                {
                    Content = new StringContent(jsonData),
                    StatusCode = HttpStatusCode.Accepted,
                    Version = new Version()
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }//catch
        }

        // DELETE api/<controller>/<deleterole>/
        //[HttpPost]
        [ResponseType(typeof(void))]
        //public IHttpActionResult DeleteRole([FromBody]string id)
        public IHttpActionResult DeleteRole(string id)
        {
            db = new ApplicationDbContext();
            IdentityRole role = db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            db.Roles.Remove(role);
            db.SaveChanges();

            return Ok();
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