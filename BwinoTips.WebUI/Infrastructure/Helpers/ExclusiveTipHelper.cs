using MagicApps.Models;
using Microsoft.AspNet.Identity.Owin;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Models.Exclusive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwitterBootstrap3;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    public class ExclusiveTipHelper
    {

        private ApplicationDbContext db;
        private ApplicationUserManager UserManager;

        int ExclusiveTipId;

        public ExclusiveTip ExclusiveTip { get; private set; }

        public string ServiceUserId { get; set; }

        public ExclusiveTipHelper()
        {
            Set();
        }

        public ExclusiveTipHelper(int ExclusiveTipId)
        {
            Set();

            this.ExclusiveTipId = ExclusiveTipId;
            this.ExclusiveTip = db.ExclusiveTips.Find(ExclusiveTipId);
        }

        public ExclusiveTipHelper(ExclusiveTip ExclusiveTip)
        {
            Set();

            this.ExclusiveTipId = ExclusiveTip.ExclusiveTipId;
            this.ExclusiveTip = ExclusiveTip;
        }

        private void Set()
        {
            this.db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            this.UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public ExclusiveListViewModel GetTipList(SearchExclusiveViewModel searchModel, int page = 1)
        {
            int pageSize = 50;

            if (page < 1)
            {
                page = 1;
            }

            IEnumerable<ExclusiveTip> records = db.ExclusiveTips.ToList();

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

            return new ExclusiveListViewModel
            {
                ExclusiveTips = records
                    .OrderByDescending(o => o.Date)
                    .ThenBy(o => o.ExclusiveTipId)
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


        public ExclusiveListViewModel AllTipList(SearchExclusiveViewModel searchModel, int page = 1)
        {
            int pageSize = 50;

            if (page < 1)
            {
                page = 1;
            }

            //var memberRole = db.Roles.FirstOrDefault(p => p.Name == "Member");
            //IEnumerable<ApplicationUser> activeMembers = db.Users.ToList().Where(p => p.GetStatus() == "Active" && p.Roles.Any(x => x.RoleId == memberRole.Id));
            
            //List of all tips in the database
            IEnumerable<ExclusiveTip> records = db.ExclusiveTips.Where(p => p.Result == null).ToList();      
            
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

            return new ExclusiveListViewModel
            {

                ExclusiveTips = records
                    .OrderByDescending(o => o.Date)
                    .ThenBy(o => o.ExclusiveTipId)
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

        public ExclusiveListViewModel PastTipList(SearchExclusiveViewModel searchModel, int page = 1)
        {
            int pageSize = 50;

            if (page < 1)
            {
                page = 1;
            }

            IEnumerable<ExclusiveTip> records = db.ExclusiveTips.Where(p => p.Result != null).ToList();

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

            return new ExclusiveListViewModel
            {
                ExclusiveTips = records
                    .OrderByDescending(o => o.Date)
                    .ThenBy(o => o.ExclusiveTipId)
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



        public async Task<UpsertModel> UpsertTip(UpsertMode mode, ExclusiveViewModel model)
        {
            var upsert = new UpsertModel();

            //try
            //{
                Activity activity;
                string title;
                System.Text.StringBuilder builder;

                // Apply changes
                ExclusiveTip = model.ParseAsEntity(ExclusiveTip);

                builder = new System.Text.StringBuilder();

                if (model.ExclusiveTipId == 0)
                {
                    db.ExclusiveTips.Add(ExclusiveTip);

                    title = "Exclusive Tip Recorded";
                    builder.Append("An Exclusive Tip record has been made").AppendLine();
                }
                else
                {
                    db.Entry(ExclusiveTip).State = System.Data.Entity.EntityState.Modified;

                    title = "Exclusive Tips Updated";
                    builder.Append("The following changes have been made to the Exclusive Tip details");

                    if (mode == UpsertMode.Admin)
                    {
                        builder.Append(" (by the Admin)");
                    }

                    builder.Append(":").AppendLine();
                }

                await db.SaveChangesAsync();

                ExclusiveTipId = ExclusiveTip.ExclusiveTipId;

                // Save activity now so we have a ClassLevelId. Not ideal, but hey
                activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                await db.SaveChangesAsync();

                if (model.ExclusiveTipId == 0)
                {
                    upsert.ErrorMsg = "Exclusive Tip record created successfully";
                }
                else
                {
                    upsert.ErrorMsg = "Exclusive Tip record updated successfully";
                }

                upsert.RecordId = ExclusiveTip.ExclusiveTipId.ToString();
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
                string title = "Exclusive Tip Deleted";
                System.Text.StringBuilder builder = new System.Text.StringBuilder()
                    .Append("The following Exclusive Tip has been deleted:")
                    .AppendLine()
                    .AppendLine().AppendFormat("Exclusive Tip: {0}", ExclusiveTip.TipName);

                // Record activity
                var activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                upsert.ErrorMsg = string.Format("Tip: '{0}' deleted successfully", ExclusiveTip.TipName);
                upsert.RecordId = ExclusiveTip.ExclusiveTipId.ToString();

                // Remove Tip
                db.ExclusiveTips.Remove(ExclusiveTip);
                db.Entry(ExclusiveTip).State = System.Data.Entity.EntityState.Deleted;

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

            if (ExclusiveTip != null)
            {
                activity.ExclusiveTipId = ExclusiveTipId;
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

            if (ExclusiveTip != null)
            {
                activity.UserId = ServiceUserId;
                activity.ExclusiveTipId = ExclusiveTip.ExclusiveTipId;
            }
            db.SaveChanges();
        }


    }
}