using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Entities
{
   public class TicketItem
    {
        [Key]
        public int TicketItemId { get; set; }

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }

        [ForeignKey("ExclusiveTip")]
        public int ExclusiveTipId { get; set; }       

        public virtual ExclusiveTip ExclusiveTip { get; set; }

        public virtual Ticket Ticket { get; set; }
        
    }
}
