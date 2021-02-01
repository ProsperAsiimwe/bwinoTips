using MagicApps.Models;
using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Exclusive
{
    public class ExclusiveListViewModel
    {
        public IEnumerable<ExclusiveTip> ExclusiveTips { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public SearchExclusiveViewModel SearchModel { get; set; }

        public string[] Roles { get; set; }

        public bool CurrentUserActive { get; set; }

        public bool CurrentUserNew { get; set; }
    }
}