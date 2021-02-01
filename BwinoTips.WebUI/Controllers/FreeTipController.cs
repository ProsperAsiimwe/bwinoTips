using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Infrastructure;
using BwinoTips.WebUI.Infrastructure.Helpers;
using BwinoTips.WebUI.Models.Free;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class FreeTipController : BaseController
    {

        public FreeTipController()
        {
            ViewBag.Area = "Free Tips";

        }

        // GET: FreeTip
        [Authorize(Roles = "Developer, Admin")]       
        public ActionResult Index(SearchFreeViewModel search, int page = 1)
        {
            // Return all Tips
            // If not a post-back (i.e. initial load) set the searchModel to session
            if (Request.Form.Count <= 0)
            {
                if (search.IsEmpty() && Session["SearchFreeViewModel"] != null)
                {
                    search = (SearchFreeViewModel)Session["SearchFreeViewModel"];
                }
            }

            var helper = new FreeTipHelper();
            var model = helper.GetTipList(search, search.ParsePage(page));

            Session["SearchFreeViewModel"] = search;

            //(search);

            return View(model);
        }

        [Authorize(Roles = "Developer, Admin")]
        public ActionResult New()
        {
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
        public async Task<ActionResult> Create(FreeViewModel model)
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
        [Route("{FreeTipId:int}")]
        public ActionResult Show(int FreeTipId)
        {
            if (!IsRoutingOK(FreeTipId))
            {
                return RedirectOnError();
            }

            var Tip = GetTip(FreeTipId);

            return View(Tip);
        }

        [Authorize(Roles = "Developer, Admin")]
        [Route("{FreeTipId:int}/Edit")]
        public ActionResult Edit(int FreeTipId)
        {
            var model = GetTipModel(FreeTipId);
            return View("New", model);
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int FreeTipId, FreeViewModel model)
        {
            if (!IsRoutingOK(FreeTipId))
            {
                return RedirectOnError();
            }

            bool success = await Upsert(FreeTipId, model);

            if (success)
            {
                return RedirectOnSuccess(FreeTipId);
            }

            // If we got this far, an error occurred
            return View("New", model);
        }

        // GET: FreeTip
        [Authorize(Roles = "Developer, Admin")]
        [Route("{FreeTipId:int}/Delete")]
        public ActionResult Delete(int FreeTipId)
        {
            if (!IsRoutingOK(FreeTipId))
            {
                return RedirectOnError();
            }

            return View(GetTip(FreeTipId));
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Destroy(int FreeTipId)
        {
            if (!IsRoutingOK(FreeTipId))
            {
                return RedirectOnError();
            }

            var helper = GetHelper(FreeTipId);
            var upsert = await helper.DeleteTip();

            if (upsert.i_RecordId() > 0)
            {
                ShowSuccess(upsert.ErrorMsg);
                return RedirectOnError();
            }

            // If we got this far, an error occurred
            ShowError(upsert.ErrorMsg);
            return RedirectToAction("Delete", new { FreeTipId = FreeTipId });
        }



        public ActionResult TipStatus(int? FreeTipId)
        {
            if (FreeTipId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tip = context.FreeTips.SingleOrDefault(p => p.FreeTipId == FreeTipId);
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
                var tip = context.FreeTips.SingleOrDefault(p => p.FreeTipId == model.FreeTipId);
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



        private async Task<bool> Upsert(int? FreeTipId, FreeViewModel model)
        {
            var helper = (FreeTipId.HasValue ? GetHelper(FreeTipId.Value) : new FreeTipHelper() { ServiceUserId = GetUserId() });
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

        private FreeTipHelper GetHelper(int FreeTipId)
        {
            FreeTipHelper helper = new FreeTipHelper(FreeTipId);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private FreeTipHelper GetHelper(FreeTip FreeTip)
        {
            var helper = new FreeTipHelper(FreeTip);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private FreeViewModel GetTipModel(int? FreeTipId)
        {
            FreeViewModel model;


            if (FreeTipId.HasValue)
            {
                var Tip = GetTip(FreeTipId.Value);
                model = new FreeViewModel(Tip);
            }
            else
            {
                model = new FreeViewModel();
            }

            // pass needed lists
            ParseDefaults(model);

            return model;
        }

        public void ParseDefaults(FreeViewModel model)
        {
            model.Leagues = context.Leagues.ToList().Select(p => new SelectListItem { Value = p.LeagueId.ToString(), Text = p.Name });
        }

        private FreeTip GetTip(int FreeTipId)
        {
            return context.FreeTips.Find(FreeTipId);
        }

        private RedirectToRouteResult RedirectOnError()
        {
            return RedirectToAction("Index");
        }

        private RedirectToRouteResult RedirectOnSuccess(int FreeTipId)
        {
            return RedirectToAction("Show", new { FreeTipId = FreeTipId });
        }

        private bool IsRoutingOK(int? FreeTipId)
        {

            // Check Tip
            if (FreeTipId.HasValue)
            {
                var Tip = context.FreeTips.ToList().SingleOrDefault(x => x.FreeTipId == FreeTipId);

                if (Tip == null)
                {
                    return false;
                }
            }

            return true;
        }

        public PartialViewResult GetActivities(int FreeTipId)
        {
            var activities = context.Activities.ToList()
                .Where(x => x.FreeTipId == FreeTipId)
                .OrderBy(o => o.Recorded);

            return PartialView("Partials/_Activities", activities);
        }

    }
}