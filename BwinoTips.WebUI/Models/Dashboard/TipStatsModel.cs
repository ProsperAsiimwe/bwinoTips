using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Dashboard
{
    public class TipStatsModel
    {

        public int TotalExclisuve { get; set; }

        public int TotalFree { get; set; }

        public int ExclPending { get; set; }

        public int ExclCorrect { get; set; }

        public int ExclWrong { get; set; }

        public int FreePending { get; set; }

        public int FreeCorrect { get; set; }

        public int FreeWrong { get; set; }

    }
}