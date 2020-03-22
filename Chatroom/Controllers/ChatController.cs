using Chatroom.Models;
using RabbitMQ.Client;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;
using System.Transactions;
using System.Web.Security;
using Chatroom.Helpers;
using Unity;

namespace Chatroom.Controllers
{
    public class ChatController : Controller
    {
        private readonly IDbHelper db;
        private readonly IMessageBroker obj;
        private readonly IBotHelper bot;

        public ChatController(IDbHelper dba, IMessageBroker obj, IBotHelper bot)
        {
            this.db = dba;
            this.obj = obj;
            this.bot = bot;
        }

        [Authorize]
        public ActionResult Index()
        {
            User authenticatedUser = db.GetUserById(Convert.ToInt32(ChatroomEncrypter.Decrypt(User.Identity.Name)));
            ViewData["Username"] = (authenticatedUser != null) ? authenticatedUser.Username : "";
            obj.suscribe();
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult SendMessage(string message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                if (message.Contains("/stock="))
                {
                    bot.RequestStockQuotes(message);
                }
                else
                {
                    User authenticatedUser = db.GetUserById(Convert.ToInt32(ChatroomEncrypter.Decrypt(User.Identity.Name)));
                    if (obj.send(authenticatedUser.Username + " : " + message))
                    {
                        db.UpdateDatabasePost(message, authenticatedUser.Id);
                    }
                }
            }

            return Json(String.Empty);
        }
    }
}
