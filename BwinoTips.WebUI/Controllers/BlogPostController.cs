using MagicApps.Infrastructure.Services;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Models.BlogPosts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class BlogPostController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();


        // IMPLEMENTING C.R.U.D FUNCTIONALITY START

        // GET: BlogPosts/Read
        public ActionResult Read()
        {
            var model = new BlogPostListViewModel
            {
                BlogPosts = ctx.BlogPosts.ToList()
            };
            return View(model);
        }

        //GET: BlogPost
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //The ValidateAntiForgeryToken attribute helps prevent cross-site request forgery attacks. It requires a corresponding Html.AntiForgeryToken()
        public ActionResult Create(BlogPostViewModel b)
        {
            if (ModelState.IsValid)
            {
                using (ctx)
                {
                    var blog = b.ParseAsEntity(new BlogPost());
                    ctx.BlogPosts.Add(blog);
                    ctx.SaveChanges();

                    string folder = @"~/Content/BlogImages";
                    FileService.CreateFolder(folder);

                    string fileName = Path.GetFileNameWithoutExtension(b.BlogImage.FileName);
                    string extension = Path.GetExtension(b.BlogImage.FileName).Substring(1);
                    folder = ConfigurationManager.AppSettings["Settings.Site.ImgFolder"];
                    fileName = string.Format("{0}.{1}", blog.BlogPostId, extension);
                    string path = Path.Combine(folder, fileName);

                    FileService.DeleteFile(path);
                    b.BlogImage.SaveAs(path);

                    string imageFolder = "~/Content/BlogImages";
                    string imageFolderPath = Path.Combine(imageFolder, fileName);

                    blog.FileName = fileName;
                    blog.FolderPath = imageFolderPath;
                    ctx.Entry(blog).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();


                    return RedirectToAction("Read");
                }
            }

            return View();
        }

        // Get: BlogPosts/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost posts = ctx.BlogPosts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost posts = ctx.BlogPosts.Find(id);
            ctx.BlogPosts.Remove(posts);
            ctx.SaveChanges();

            return RedirectToAction("Read");
        }


        //[Route("{BlogPostId:int}/Delete")]
        //public ActionResult Delete(int BlogPostId)
        //{
        //    if (!IsRoutingOK(BlogPostId))
        //    {
        //        return RedirectOnError();
        //    }

        //    System.Text.StringBuilder hBody = new System.Text.StringBuilder();

        //    hBody.AppendLine(@"<h1 class=""animated slideInDown"">Delete Article</h1>")
        //        .AppendLine(string.Format(@"<h3 class=""animated slideInUp"">{0}</h3>", GetBlogPost(BlogPostId).Title));
        //    ViewBag.HeaderText = hBody.ToString();

        //    return View(GetBlogPost(BlogPostId));
        //}

        // POST: BlogPosts/Delete
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{           
        //    try
        //    {
        //        BlogPost blogPost = ctx.BlogPosts.Find(id);
        //        ctx.BlogPosts.Remove(blogPost);
        //        ctx.SaveChanges();
        //    }
        //    catch (DataException/* dex */)
        //    {
        //        //Log the error (uncomment dex variable name and add a line here to write a log.
        //        return RedirectToAction("Index", new { id = id, saveChangesError = true });
        //    }
        //    return RedirectToAction("Create");
        //}


        // IMPLEMENTING C.R.U.D FUNCTIONALITY END

        private BlogPost GetBlogPost(int BlogPostId)
        {
            return ctx.BlogPosts.Find(BlogPostId);
        }

        private bool IsRoutingOK(int? BlogPostId)
        {

            // Check Article
            if (BlogPostId.HasValue)
            {
                var blogPost = ctx.BlogPosts.SingleOrDefault(x => x.BlogPostId == BlogPostId);

                if (blogPost == null)
                {
                    return false;
                }

            }

            return true;
        }

        private RedirectToRouteResult RedirectOnError()
        {
            return RedirectToAction("Read");
        }

    }
}