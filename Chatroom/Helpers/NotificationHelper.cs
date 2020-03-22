using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Chatroom.Helpers
{
    public class NotificationHelper
    {
        public static void sendMessageToAllClients(string message)
        {
            using (var hubConnection = new HubConnection(ConfigurationManager.AppSettings["Host"]))
            {
                if (!String.IsNullOrEmpty(message))
                {
                    IHubProxy chatProxy = hubConnection.CreateHubProxy("ChatHub");
                    hubConnection.Start().Wait();
                    chatProxy.Invoke("SendMessage", message);
                    hubConnection.Dispose();
                }
            }
        }
    }
}