﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagicApps.Infrastructure.Services;
using Microsoft.AspNet.Identity.Owin;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    public class MailHelper : EmailService
    {
        private ApplicationDbContext db;
        private string ServiceUserId;

        public int? ReferenceId { get; set; }

        public string UserId { get; set; }

        public MailHelper(string serviceUserId)
        {
            this.db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            this.ServiceUserId = serviceUserId;
        }

        public void RecordErrors()
        {
            if (ErrorCount() > 0) {
                foreach (var item in Errors) {
                    //CreateActivity("Mail Delivery Error", item);
                }
            }
        }

        //private Activity CreateActivity(string title, string description)
        //{
        //    var activity = new Activity {
        //        UserId = UserId,
        //        Title = title,
        //        Description = description,
        //        RecordedById = ServiceUserId,
        //        ReferenceId = ReferenceId
        //    };
        //    db.Activities.Add(activity);
        //    return activity;
        //}
    }
}