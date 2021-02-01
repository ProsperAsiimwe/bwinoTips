using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Carts
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}