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
       public IHttpActionResult FirebaseNotification()
        {
            var data = new
            {
                to = "fhhbCYyDf3M:APA91bGlF5pZF-DMN0ZECfIhTWAeoJxZEt8l-09-E4-F7Llv-COy6gfWVTyjDGy7Z6e96sVVFASFwY_lzIMPAOP6GJ5m84Irunt_6UcuiuSCYbQI1sg_LVYswL4nd_ssQBFB4EAzIa7T",
                data = new
                {
                    message = "Asp.Net FCM Api",
                    title = "Short name",
                    userId = "f9bFzZPr9mA:APA91bG2sH1uuHlx_xSnd-xsI3iR6_yrqeQsZ-oc1aU65QjbB6k12JBJNaH6ZvGeZisNAlYysMI6yfY92zC31CtH7B7xfSMfWGQD4YlA5_C4KB7m5WEehFlzJnCMC5DbWlv6esZVy5v0",
                    status = true
                }
            };

            SendNotification(data);
            return Ok();
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