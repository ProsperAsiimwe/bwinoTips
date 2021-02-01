using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BwinoTips.Domain.Entities;

namespace BwinoTips.WebUI.Models.Highlights
{
    public class HighlightListViewModel
    {
        public IEnumerable<Highlight> Highlights { get; set; }
    }
}