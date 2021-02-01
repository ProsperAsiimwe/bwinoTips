using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Infrastructure;
using BwinoTips.WebUI.Infrastructure.Helpers;
using BwinoTips.WebUI.Models.Exclusive;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BwinoTips.WebUI.Models.Account;

namespace BwinoTips.WebUI.Controllers
{
    [Authorize]
    [RoutePrefix("Games")]
    public class ExclusiveTipController : BaseController
    {

        //public ExclusiveTipController()
        //{
        //    ViewBag.Area = "Exclusive Tips";

        //}

        // GET: Exclusive
        [Authorize(Roles = "Developer, Admin")]       
        public ActionResult Index(SearchExclusiveViewModel search, int page = 1)
        {           
            ViewBag.Active = "ExclusiveTip";

            // Return all Tips
            // If not a post-back (i.e. initial load) set the searchModel to session
            if (Request.Form.Count <= 0)
            {
                if (search.IsEmpty() && Session["SearchExclusiveViewModel"] != null)
                {
                    search = (SearchExclusiveViewModel)Session["SearchExclusiveViewModel"];
                }
            }

            var helper = new ExclusiveTipHelper();
            var model = helper.GetTipList(search, search.ParsePage(page));

            Session["SearchExclusiveViewModel"] = search;

            //(search);

            return View(model);
        }

        public JsonResult Tips()
        {
            return Json(new { tips = context.ExclusiveTips.Count() }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Developer, Admin")]
        public ActionResult New()
        {
            ViewBag.Active = "ExclusiveTip";

            if (!IsRoutingOK(null))
            {
                return RedirectOnError();
            }

            var model = GetTipModel(null);

            return View(model);
        }


        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExclusiveViewModel model)
        {

            if (!IsRoutingOK(null))
            {
                return RedirectOnError();
            }

            bool success = await Upsert(null, model);

            if (success)
            {
                return RedirectOnError();
            }

            return View("New", model);
        }

        [Authorize(Roles = "Developer, Admin")]
        [Route("{ExclusiveTipId:int}")]
        public ActionResult Show(int ExclusiveTipId)
        {
            ViewBag.Active = "ExclusiveTip";

            if (!IsRoutingOK(ExclusiveTipId))
            {
                return RedirectOnError();
            }

            var Tip = GetTip(ExclusiveTipId);

            return View(Tip);
        }

        [Authorize(Roles = "Developer, Admin")]
        [Route("{ExclusiveTipId:int}/Edit")]
        public ActionResult Edit(int ExclusiveTipId)
        {
            ViewBag.Active = "ExclusiveTip";

            var model = GetTipModel(ExclusiveTipId);
            return View("New", model);
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int ExclusiveTipId, ExclusiveViewModel model)
        {
            if (!IsRoutingOK(ExclusiveTipId))
            {
                return RedirectOnError();
            }

            bool success = await Upsert(ExclusiveTipId, model);

            if (success)
            {
                return RedirectOnSuccess(ExclusiveTipId);
            }

            // If we got this far, an error occurred
            return View("New", model);
        }

        // GET: ExclusiveTip
        [Authorize(Roles = "Developer, Admin")]
        [Route("{ExclusiveTipId:int}/Delete")]
        public ActionResult Delete(int ExclusiveTipId)
        {
            ViewBag.Active = "ExclusiveTip";

            if (!IsRoutingOK(ExclusiveTipId))
            {
                return RedirectOnError();
            }

            return View(GetTip(ExclusiveTipId));
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Destroy(int ExclusiveTipId)
        {
            if (!IsRoutingOK(ExclusiveTipId))
            {
                return RedirectOnError();
            }

            var helper = GetHelper(ExclusiveTipId);
            var upsert = await helper.DeleteTip();

            if (upsert.i_RecordId() > 0)
            {
                ShowSuccess(upsert.ErrorMsg);
                return RedirectOnError();
            }

            // If we got this far, an error occurred
            ShowError(upsert.ErrorMsg);
            return RedirectToAction("Delete", new { ExclusiveTipId = ExclusiveTipId });
        }


        public ActionResult TipStatus(int? ExclusiveTipId)
        {
            if (ExclusiveTipId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tip = context.ExclusiveTips.SingleOrDefault(p => p.ExclusiveTipId == ExclusiveTipId);
            if (tip == null)
            {
                return HttpNotFound();
            }

            var model = new TipStatusViewModel(tip);

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateTipStatus(TipStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tip = context.ExclusiveTips.SingleOrDefault(p => p.ExclusiveTipId == model.ExclusiveTipId);
                tip = model.ParseAsEntity(tip);

                context.Entry(tip).State = EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                ShowError("Data unstable");

                return View("TipStatus", model);
            }

            ShowSuccess("Tip status updated successfully");

            return RedirectToAction("Index");
        }
        

        private async Task<bool> Upsert(int? ExclusiveTipId, ExclusiveViewModel model)
        {
            var helper = (ExclusiveTipId.HasValue ? GetHelper(ExclusiveTipId.Value) : new ExclusiveTipHelper() { ServiceUserId = GetUserId() });
            var upsert = await helper.UpsertTip(UpsertMode.Admin, model);

            if (upsert.i_RecordId() > 0)
            {
                ShowSuccess(upsert.ErrorMsg);

                return true;
            }
            else
            {
                ShowError(upsert.ErrorMsg);
            }

            //model.SetLists();
            return false;
        }

        private ExclusiveTipHelper GetHelper(int ExclusiveTipId)
        {
            ExclusiveTipHelper helper = new ExclusiveTipHelper(ExclusiveTipId);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private ExclusiveTipHelper GetHelper(ExclusiveTip ExclusiveTip)
        {
            var helper = new ExclusiveTipHelper(ExclusiveTip);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private ExclusiveViewModel GetTipModel(int? ExclusiveTipId)
        {
            ExclusiveViewModel model;


            if (ExclusiveTipId.HasValue)
            {
                var Tip = GetTip(ExclusiveTipId.Value);
                model = new ExclusiveViewModel(Tip);
            }
            else
            {
                model = new ExclusiveViewModel();
            }

            // pass needed lists
            ParseDefaults(model);

            return model;
        }

        public void ParseDefaults(ExclusiveViewModel model)
        {
            model.Leagues = context.Leagues.ToList().Select(p => new SelectListItem { Value = p.LeagueId.ToString(), Text = p.Name });
        }

        private ExclusiveTip GetTip(int ExclusiveTipId)
        {
            return context.ExclusiveTips.Find(ExclusiveTipId);
        }

        private RedirectToRouteResult RedirectOnError()
        {
            return RedirectToAction("Index");
        }

        private RedirectToRouteResult RedirectOnSuccess(int ExclusiveTipId)
        {
            return RedirectToAction("Show", new { ExclusiveTipId = ExclusiveTipId });
        }

        private bool IsRoutingOK(int? ExclusiveTipId)
        {

            // Check Tip
            if (ExclusiveTipId.HasValue)
            {
                var Tip = context.ExclusiveTips.ToList().SingleOrDefault(x => x.ExclusiveTipId == ExclusiveTipId);

                if (Tip == null)
                {
                    return false;
                }
            }

            return true;
        }

        public PartialViewResult GetActivities(int ExclusiveTipId)
        {
            var activities = context.Activities.ToList()
                .Where(x => x.ExclusiveTipId == ExclusiveTipId)
                .OrderBy(o => o.Recorded);

            return PartialView("Partials/_Activities", activities);
        }

        public PartialViewResult ShowAllTips(SearchExclusiveViewModel search, int page = 1)
        {
            //var memberRole = context.Roles.FirstOrDefault(p => p.Name == "Member");
            //var activeMembers = context.Users.ToList().Where(p => p.GetStatus() == "Active" && p.Roles.Any(x => x.RoleId == memberRole.Id));

            //foreach (ApplicationUser user in activeMembers)
            //{

            //}
                      
                                 
            // Return all Tips
            // If not a post-back (i.e. initial load) set the searchModel to session
            if (Request.Form.Count <= 0)
            {
                if (search.IsEmpty() && Session["SearchExclusiveViewModel"] != null)
                {
                    search = (SearchExclusiveViewModel)Session["SearchExclusiveViewModel"];
                }
            }

            var helper = new ExclusiveTipHelper();
            var model = helper.AllTipList(search, search.ParsePage(page));

            Session["SearchExclusiveViewModel"] = search;

           //Check if the user is active or not
            model.CurrentUserActive = GetUser().Active;
            model.CurrentUserNew = GetUser().Inactive;

            return PartialView("Dashboard/_AllTips", model);
        }

      
        public ActionResult PastPredictions(SearchExclusiveViewModel search, int page = 1)
        {
            // Return all Tips
            // If not a post-back (i.e. initial load) set the searchModel to session
            if (Request.Form.Count <= 0)
            {
                if (search.IsEmpty() && Session["SearchExclusiveViewModel"] != null)
                {
                    search = (SearchExclusiveViewModel)Session["SearchExclusiveViewModel"];
                }
            }

            var helper = new ExclusiveTipHelper();
            var model = helper.PastTipList(search, search.ParsePage(page));

            Session["SearchExclusiveViewModel"] = search;

            //(search);

            return View(model);
        }

        public PartialViewResult GetTicketItems()
        {
            return PartialView("Partials/_Basket");
        }

    }
}