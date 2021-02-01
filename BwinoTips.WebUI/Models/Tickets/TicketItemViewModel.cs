using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Tickets
{
    public class TicketItemViewModel
    {

        public TicketItemViewModel()
        {
            ExclusiveTips = new List<ExclusiveTip>();
        }

        public TicketItemViewModel(TicketItem entity)
        {
            TicketItemId = entity.TicketItemId;
            TicketId = entity.TicketId;
            ExclusiveTipId = entity.ExclusiveTipId;          
        }

        public int TicketItemId { get; set; }      
        public int TicketId { get; set; }       
        public int ExclusiveTipId { get; set; }

        public IEnumerable<ExclusiveTip> ExclusiveTips { get; set; }

    }
}