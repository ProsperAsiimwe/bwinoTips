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
    public class FreeTip
    {

        public FreeTip()
        {
            Added = UgandaDateTime.DateNow();
            Status = Status.Pending;
        }

        [Key]
        public int FreeTipId { get; set; }

        [ForeignKey("League")]
        public int LeagueId { get; set; }

        [Display(Name = "Home Team")]
        [StringLength(30)]
        public string HomeTeam { get; set; }

        [Display(Name = "Away Team")]
        [StringLength(30)]
        public string AwayTeam { get; set; }

        [Display(Name = "Tip")]
        [StringLength(30)]
        public string Tip { get; set; }

        [Display(Name = "Result")]
        [StringLength(20)]
        public string Result { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime Added { get; set; }

        public double Odd { get; set; }

        public DateTime? Deleted { get; set; }

        public virtual League League { get; set; }


        [NotMapped]
        [Display(Name = "Tip name")]
        public string TipName
        {
            get
            {
                return string.Format("{0} Vs {1}, Result: {2}, Played on: {3}", HomeTeam, AwayTeam, Status, Date);
            }
        }


        public string GetStatus()
        {
            return Status.ToString();
        }

        public string GetStatusCssClass()
        {
            switch (Status)
            {
                case Status.Pending:
                    return "warning";
                case Status.Correct:
                    return "success";
                case Status.Wrong:
                    return "danger";
                default:
                    return "primary";
            }

        }

    }
}
