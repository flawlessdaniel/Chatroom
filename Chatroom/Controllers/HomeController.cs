using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Chatroom.Models;
using Chatroom.Helpers;
using System.Web.Security;

namespace Chatroom.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbHelper db;

        public HomeController(IDbHelper dba)
        {
            this.db = dba;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UserRegister(User usuario)
        {
            return Json(db.UserRegister(usuario));
        }

        [HttpPost]
        public JsonResult UserLogin(User usuario, string ReturnUrl = "/Chat/Index")
        {
            ResponseHandler isValid = db.UserValidate(usuario);
            if (isValid.Code.Equals(200))
            {
                FormsAuthentication.SetAuthCookie(ChatroomEncrypter.Encrypt(isValid.Msg), false);
            }
            return Json(isValid);
        }

        [HttpPost]
        public JsonResult UserLogOut()
        {
            try
            {
                FormsAuthentication.SignOut();
                return Json(new ResponseHandler(200, "OK"));
            }
            catch (Exception ex)
            {
                return Json(new ResponseHandler(500, ex.Message));
            }
        }
    }

}