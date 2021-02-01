using MagicApps.Models;
using Microsoft.AspNet.Identity.Owin;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Models.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwitterBootstrap3;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    public class LeagueHelper
    {
        private ApplicationDbContext db;
        private ApplicationUserManager UserManager;

        int LeagueId;

        public League League { get; private set; }

        public string ServiceUserId { get; set; }

        public LeagueHelper()
        {
            Set();
        }

        public LeagueHelper(int LeagueId)
        {
            Set();

            this.LeagueId = LeagueId;
            this.League = db.Leagues.Find(LeagueId);
        }

        public LeagueHelper(League League)
        {
            Set();

            this.LeagueId = League.LeagueId;
            this.League = League;
        }

        private void Set()
        {
            this.db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            this.UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }


        public LeagueListViewModel GetLeagueList(SearchLeagueViewModel searchModel, int page = 1)
        {
            int pageSize = 50;

            if (page < 1)
            {
                page = 1;
            }

            IEnumerable<League> records = db.Leagues.ToList();

            // Remove any default information
            //searchModel.ParseRouteInfo();

            if (!String.IsNullOrEmpty(searchModel.Name))
            {
                string name = searchModel.Name.ToLower();
                records = records.Where(x => x.Name.ToLower().Contains(searchModel.Name));
                
            }

            if (!String.IsNullOrEmpty(searchModel.Country))
            {
                string country = searchModel.Country.ToLower();
                records = records.Where(x => x.Country.ToLower().Contains(searchModel.Country));
               
            }

           
            records = records.Where(x => !x.Deleted.HasValue);

            return new LeagueListViewModel
            {
                Leagues = records
                    .OrderByDescending(o => o.Country)
                    .ThenBy(o => o.LeagueId)
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

        public async Task<UpsertModel> UpsertLeague(UpsertMode mode, LeagueViewModel model)
        {
            var upsert = new UpsertModel();

            try
            {
                Activity activity;
                string title;
                System.Text.StringBuilder builder;

                // Apply changes
                League = model.ParseAsEntity(League);

                builder = new System.Text.StringBuilder();

                if (model.LeagueId == 0)
                {
                    db.Leagues.Add(League);

                    title = "League Recorded";
                    builder.Append("A League record has been made").AppendLine();
                }
                else
                {
                    db.Entry(League).State = System.Data.Entity.EntityState.Modified;

                    title = "League Updated";
                    builder.Append("The following changes have been made to the League details");

                    if (mode == UpsertMode.Admin)
                    {
                        builder.Append(" (by the Admin)");
                    }

                    builder.Append(":").AppendLine();
                }

                await db.SaveChangesAsync();

                LeagueId = League.LeagueId;

                // Save activity now so we have a ClassLevelId. Not ideal, but hey
                activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                await db.SaveChangesAsync();

                if (model.LeagueId == 0)
                {
                    upsert.ErrorMsg = "League record created successfully";
                }
                else
                {
                    upsert.ErrorMsg = "League record updated successfully";
                }

                upsert.RecordId = League.LeagueId.ToString();
            }
            catch (Exception ex)
            {
                upsert.ErrorMsg = ex.Message;
                //RecordException("Update ClassLevel Error", ex);
            }

            return upsert;
        }

        public async Task<UpsertModel> DeleteLeague()
        {
            var upsert = new UpsertModel();

            try
            {
                string title = "League Deleted";
                System.Text.StringBuilder builder = new System.Text.StringBuilder()
                    .Append("The following League has been deleted:")
                    .AppendLine()
                    .AppendLine().AppendFormat("League: {0}", League.FullName);

                // Record activity
                var activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                // Remove League
                if (League.FreeTips.Count() <= 0 || League.ExclusiveTips.Count() <= 0)
                {
                    db.Leagues.Remove(League);
                    db.Entry(League).State = System.Data.Entity.EntityState.Deleted;
                }
                else
                {
                    League.Deleted = DateTime.Now;
                    db.Entry(League).State = System.Data.Entity.EntityState.Modified;
                }



                upsert.ErrorMsg = string.Format("League: '{0}' deleted successfully", League.FullName);
                upsert.RecordId = League.LeagueId.ToString();

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

            if (League != null)
            {
                activity.LeagueId = LeagueId;
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

            if (League != null)
            {
                activity.UserId = ServiceUserId;
                activity.LeagueId = League.LeagueId;
            }
            db.SaveChanges();
        }


    }
}