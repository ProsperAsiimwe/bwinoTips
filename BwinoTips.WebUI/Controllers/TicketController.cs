using BwinoTips.Domain.Enums;
using BwinoTips.Domain.Models;
using BwinoTips.WebUI.Infrastructure.Helpers;
using BwinoTips.WebUI.Models;
using BwinoTips.WebUI.Models.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class TicketController : BaseController
    {
        // GET: Ticket
        public TicketController()
        {
            ViewBag.Area = "Tickets";

        }

       // GET: Tickest
        public ActionResult Index()
        {
            ViewBag.Active = "Tickets";

            var model = new TicketListViewModel
            {
                Tickets = context.Tickets.ToList().Where(p => p.TicketType == TicketType.Exclusive && p.Added.ToShortDateString() == UgandaDateTime.DateNow().ToShortDateString())
            };
            return View(model);
        }

        public PartialViewResult ShowAllTickets(SearchTicketsModel search, int page = 1)
        {
          

            // Return all Tips
            // If not a post-back (i.e. initial load) set the searchModel to session
            if (Request.Form.Count <= 0)
            {
                if (search.IsEmpty() && Session["SearchTicketsModel"] != null)
                {
                    search = (SearchTicketsModel)Session["SearchTicketsModel"];
                }
            }

            var model = new TicketListViewModel
            {
                Tickets = context.Tickets.ToList().Where(presh => presh.TicketType == TicketType.Exclusive)
            };

            //Check if the user is active or not
            model.CurrentUserActive = GetUser().Active;
            model.CurrentUserNew = GetUser().Inactive;

            return PartialView("Dashboard/_AllTickets", model);
        }


        //[Authorize(Roles = "Developer, Admin")]
        //[Route("Page-{page:int}", Order = 13)]
        //public ActionResult Index(SearchTicketsModel search, string method, FilterModel filterModel)
        //{
        //    // Return all Tips
        //    // If not a post-back (i.e. initial load) set the searchModel to session
        //    if (Request.Form.Count <= 0)
        //    {
        //        if (search.IsEmpty() && Session["SearchTicketsModel"] != null)
        //        {
        //            search = (SearchTicketsModel)Session["SearchTicketsModel"];
        //        }
        //    }

        //    var helper = new TicketHelper();
        //    var model = helper.GetTicketList(search, filterModel);

        //    Session["SearchTicketsModel"] = search;

        //    //(search);

        //    return View(model);
        //}


    }
}