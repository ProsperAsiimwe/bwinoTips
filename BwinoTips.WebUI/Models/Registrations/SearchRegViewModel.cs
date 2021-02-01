using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Registrations
{
    public class SearchRegViewModel : ListModel
    {

        public SearchRegViewModel()
        {

        }

        [StringLength(60)]
        [Display(Name = "Forename(s)")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [StringLength(20)]
        [Display(Name = "Telephone")]
        public string Tel { get; set; }

        [UIHint("_DateTimePicker")]
        public DateTime? Date { get; set; }

        public bool IsEmpty()
        {
            if (!String.IsNullOrEmpty(this.Tel) || !String.IsNullOrEmpty(this.FirstName) || !String.IsNullOrEmpty(this.LastName) || Date.HasValue)
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