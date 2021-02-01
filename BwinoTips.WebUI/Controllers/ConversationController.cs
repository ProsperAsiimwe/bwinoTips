using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Models.Conversations;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class ConversationController : BaseController
    {
        // GET: Conversation
        private Pusher pusher;

        //class constructor
        public ConversationController()
        {
            var options = new PusherOptions() { Cluster = "ap2" };

            pusher = new Pusher(
               "786836",
               "72c6d1af74531fe8d1a7",
               "2f3577f85917b2fb973f",
               options
           );
        }

        // GET: Conversation
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return Redirect("/");
            }

            var currentUser = (ApplicationUser)Session["user"];

            ViewBag.allUsers = context.Users.Where(u => u.Email != currentUser.Email)
                                 .ToList();

            ViewBag.currentUser = currentUser;

            return View();
        }

        [Route("Converse/{Contact:int}")]
        public JsonResult ConversationWithContact(int Contact)
        {
            if (Session["user"] == null)
            {
                return Json(new { status = "error", message = "User is not logged in" });
            }

            var currentUser = (ApplicationUser)Session["user"];

            var conversations = new List<Conversation>();
            var ContactId = context.Users.FirstOrDefault(p => p.DisplayId == Contact).Id;

            conversations = context.Conversations.Count() > 0 ? context.Conversations.ToList().
                                  Where(c => (c.ReceiverId == currentUser.Id
                                      && c.SenderId == ContactId) ||
                                      (c.ReceiverId == ContactId
                                      && c.SenderId == currentUser.Id))
                                  .OrderBy(c => c.Created)
                                  .ToList() : new List<Conversation>();

            return Json(
                new { status = "success", data = conversations.Select(p => new ConversationModel(p)) },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult SendMessage()
        {
            if (Session["user"] == null)
            {
                return Json(new { status = "error", message = "User is not logged in" });
            }

            var currentUser = (ApplicationUser)Session["user"];

            string socket_id = Request.Form["socket_id"];
            var contact = Convert.ToInt32(Request.Form["contact"]);
            var receiver = context.Users.FirstOrDefault(p => p.DisplayId == contact);

            Conversation convo = new Conversation
            {
                SenderId = currentUser.Id,
                Message = Request.Form["message"],
                ReceiverId = receiver.Id
            };

            context.Conversations.Add(convo);
            context.SaveChanges();

            var conversationChannel = getConvoChannel(currentUser.DisplayId, contact);

            convo.Sender = currentUser;
            convo.Receiver = receiver;

            var model = new ConversationModel(convo);

            pusher.TriggerAsync(
              conversationChannel,
              "new_message",
              model,
              new TriggerOptions() { SocketId = socket_id });

            return Json(model);
        }

        [HttpPost]
        [Route("Delivered")]
        public JsonResult MessageDelivered(int message_id, string socket_id)
        {
            Conversation convo = context.Conversations.FirstOrDefault(c => c.ConversationId == message_id);
            if (convo != null)
            {
                convo.Status = Conversation.MessageStatus.Delivered;
                context.Entry(convo).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            var model = new ConversationModel(convo);
            //string socket_id = Request.Form["socket_id"];
            var conversationChannel = getConvoChannel(convo.Sender.DisplayId, convo.Receiver.DisplayId);
            pusher.TriggerAsync(
              conversationChannel,
              "message_delivered",
              model,
              new TriggerOptions() { SocketId = socket_id });
            return Json(model);
        }

        private String getConvoChannel(int user_id, int contact_id)
        {
            if (user_id > contact_id)
            {
                return string.Format("private-Conversation-{0}-{1}", contact_id, user_id);
            }

            return string.Format("private-Conversation-{0}-{1}", user_id, contact_id);
        }
    }
}