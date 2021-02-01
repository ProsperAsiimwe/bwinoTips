using BwinoTips.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Tickets
{
    public class SearchTicketsModel : ListModel
    {
        public SearchTicketsModel()
        {
           
        }

        public TicketType TicketType { get; set; }

        [Display(Name = "Ticket Item")]
        public string Item { get; set; }

        public string Status { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public bool IsEmpty()
        {
            if (!string.IsNullOrEmpty(TicketType.ToString()) || !string.IsNullOrEmpty(Status))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string[] Statuses()
        {
            return new string[] { "Won", "Lost"};
        }

    }
}