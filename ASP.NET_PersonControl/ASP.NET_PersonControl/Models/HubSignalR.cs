using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ASP.NET_PersonControl.Models
{
    public class HubSignalR : Hub
    {
        public void SendMessage(string chatId, string message)
        {
            Clients.All.SendMessage(chatId, message);
        }
    }
}