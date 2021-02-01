using MagicApps.Models;
using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Free
{
    public class FreeListViewModel
    {
        public IEnumerable<FreeTip> FreeTips { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public SearchFreeViewModel SearchModel { get; set; }

        public string[] Roles { get; set; }
    }
}