using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class ChatController : BaseController
    {
        public ChatController()
        {
            ViewBag.Area = "Chat";

        }

        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
    }
}