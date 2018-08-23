using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using static ASP.NET_PersonControl.Controllers.Api.UsersController;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public class RolesController : ApiController
    {
        private ApplicationDbContext db { get; set; }

        [HttpPost]
        [AcceptVerbs("Post")]
        public HttpResponseMessage GetRoles(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(user.token))
            {
                result.Add("message", "Token is not valid.");
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                var jsonSerialiser = new JavaScriptSerializer();
                var jsonData = JsonConvert.SerializeObject(result);

                var resp = new HttpResponseMessage()
                {
                    Content = new StringContent(jsonData),
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Version = new Version()
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                return resp;
            }

            db = new ApplicationDbContext();
            try
            {
                result.Add("code", HttpStatusCode.Accepted);
                result.Add("roles", db.Roles.ToList());
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

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
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                result.Add("exception", ex.ToString());

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
        public IHttpActionResult DeleteRole(User user, string id)
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
            IdentityRole role = db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            db.Roles.Remove(role);
            db.SaveChanges();

            result.Add("code", HttpStatusCode.Accepted);
            result.Add("message", "Role was delete.");
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok();
        }

        private bool isTokenValid(string token)
        {
            db = new ApplicationDbContext();

            if (db.Tokens.FirstOrDefault(t => t.token == token) != null)
                return true;
            else
                return false;
        }
    }
}