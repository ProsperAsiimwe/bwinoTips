using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Free
{
    public class SearchFreeViewModel : ListModel
    {

        public SearchFreeViewModel()
        {

        }

        [Display(Name = "Home Team")]
        [StringLength(30)]
        public string HomeTeam { get; set; }

        [Display(Name = "Away Team")]
        [StringLength(30)]
        public string AwayTeam { get; set; }

        [UIHint("_DateTimePicker")]
        public DateTime? Date { get; set; }

        public bool IsEmpty()
        {
            if (!String.IsNullOrEmpty(this.HomeTeam) || !String.IsNullOrEmpty(this.AwayTeam) || Date.HasValue)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}