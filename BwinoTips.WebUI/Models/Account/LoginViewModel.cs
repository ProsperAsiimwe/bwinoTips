using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [UIHint("_PhoneNo")]
        [StringLength(20)]
        [Display(Name = "Telephone No")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}