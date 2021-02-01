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
    public class League
    {
        public League()
        {

            FreeTips = new List<FreeTip>();
            ExclusiveTips = new List<ExclusiveTip>();
            Added = UgandaDateTime.DateNow();
        }

        [Key]
        public int LeagueId { get; set; }

        [Display(Name = "Name")]
        [StringLength(30)]
        public string Name { get; set; }

        [Display(Name = "Country")]
        [StringLength(30)]
        public string Country { get; set; }

        public DateTime Added { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime Updated { get; set; }

        public DateTime? Deleted { get; set; }

        public virtual ICollection<FreeTip> FreeTips { get; set; }

        public virtual ICollection<ExclusiveTip> ExclusiveTips { get; set; }

        [NotMapped]
        [Display(Name = "Full name")]
        public string FullName
        {
            get
            {
                return string.Format("{0} - {1}", Name, Country);
            }
        }


        public int ExclPending()
        {
            return ExclusiveTips.Where(p => p.Status.Equals("Pending")).Count();
        }


        public bool HasPendingFreeTips()
        {
            string status = "Pending";
            var pending = FreeTips.Where(x => x.Status.ToString() == status);
            return pending.Count() > 0;
        }

        public bool HasCorrectFreeTips()
        {
            string status = "Correct";
            var pending = FreeTips.Where(x => x.Status.ToString() == status);
            return pending.Count() > 0;
        }

        public bool HasWrongFreeTips()
        {
            string status = "Wrong";
            var pending = FreeTips.Where(x => x.Status.ToString() == status);
            return pending.Count() > 0;
        }

        public bool HasPendingExclTips()
        {
            string status = "Pending";
            var pending = FreeTips.Where(x => x.Status.ToString() == status);
            return pending.Count() > 0;
        }

        public bool HasCorrectExclTips()
        {
            string status = "Correct";
            var pending = FreeTips.Where(x => x.Status.ToString() == status);
            return pending.Count() > 0;
        }

        public bool HasWrongExclTips()
        {
            string status = "Wrong";
            var pending = FreeTips.Where(x => x.Status.ToString() == status);
            return pending.Count() > 0;
        }

    }
}
