using MagicApps.Models;
using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Adverts
{
    public class AdListViewModel
    {
        public IEnumerable<Advert> Adverts { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}