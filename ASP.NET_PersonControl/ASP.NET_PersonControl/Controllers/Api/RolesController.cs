using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public class RolesController : ApiController
    {
        private ApplicationDbContext db { get; set; }

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
        [HttpPost]
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

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}