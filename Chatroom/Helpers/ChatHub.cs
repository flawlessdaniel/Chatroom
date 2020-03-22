using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Chatroom.Helpers
{
    public class ChatHub : Hub
    {
        public void SendMessage(string msg)
        {
            Clients.All.sendChat(msg);
        }
    }
}