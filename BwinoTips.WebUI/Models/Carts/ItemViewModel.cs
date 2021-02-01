using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Carts
{
    public class ItemViewModel
    {
        public ItemViewModel()
        {

        }

        public int ItemId { get; set; }

        [Required]
        public int ExclusiveTipId { get; set; }

        [Required]
        public double Quantity { get; set; }
    }
}