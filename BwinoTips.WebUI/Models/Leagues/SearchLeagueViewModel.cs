using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Leagues
{
    public class SearchLeagueViewModel : ListModel
    {
        public SearchLeagueViewModel()
        {

        }

        [Display(Name = "Name")]
        [StringLength(30)]
        public string Name { get; set; }

        [Display(Name = "Country")]
        [StringLength(30)]
        public string Country { get; set; }
        
        public bool IsEmpty()
        {
            if (!String.IsNullOrEmpty(this.Name) || !String.IsNullOrEmpty(this.Country) )
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