using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Tickets
{
    public class TicketStatsModel
    {
        public IEnumerable<Ticket> Tickets { get; set; }

        public IEnumerable<Ticket> GetLatest()
        {
            return Tickets
                .OrderByDescending(p => p.Added)
                .Take(5);
        }

        public IEnumerable<Ticket> ThisMonth()
        {
            return Tickets.Where(m => m.Added.Month == DateTime.Today.Month);
        }
    }
}