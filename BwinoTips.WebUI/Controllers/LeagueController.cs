using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Enums;
using BwinoTips.WebUI.Infrastructure;
using BwinoTips.WebUI.Infrastructure.Helpers;
using BwinoTips.WebUI.Models.Dashboard;
using BwinoTips.WebUI.Models.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    [Authorize]
    [RoutePrefix("League")]
    public class LeagueController : BaseController
    {

        //public LeagueController()
        //{
        //    ViewBag.Area = "Leagues";

        //}

        // GET: League
        [Authorize(Roles = "Developer, Admin")]        
        public ActionResult Index(SearchLeagueViewModel search, int page = 1)
        {
            ViewBag.Active = "League";

            // Return all Leagues
            // If not a post-back (i.e. initial load) set the searchModel to session
            if (Request.Form.Count <= 0)
            {
                if (search.IsEmpty() && Session["SearchLeagueViewModel"] != null)
                {
                    search = (SearchLeagueViewModel)Session["SearchLeagueViewModel"];
                }
            }

            var helper = new LeagueHelper();
            var model = helper.GetLeagueList(search, search.ParsePage(page));

            Session["SearchLeagueViewModel"] = search;

            //(search);

            return View(model);
        }

        [Authorize(Roles = "Developer, Admin")]
        public ActionResult New()
        {
            ViewBag.Active = "League";

            if (!IsRoutingOK(null))
            {
                return RedirectOnError();
            }

            var model = GetLeagueModel(null);

            return View(model);
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeagueViewModel model)
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
        [Route("{LeagueId:int}")]
        public ActionResult Show(int LeagueId)
        {
            ViewBag.Active = "League";

            if (!IsRoutingOK(LeagueId))
            {
                return RedirectOnError();
            }

            var League = GetLeague(LeagueId);

            return View(League);
        }

        [Authorize(Roles = "Developer, Admin")]
        [Route("{LeagueId:int}/Edit")]
        public ActionResult Edit(int LeagueId)
        {
            ViewBag.Active = "League";

            var model = GetLeagueModel(LeagueId);
            return View("New", model);
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int LeagueId, LeagueViewModel model)
        {
            if (!IsRoutingOK(LeagueId))
            {
                return RedirectOnError();
            }

            bool success = await Upsert(LeagueId, model);

            if (success)
            {
                return RedirectOnSuccess(LeagueId);
            }

            // If we got this far, an error occurred
            return View("New", model);
        }

        // GET: League
        [Authorize(Roles = "Developer, Admin")]
        [Route("{LeagueId:int}/Delete")]
        public ActionResult Delete(int LeagueId)
        {
            ViewBag.Active = "League";

            if (!IsRoutingOK(LeagueId))
            {
                return RedirectOnError();
            }

            return View(GetLeague(LeagueId));
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Destroy(int LeagueId)
        {
            if (!IsRoutingOK(LeagueId))
            {
                return RedirectOnError();
            }

            var helper = GetHelper(LeagueId);
            var upsert = await helper.DeleteLeague();

            if (upsert.i_RecordId() > 0)
            {
                ShowSuccess(upsert.ErrorMsg);
                return RedirectOnError();
            }

            // If we got this far, an error occurred
            ShowError(upsert.ErrorMsg);
            return RedirectToAction("Delete", new { LeagueId = LeagueId });
        }

        private async Task<bool> Upsert(int? LeagueId, LeagueViewModel model)
        {
            if (ModelState.IsValid)
            {
                var helper = (LeagueId.HasValue ? GetHelper(LeagueId.Value) : new LeagueHelper() { ServiceUserId = GetUserId() });
                var upsert = await helper.UpsertLeague(UpsertMode.Admin, model);

                if (upsert.i_RecordId() > 0)
                {
                    ShowSuccess(upsert.ErrorMsg);

                    return true;
                }
                else
                {
                    ShowError(upsert.ErrorMsg);
                }
            }

            //model.SetLists();
            return false;

        }

        private LeagueHelper GetHelper(int LeagueId)
        {
            LeagueHelper helper = new LeagueHelper(LeagueId);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private LeagueHelper GetHelper(League League)
        {
            var helper = new LeagueHelper(League);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private LeagueViewModel GetLeagueModel(int? LeagueId)
        {
            LeagueViewModel model;


            if (LeagueId.HasValue)
            {
                var League = GetLeague(LeagueId.Value);
                model = new LeagueViewModel(League);
            }
            else
            {
                model = new LeagueViewModel();
            }

            // pass needed lists
            //ParseDefaults(model);

            return model;
        }

        private League GetLeague(int LeagueId)
        {
            return context.Leagues.Find(LeagueId);
        }

        private RedirectToRouteResult RedirectOnError()
        {
            return RedirectToAction("Index");
        }

        private RedirectToRouteResult RedirectOnSuccess(int LeagueId)
        {
            return RedirectToAction("Show", new { LeagueId = LeagueId });
        }

        private bool IsRoutingOK(int? LeagueId)
        {

            // Check League
            if (LeagueId.HasValue)
            {
                var League = context.Leagues.ToList().SingleOrDefault(x => x.LeagueId == LeagueId);

                if (League == null)
                {
                    return false;
                }
            }

            return true;
        }

        public PartialViewResult GetActivities(int LeagueId)
        {
            var activities = context.Activities.ToList()
                .Where(x => x.LeagueId == LeagueId)
                .OrderBy(o => o.Recorded);

            return PartialView("Partials/_Activities", activities);
        }

        public PartialViewResult GetTipStats()
        {
           
            var model = new TipStatsModel
            {
                TotalExclisuve = context.ExclusiveTips.ToList().Count(),
                TotalFree = context.FreeTips.ToList().Count(),
                ExclPending = context.ExclusiveTips.ToList().Where(p => p.Status == Status.Pending).Count(),
                ExclCorrect = context.ExclusiveTips.ToList().Where(p => p.Status == Status.Correct).Count(),
                ExclWrong = context.ExclusiveTips.ToList().Where(p => p.Status == Status.Wrong).Count(),
                FreePending = context.FreeTips.ToList().Where(p => p.Status == Status.Pending).Count(),
                FreeCorrect = context.FreeTips.ToList().Where(p => p.Status == Status.Correct).Count(),
                FreeWrong = context.FreeTips.ToList().Where(p => p.Status == Status.Wrong).Count(),

            };

            return PartialView("Dashboard/_TipStats", model);
        }
               
    }
}