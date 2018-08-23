using ASP.NET_PersonControl.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
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

        [AcceptVerbs("Get", "Post")]
        public IHttpActionResult HelpInfo()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            //MethodInfo[] methodInfos = Type.GetType(selectedObjcClass).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            Dictionary<int, string> codeOfResponse = new Dictionary<int, string>();
            codeOfResponse.Add(200, "Action accept.");
            codeOfResponse.Add(202, "Action accept.");
            codeOfResponse.Add(302, "Data already exist.");
            codeOfResponse.Add(404, "Page not found.");
            codeOfResponse.Add(405, "Method not allowed.");
            codeOfResponse.Add(406, "Users didn't register.");
            codeOfResponse.Add(417, "Action fail.");
            codeOfResponse.Add(500, "Server internal error."); 
            codeOfResponse.Add(504, "Gateway Timeout");


            try
            {
                //MethodInfo[] methodInfos = typeof(RestApiHelperController).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                result.Add("responses_code", codeOfResponse);
                result.Add("helper_methods", typeof(RestApiHelperController).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList());
                result.Add("roles_methods", typeof(RolesController).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList());
                result.Add("users_methods", typeof(UsersController).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList());
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

        [AcceptVerbs("Get")]
        private IHttpActionResult GenerateTokens()
        {

            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            // To decode the token to get the creation time:

            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                // too old
            }

            return Ok(token);
        }
    }
}