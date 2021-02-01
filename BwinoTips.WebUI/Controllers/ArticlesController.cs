using Newtonsoft.Json;
using BwinoTips.Domain.Models;
using BwinoTips.WebUI.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class ArticlesController : BaseController
    {
        // GET: Articles
        public ArticlesController()
        {
            ViewBag.Area = "Articles";

        }

        // GET: Articles
        //[Authorize(Roles = "Developer, Admin, Client")]

        //[Route("Page-{page:int}", Order = 13)]
        //[Route("", Order = 21, Name = "Articles_Index")]
        //public ActionResult Index()
        //{
                      
        //    var url = string.Format("https://newsapi.org/v2/everything?q=Soccer+Betting&from={0}&sortBy=popularity&apiKey=dedd35ef4a33423c8c8629813b84b00a", UgandaDateTime.DateNow().AddDays(-5).ToString("yyyy-MM-dd"));


        //    var json = new WebClient().DownloadString(url);

        //    var model = JsonConvert.DeserializeObject<ArticlesViewModel>(json);

        //    model.Articles.AddRange(context.Articles.ToList().Select(x => new NewsViewModel(x)));

        //    model.Articles = model.Articles.OrderByDescending(p => p.PublishedAt).ToList();

        //    return View(model);
        //}
    }
}