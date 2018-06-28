using ASP.NET_PersonControl.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public class UsersController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // GET api/<controller>
        public HttpResponseMessage GetUsers()
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var jsonData = JsonConvert.SerializeObject(db.Users.ToList());

            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(jsonData),
                StatusCode = HttpStatusCode.Accepted,
                Version = new Version()
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
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
        public void Delete(int id)
        {
        }
    }
}