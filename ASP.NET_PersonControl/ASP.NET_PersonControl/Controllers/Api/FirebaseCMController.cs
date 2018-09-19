using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public class FirebaseCMController : ApiController
    {
       [HttpGet]
       public IHttpActionResult FirebaseNotification(string FCMToken,string TouserId,string Title,string Message)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
                if (FCMToken.Equals("") == true)
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "FCMToken is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                return Ok(result);
            }
            var data = new
            {
                to = FCMToken,
                data = new
                {
                    message = Message,
                    title = Title,
                   // title = "Short name",
                   // userId = "f9bFzZPr9mA:APA91bG2sH1uuHlx_xSnd-xsI3iR6_yrqeQsZ-oc1aU65QjbB6k12JBJNaH6ZvGeZisNAlYysMI6yfY92zC31CtH7B7xfSMfWGQD4YlA5_C4KB7m5WEehFlzJnCMC5DbWlv6esZVy5v0",
                   userId = TouserId,
                    status = true
                }
            };

            SendNotification(data);
            result.Add("code", HttpStatusCode.Accepted);
            result.Add("message", "FCM was send");
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
            return Ok(result);
        }

        public void SendNotification(object data)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            SendNotification(byteArray);
        }


        public void SendNotification(byte[] byteArray)
        {
            try
            {
                string server_api_key = ConfigurationManager.AppSettings["Server_Api_Id"];
                string sender_id = ConfigurationManager.AppSettings["Sender_Id"];

                WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                webRequest.Method = "post";
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add($"Authorization: key={server_api_key}");
                webRequest.Headers.Add($"Sender: id={sender_id}");
                webRequest.ContentLength = byteArray.Length;

                Stream stream = webRequest.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();

                WebResponse webResponse = webRequest.GetResponse();
                stream = webResponse.GetResponseStream();

                StreamReader streamReader = new StreamReader(stream);
                string sResponseFromServer = streamReader.ReadToEnd();

                streamReader.Close();
                webResponse.Close();
                stream.Close();

               
            }
            catch (Exception ex) { }
        }

    }
}