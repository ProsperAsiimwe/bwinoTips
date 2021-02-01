﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicApps.Infrastructure.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using System.IO;

namespace BwinoTips.WebUI.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext context;
        protected ApplicationUserManager UserManager;

        // Create an instance of the context and user manager
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            context = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        protected void ShowStandardError()
        {
            TempData["error"] = CustomHelper.FormatErrorMsg("An error has occurred, please scroll down to view details");
        }

        protected void ShowError(string msg)
        {
            TempData["error"] = msg;
        }

        protected void ShowSuccess(string msg)
        {
            TempData["success"] = msg;
        }

        protected void ShowTryCatchError(string error)
        {
            ModelState.AddModelError(String.Empty, error);
            ShowStandardError();
        }

        protected List<string> GetModelStateErrors()
        {
            return new ControllerHelper(this.ControllerContext).GetModelStateErrors();
        }

        protected void ShowModelStateErrors()
        {
            List<string> errors = GetModelStateErrors();

            foreach (var error in errors) {
                ModelState.AddModelError("", error);
            }
        }

        protected string NewMemberNotifyMsg(ApplicationUser user)
        {
            var controller = new ControllerHelper(this.ControllerContext);
            return controller.RenderRazorViewToString("Mail/_NewUserNotification", user);
        }

        protected string ExpiredMemberNotifyMsg(ApplicationUser user)
        {
            var controller = new ControllerHelper(this.ControllerContext);
            return controller.RenderRazorViewToString("Mail/_ExpiredUserNotification", user);
        }

        protected void ShowModelStateErrorsInMessage()
        {
            List<string> errors = new ControllerHelper(this.ControllerContext).GetModelStateErrors();
            string msg = "An error has occurred<ul><li>" +
                String.Join("</li><li>", errors.ToArray()) +
                "</li></ul>";

            TempData["error"] = CustomHelper.FormatErrorMsg(msg);
        }

        protected void ShowIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        protected void ParseError(Exception ex)
        {
            TempData["error"] = CustomHelper.FormatErrorMsg("An error has occurred: " + ex.Message);
        }

        protected string GetUserId()
        {
            return User.Identity.GetUserId();
        }

        protected ApplicationUser GetUser()
        {
            string userId = GetUserId();
            return context.Users.Find(userId);
        }

        protected ApplicationUser GetUser(string userId)
        {
            return context.Users.Find(userId);
        }

        protected ApplicationUser GetUser(int displayId)
        {
            return context.Users.SingleOrDefault(x => x.DisplayId == displayId);
        }

        protected bool IsAdmin()
        {
            return User.IsInRole("Admin") || User.IsInRole("Developer");
        }

        protected bool IsMember()
        {
            return User.IsInRole("Member");
        }

        protected bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        public static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected string[] GetRoles()
        {
            string[] exclude = { "Developer" };
            string[] roles = context.Roles
                .Where(x => !exclude.Contains(x.Name))
                .OrderBy(o => o.Name)
                .Select(x => x.Name)
                .ToArray();

            return roles;
        }

        //protected Activity CreateActivity(string title, string description, string userId)
        //{
        //    string recordedId = GetUserId();

        //    var activity = new Activity {
        //        Title = title,
        //        Description = description,
        //        RecordedById = recordedId,
        //        UserId = userId
        //    };
        //    context.Activities.Add(activity);
        //    return activity;
        //}
    }
}