using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Models;
using BwinoTips.WebUI.Models;
using BwinoTips.WebUI.Models.Tickets;
using MagicApps.Infrastructure.Helpers;
using MagicApps.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using TwitterBootstrap3;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db;
        private ApplicationUserManager UserManager;

        public int PageSize = 20;

        int TicketId;

        public Ticket Ticket { get; private set; }

        public string ServiceUserId { get; set; }

        public TicketHelper()
        {
            Set();
        }

        public TicketHelper(int TicketId)
        {
            Set();

            this.TicketId = TicketId;
            this.Ticket = db.Tickets.Find(TicketId);
        }

        public TicketHelper(Ticket Ticket)
        {
            Set();

            this.TicketId = Ticket.TicketId;
            this.Ticket = Ticket;
        }

        private void Set()
        {
            this.db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            this.UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public TicketListViewModel GetTicketList(SearchTicketsModel searchModel, FilterModel filterModel)
        {
            filterModel.Init("Date", "DESC", PageSize);

            if (filterModel.Page < 1)
            {
                filterModel.Page = 1;
            }

            var predicate = GetSearchPredicate(searchModel); // Construct your WHERE statement
            var records = GetTickets(predicate, filterModel, filterModel.Page); // Construct your correct, efficient SQL query
            filterModel.TotalItems = db.Tickets.AsNoTracking().Count(predicate); // Gets a total count using the same WHERE statement

            var allRecords = GetTicketsFully(predicate);
            // Convert to models here. SQL execution is still awaiting
            //var dtos = records.Select(x => new TicketViewModel(x));

            return new TicketListViewModel
            {
                Tickets = records,
                SearchModel = searchModel,
                FilterModel = filterModel,
                Records = allRecords,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = filterModel.Page,
                    PageSize = filterModel.PageSize,
                    TotalItems = filterModel.TotalItems
                }
            };
        }

        public async Task<UpsertModel> UpsertTicket(UpsertMode mode, TicketViewModel model, Cart cart)
        {
            var upsert = new UpsertModel();

            try
            {
                Activity activity;
                string title;
                System.Text.StringBuilder builder;
                             

                // Apply changes
                Ticket = model.ParseAsEntity(Ticket);

                builder = new System.Text.StringBuilder();

                if (model.TicketId == 0)
                {
                    db.Tickets.Add(Ticket);

                    title = "Ticket Created";
                    builder.Append(string.Format("A Ticket record has been created.")).AppendLine();
                }
                else
                {
                    db.Entry(Ticket).State = System.Data.Entity.EntityState.Modified;

                    title = "Ticket Updated";
                    builder.Append("The following changes have been made to the Ticket details");

                    if (mode == UpsertMode.Admin)
                    {
                        builder.Append(" (by the Admin)");
                    }

                    builder.Append(":").AppendLine();
                }
                
                await db.SaveChangesAsync();

                TicketId = Ticket.TicketId;

                // store cart items on Ticket. 
                if (cart.Lines.Count() > 0)
                {
                    foreach (var item in cart.Lines)
                    {
                        var TicketItem = new TicketItem
                        {
                            TicketId = TicketId,
                            ExclusiveTipId = item.ExclusiveTip.ExclusiveTipId
                        };

                        db.TicketItems.Add(TicketItem);
                        await db.SaveChangesAsync();
                    }

                }

              
                string rst = string.Empty;

                // Save activity now so we have a TicketId. Not ideal, but hey..Thanks so much for continuing to be part of our Urban family.Enjoy your glow shine
                activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                await db.SaveChangesAsync();

                if (model.TicketId == 0)
                {
                    upsert.ErrorMsg = "Ticket record created successfully." + rst;
                }
                else
                {
                    upsert.ErrorMsg = "Ticket record updated successfully";
                }

                upsert.RecordId = Ticket.TicketId.ToString();

                // clear the cart after adding the items
                cart.Clear();
            }
            catch (Exception ex)
            {
                upsert.ErrorMsg = ex.Message;
            }

            return upsert;
        }

      
        public async Task<UpsertModel> DeleteTicket()
        {
            var upsert = new UpsertModel();

            try
            {
                string title = "Ticket Deleted";
                System.Text.StringBuilder builder = new System.Text.StringBuilder()
                    .Append("The following Ticket has been deleted:")
                    .AppendLine()
                    .AppendLine().AppendFormat("Ticket Type: {0}", Ticket.TicketType.ToString());

                // Record activity
                var activity = CreateActivity(title, builder.ToString());
                activity.UserId = ServiceUserId;

                upsert.ErrorMsg = string.Format("Ticket deleted successfully");
                upsert.RecordId = Ticket.TicketId.ToString();

                // Remove Ticket and items and update stock
                var items = db.TicketItems.Where(p => p.TicketId == TicketId);
                db.TicketItems.RemoveRange(items);
                db.Tickets.Remove(Ticket);

                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                upsert.ErrorMsg = ex.Message;
            }

            return upsert;
        }

       

        // This will allow you to return records from other areas in your application
        public IEnumerable<Ticket> GetTickets(SearchTicketsModel searchModel)
        {
            var predicate = GetSearchPredicate(searchModel);
            var filterModel = new FilterModel("Date");
            return GetTickets(predicate, filterModel);
        }

        // Search the database using your predicate. REMEMBER to use INCLUDE to include your one-one related entities in the same db-call.
        // PS. The filterModel is passed in when using a web-grid where you are sorting by column headings
        private IEnumerable<Ticket> GetTicketsFully(Expression<Func<Ticket, bool>> predicate)
        {
            IEnumerable<Ticket> records;

            records = db.Tickets
                //.AsNoTracking() // This speeds up data retrieval. It is safe to use this if your intention is to only read data. If you intend to do some updates to an entity, do NOT use NoTracking
                //.Include("Client") // Include the related user entity in the db call. The value you use is the property name of your relation
                //.Include("Operator")                  //.Include("OtherRelatedEntity1") // Chain includes like this for all your one-ones
                //.Include("OtherRelatedEntity1.NestedRelatedEntity") // If your related entity contains another one to one you deem important, you can grab it like this, using the dot convention
                .Where(predicate);

            return records;
        }

        private IEnumerable<Ticket> GetTickets(Expression<Func<Ticket, bool>> predicate, FilterModel filterModel, int? page = null)
        {
            IEnumerable<Ticket> records = GetTicketsFully(predicate);

            if (filterModel.SortDir == "DESC")
            {
                records = records.OrderByDescending(GetOrderByExpression(filterModel.Sort));
            }
            else
            {
                records = records.OrderBy(GetOrderByExpression(filterModel.Sort));
            }

            records = records.Skip(page.HasValue ? ((page.Value - 1) * filterModel.PageSize) : 0)
                .Take(page.HasValue ? filterModel.PageSize : int.MaxValue);

            // If you notice we haven't called ToList() yet. ToList() or Count() executes the query. It is important to execute the query at the right time. ALWAYS after
            // your WHERE statement, NEVER before
            return records;
        }

        // Parse your search model using a Predicate builder. This is useful for reusing the same logic to produce your counts
        public Expression<Func<Ticket, bool>> GetSearchPredicate(SearchTicketsModel searchModel)
        {
            var predicate = PredicateBuilder.True<Ticket>();

         
            // Filter by owner Id
            if (!string.IsNullOrEmpty(searchModel.Item))
            {
                string item = searchModel.Item.Trim().ToLower();
                predicate = predicate.And(x => x.ContainsItem(item));
            }
         
            return predicate;
        }

        // This is useful if you want to apply your search to the web grid
        private Func<Ticket, object> GetOrderByExpression(string sort)
        {
            sort = sort.ToLower();

            if (sort == "date")
            {
                return o => o.Added;
            }           
            else
            {
                return o => o.TicketId;
            }

        }

        public Activity CreateActivity(string title, string description)
        {
            var activity = new Activity
            {
                Title = title,
                Description = description,
                RecordedById = ServiceUserId,
                TicketId = TicketId
            };
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
    }
}