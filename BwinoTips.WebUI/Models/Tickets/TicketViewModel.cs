using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Enums;
using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Models.Tickets
{
    public class TicketViewModel
    {
        public TicketViewModel()
        {
              TotalAmount = 0;
              TicketType = TicketType.Exclusive;
            Leagues = new List<SelectListItem>();
        }

        public TicketViewModel(Ticket Ticket)
        {
            setFromEntity(Ticket);
        }

        public int TicketId { get; set; }
       
        //public int LeagueId { get; set; }

        public TicketType TicketType { get; set; }

        public double? TotalAmount { get; set; }

        public IEnumerable<SelectListItem> Leagues { get; set; }

        public Ticket ParseAsEntity(Ticket Ticket)
        {
            if (Ticket == null)
            {
                Ticket = new Ticket();
            }

            //Ticket.LeagueId = LeagueId;
            Ticket.TicketType = TicketType;          

            return Ticket;
        }

        public void setFromEntity(Ticket Ticket)
        {
            this.TicketId = Ticket.TicketId;
            //this.LeagueId = Ticket.LeagueId;
            this.TicketType = Ticket.TicketType;
        }
    }
}