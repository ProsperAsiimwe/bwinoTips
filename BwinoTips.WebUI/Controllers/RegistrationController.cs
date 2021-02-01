using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Enums;
using BwinoTips.WebUI.Infrastructure;
using BwinoTips.WebUI.Infrastructure.Helpers;
using BwinoTips.WebUI.Models.Dashboard;
using BwinoTips.WebUI.Models.Registrations;
using BwinoTips.WebUI.Models.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    [RoutePrefix("Registration/{roleName:regex(^(member)s$)?}")]
    public class RegistrationController : BaseController
    {

        public RegistrationController()
        {
            ViewBag.Area = "Registration";

        }

      
        [Route("page-{page:int:min(1):max(999)}", Order = 1)]
        [Route("", Order = 2)]
        public ActionResult Index()
        {
            ViewBag.Active = "Registration";
            return View();
        }


        //[Authorize(Roles = "Developer, Admin")]
        public ActionResult New()
        {
            ViewBag.Active = "Registration";
            var model = new ProfileViewModel();
            model.SetLists(GetUserId());
            ParseDefaults(model);

            return View(model);

                     
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<ActionResult> Create(string roleName, ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.ParseAsEntity(new ApplicationUser());
                // Create a user without a password
                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    string userId = GetUserId();
                    var helper = GetHelper(user);

                    await helper.ResetPassword(model.Password);

                    // Create an activity
                    var activity = helper.CreateActivity("User Account Created", string.Format("User account created for '{1} {2}', {0}", model.PhoneNumber, model.FirstName, model.LastName));
                    activity.UserId = userId;

                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    await context.SaveChangesAsync();
                    
                    roleName = GetRoleName(roleName);

                    // Send the activation email to the user (if they are an admin)
                    // TO DO: When live with time sheets, this should be anyone
                  

                    //ShowSuccess("The user account has been created successfully - the activation email has been sent to the user");
                    //return RedirectToAction("Roles", new { id = user.DisplayId });

                    // Automatically add the user into the role
                    await helper.UpdateRoles(new string[] { roleName });

                    if (roleName == "Member")
                    {
                        
                    }else{
                        ShowSuccess("The user account has been created successfully");
                                              
                        return RedirectToAction("Thanks");
                    }

                }else{
                    ShowIdentityErrors(result);
                }

                // send email to admin
                var mail = GetMailHelper();
                string subject = string.Format("{0} - New Member Created", user.FullName);
                string message = NewMemberNotifyMsg(user);
                string status = string.Join(":", mail.SendMail(subject, message, ConfigurationManager.AppSettings["Settings.Company.Email"]));
                mail.RecordErrors();
            }
            else
            {
                ShowModelStateErrorsInMessage();
            }

            // If we got this far, something failed, redisplay form
            model.SetLists(GetUserId());
            ParseDefaults(model);
            return View("Thanks", model);
        }

        private string GetRoleName(string roleName)
        {
            return UserHelper.SingulariseRoleName(roleName);
        }

        private UserHelper GetHelper(ApplicationUser user)
        {
            var helper = new UserHelper(user);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private void ParseDefaults(ProfileViewModel model)
        {
            //model.Branches = context.Branches.ToList().Where(p => !p.Terminated.HasValue);
        }

        public MailHelper GetMailHelper()
        {
            MailHelper mail = new MailHelper(null);
            mail.UserId = null;

            return mail;
        }

        public PartialViewResult ShowSubStatus()
        {
          
            var model = new SubscriptionStatus();

          
            model.SubscritionDate = GetUser().subDate;
            model.ExpiryDate = GetUser().expDate;

            return PartialView("Dashboard/_SubscriptionStatus", model);
        }


    }
}