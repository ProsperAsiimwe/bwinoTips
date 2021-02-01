using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class PaymentController : BaseController
    {
        // GET: Payments
        public ActionResult Index()
        {
            return View();
        }
    }
}