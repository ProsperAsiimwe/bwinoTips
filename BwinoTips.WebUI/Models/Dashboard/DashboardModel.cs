using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Dashboard
{
    public class DashboardModel
    {

        //public DashboardModel()
        //{
        //    ExclusiveTips = new List<ExclusiveTip>();
        //}

        public IEnumerable<Activity> Activities { get; set; }

        public IEnumerable<League> Leagues { get; set; }

        public IEnumerable<ExclusiveTip> ExclusiveTips { get; set; }

        public IEnumerable<FreeTip> FreeTips { get; set; }
        
        public IEnumerable<Activity> GetLatestActivity()
        {
            return Activities
                .OrderByDescending(o => o.Recorded)
                .Take(6);
        }

        public string TruncString(string myStr, int THRESHOLD)
        {
            if (myStr.Length > THRESHOLD)
                return myStr.Substring(0, THRESHOLD) + "...";
            return myStr;
        }

        //public double Balance
        //{
        //    get
        //    {
        //        var requisitions = Requisitions.ToList().Where(p => p.HasPay).Sum(p => p.AmountPaid);
        //        var capital = WorkingCapital.Sum(p => p.Amount);

        //        return capital - requisitions;
        //    }
        //}

            
    
    }
}