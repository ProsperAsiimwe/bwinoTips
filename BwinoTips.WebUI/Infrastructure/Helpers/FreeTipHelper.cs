using MagicApps.Models;
using Microsoft.AspNet.Identity.Owin;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Models.Free;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwitterBootstrap3;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    public class FreeTipHelper
    {

        private ApplicationDbContext db;
        private ApplicationUserManager UserManager;

        int FreeTipId;

        public FreeTip FreeTip { get; private set; }

        public string ServiceUserId { get; set; }

        public FreeTipHelper()
        {
            Set();
        }

        public FreeTipHelper(int FreeTipId)
        {
            Set();

            this.FreeTipId = FreeTipId;
            this.FreeTip = db.FreeTips.Find(FreeTipId);
        }

        public FreeTipHelper(FreeTip FreeTip)
        {
            Set();

            this.FreeTipId = FreeTip.FreeTipId;
            this.FreeTip = FreeTip;
        }

        private void Set()
        {
            this.db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            this.UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public FreeListViewModel GetTipList(SearchFreeViewModel searchModel, int page = 1)
        {
            int pageSize = 50;

            if (page < 1)
            {
                page = 1;
            }

            IEnumerable<FreeTip> records = db.FreeTips.ToList();

            // Remove any default information
            //searchModel.ParseRouteInfo();

            if (!String.IsNullOrEmpty(searchModel.AwayTeam))
            {
                string name = searchModel.AwayTeam.ToLower();
                records = records.Where(x => x.AwayTeam.ToLower().Contains(searchModel.AwayTeam));
            }

            if (!String.IsNullOrEmpty(searchModel.HomeTeam))
            {
                string name = searchModel.HomeTeam.ToLower();
                records = records.Where(x => x.HomeTeam.ToLower().Contains(searchModel.HomeTeam));
            }

            if (searchModel.Date.HasValue)
            {
                records = records.Where(x => x.Date == searchModel.Date);
            }


            records = records.Where(x => !x.Deleted.HasValue);

            return new FreeListViewModel
            {
                FreeTips = records
                    .OrderByDescending(o => o.Date)
                    .ThenBy(o => o.FreeTipId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                SearchModel = searchModel,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = records.Count()
                }
            };
        }


        public async Task<UpsertModel> UpsertTip(UpsertMode mode, FreeViewModel model)
        {
            var upsert = new UpsertModel();

            //try
            //{
                Activity activity;
                string title;
                System.Text.StringBuilder builder;

                // Apply changes
                FreeTip = model.ParseAsEntity(FreeTip);

                builder = new System.Text.StringBuilder();

                if (model.FreeTipId == 0)
                {
                    db.FreeTips.Add(FreeTip);

                    title = "Free Tip Recorded";
                    builder.Append("An Free Tip record has been made").AppendLine();
                }
                else
                {
                    db.Entry(FreeTip).State = System.Data.Entity.EntityState.Modified;

                    title = "Free Tips Updated";
                    builder.Append("The following changes have been made to the Free Tip details");

                    if (mode == UpsertMode.Admin)
                    {
                        builder.Append(" (by the Admin)");
                    }

                    builder.Append(":").AppendLine();
                }

                await db.SaveChangesAsync();

                FreeTipId = FreeTip.FreeTipId;

                // Save activity now so we have a ClassLevelId. Not ideal, but hey
                activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                await db.SaveChangesAsync();

                if (model.FreeTipId == 0)
                {
                    upsert.ErrorMsg = "Free Tip record created successfully";
                }
                else
                {
                    upsert.ErrorMsg = "Free Tip record updated successfully";
                }

                upsert.RecordId = FreeTip.FreeTipId.ToString();
            //}
            //catch (Exception ex)
            //{
            //    upsert.ErrorMsg = ex.Message;
            //    //RecordException("Update ClassLevel Error", ex);
            //}

            return upsert;
        }

        public async Task<UpsertModel> DeleteTip()
        {
            var upsert = new UpsertModel();

            try
            {
                string title = "Free Tip Deleted";
                System.Text.StringBuilder builder = new System.Text.StringBuilder()
                    .Append("The following Free Tip has been deleted:")
                    .AppendLine()
                    .AppendLine().AppendFormat("Free Tip: {0}", FreeTip.TipName);

                // Record activity
                var activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                upsert.ErrorMsg = string.Format("Tip: '{0}' deleted successfully", FreeTip.TipName);
                upsert.RecordId = FreeTip.FreeTipId.ToString();

                // Remove Tip
                db.FreeTips.Remove(FreeTip);
                db.Entry(FreeTip).State = System.Data.Entity.EntityState.Deleted;

                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                upsert.ErrorMsg = ex.Message;
                //RecordException("Delete ClassLevel Error", ex);
            }

            return upsert;
        }

        public Activity CreateActivity(string title, string description)
        {
            var activity = new Activity
            {
                Title = title,
                Description = description,
                RecordedById = ServiceUserId
            };

            if (FreeTip != null)
            {
                activity.FreeTipId = FreeTipId;
            }

            db.Activities.Add(activity);
            return activity;
        }

        public static ButtonStyle GetButtonStyle(string css)
        {
            ButtonStyle button_css;

            if (css == "warning")
            {
                button_css = ButtonStyle.Warning;
            }
            else if (css == "success")
            {
                button_css = ButtonStyle.Success;
            }
            else if (css == "info")
            {
                button_css = ButtonStyle.Info;
            }
            else
            {
                button_css = ButtonStyle.Danger;
            }

            return button_css;
        }

        private void RecordException(string title, Exception ex)
        {
            var activity = CreateActivity(title, ex.Message);

            if (FreeTip != null)
            {
                activity.UserId = ServiceUserId;
                activity.FreeTipId = FreeTip.FreeTipId;
            }
            db.SaveChanges();
        }


    }
}