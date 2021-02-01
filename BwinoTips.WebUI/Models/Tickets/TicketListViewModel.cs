using BwinoTips.Domain.Entities;
using MagicApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Tickets
{
    public class TicketListViewModel
    {

        public TicketListViewModel()
        {
            FilterModel = new FilterModel
            {
                PageSize = 20,
                Sort = "Date",
                SortDir = "DESC"
            };
        }

        public IEnumerable<Ticket> Tickets { get; set; }

        public IEnumerable<Ticket> Records { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public SearchTicketsModel SearchModel { get; set; }

        public FilterModel FilterModel { get; set; }

        public string[] Roles { get; set; }

        public bool CurrentUserActive { get; set; }

        public bool CurrentUserNew { get; set; }

    }
}