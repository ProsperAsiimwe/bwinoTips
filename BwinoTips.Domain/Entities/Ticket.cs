using BwinoTips.Domain.Enums;
using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Entities
{
   public class Ticket
    {
        public Ticket()
        {
            Items = new List<TicketItem>();           
            Added = UgandaDateTime.DateNow();
          
        }

        [Key]
        public int TicketId { get; set; }

        //[ForeignKey("League")]
        //public int LeagueId { get; set; }

        public TicketType TicketType { get; set; }

        public DateTime Added { get; set; }

        public virtual ICollection<TicketItem> Items { get; set; }

        //public virtual League League { get; set; }

        [Display(Name = "Total Odds")]
        [NotMapped]
        public double TotalOdds
        {
            get
            {
                return Items.Select(p => p.ExclusiveTip.Odd).Aggregate((x, y) => x * y);
            }
        }

        public bool ContainsItem(string name)
        {
            return Items.Where(p => p.ExclusiveTip.Teams.ToLower().Contains(name)).Count() > 0;
        }

    
    }
}
