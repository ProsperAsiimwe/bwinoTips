using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Dashboard
{
    public class MemberStatsModel
    {
        public int TotalRequests { get; set; }

        public int TotalNewMember { get; set; }

        public int TotalActive { get; set; }

        public int TotalExpired { get; set; }

        public int ActiveInactive { get; set; }

        public int ExpiredActive { get; set; }
    }
}