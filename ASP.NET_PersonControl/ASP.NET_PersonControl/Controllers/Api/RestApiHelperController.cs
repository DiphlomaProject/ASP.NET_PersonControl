using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public class RestApiHelperController : ApiController
    {
        // 302 - already exist, 200/202 - accept, 417 - fail, 500 - server error
        public IHttpActionResult HelpInfo()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            //MethodInfo[] methodInfos = Type.GetType(selectedObjcClass).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            try
            {
                result.Add("helper_methods", Type.GetType("RestApiHelperController").GetMethods(BindingFlags.Public | BindingFlags.Instance));
                result.Add("roles_methods", Type.GetType("RolesController").GetMethods(BindingFlags.Public | BindingFlags.Instance));
                result.Add("users_methods", Type.GetType("UsersController").GetMethods(BindingFlags.Public | BindingFlags.Instance));
            }
            catch (Exception ex)
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("exception", ex.ToString());
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                return Ok(result);
            }

            result.Add("code", HttpStatusCode.Accepted);
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
            return Ok(result);
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

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