using MagicApps.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Models.Highlights;

namespace BwinoTips.WebUI.Controllers
{
    public class HighlightController : BaseController
    {
        public HighlightController()
        {
            ViewBag.Area = "Highlights";

        }

        // GET: Highlight
        public ActionResult Index()
        {
            var model = new HighlightListViewModel
            {
                Highlights = context.Highlights.ToList()
            };
            return View(model);
        }

        public ActionResult New()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(HightlightViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int count = viewModel.HighlightArts.Count();
                foreach (var image in viewModel.HighlightArts)
                {

                    if (image != null)
                    {
                        if (image.ContentLength > 0)
                        {
                            if (Path.GetExtension(image.FileName).ToLower() == ".jpg"
                                || Path.GetExtension(image.FileName).ToLower() == ".png"
                                || Path.GetExtension(image.FileName).ToLower() == ".gif"
                                || Path.GetExtension(image.FileName).ToLower() == ".jpeg")
                            {

                                var item = viewModel.ParseAsEntity(new Highlight());
                                context.Highlights.Add(item);
                                context.SaveChanges();

                                string folder = @"~/Content/HighlightGallery";
                                FileService.CreateFolder(folder);

                                string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                                string extension = Path.GetExtension(image.FileName).Substring(1);
                                folder = ConfigurationManager.AppSettings["Settings.Site.HighlightFolder"];
                                fileName = string.Format("{0}.{1}", item.HighlightId, extension);
                                string path = Path.Combine(folder, fileName);

                                FileService.DeleteFile(path);
                                image.SaveAs(path);
                                string imageFolder = "~/Content/HighlightGallery";
                                string imageFolderPath = Path.Combine(imageFolder, fileName);

                                item.FileName = fileName;
                                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                ViewBag.UploadSuccess = true;

                            }
                        }
                    }

                }

                ViewBag.Count = count;
                return RedirectToAction("Index");

            }

            return View();

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Highlight highlights = context.Highlights.Find(id);
            if (highlights == null)
            {
                return HttpNotFound();
            }
            return View(highlights);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Highlight highlights = context.Highlights.Find(id);
            context.Highlights.Remove(highlights);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}