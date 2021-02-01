using MagicApps.Infrastructure.Services;
using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Infrastructure.Helpers;
using BwinoTips.WebUI.Models.Adverts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    public class AdvertController : BaseController
    {
        // GET: Advert
        public ActionResult Index()
        {
            ViewBag.Active = "Adverts";

            var model = new AdListViewModel
            {
                Adverts = context.Adverts.ToList()
            };
            return View(model);

        }

        public ActionResult New()
        {

            return View();
        }

        [Authorize(Roles = "Developer, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int count = viewModel.AdImages.Count();
                foreach (var image in viewModel.AdImages)
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

                                var item = viewModel.ParseAsEntity(new Advert());
                                context.Adverts.Add(item);
                                context.SaveChanges();

                                string folder = @"~/Content/Adverts";
                                FileService.CreateFolder(folder);

                                string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                                string extension = Path.GetExtension(image.FileName).Substring(1);
                                folder = ConfigurationManager.AppSettings["Settings.Site.ImgFolder"];
                                fileName = string.Format("{0}.{1}", item.AdvertId, extension);
                                string path = Path.Combine(folder, fileName);

                                FileService.DeleteFile(path);
                                image.SaveAs(path);
                                string imageFolder = "~/Content/Adverts";
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
            Advert adverts = context.Adverts.Find(id);
            if (adverts == null)
            {
                return HttpNotFound();
            }
            return View(adverts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var Pic = string.Format(@"~/Content/Adverts/{0}", id);
            var folder = ConfigurationManager.AppSettings["Settings.Site.ImgFolder"];

            Advert adverts = context.Adverts.Find(id);
            context.Adverts.Remove(adverts);

            //Delete file from directory
            folder = string.Format(@"~/Content/Adverts", folder);
            string path = Path.Combine(folder, string.Format("{0}", Pic));
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            //FileService.DeleteFile(path);


            context.SaveChanges();

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    if (!IsRoutingOK(id))
        //    {
        //        return RedirectOnError();
        //    }

        //    var helper = GetHelper(id);
        //    var upsert = await helper.DeletePhoto();

        //    if (upsert.i_RecordId() > 0)
        //    {
        //        ShowSuccess(upsert.ErrorMsg);
        //        return RedirectOnError();
        //    }

        //    // If we got this far, an error occurred
        //    ShowError(upsert.ErrorMsg);

        //    //return RedirectToAction("Index");
        //    return RedirectToAction("Delete", new { id = id });


        //}


        private AdvertHelper GetHelper(int id)
        {
            AdvertHelper helper = new AdvertHelper(id);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private AdvertHelper GetHelper(Advert Advert)
        {
            var helper = new AdvertHelper(Advert);

            helper.ServiceUserId = GetUserId();

            return helper;
        }

        private bool IsRoutingOK(int? id)
        {

            // Check Player
            if (id.HasValue)
            {
                var Advert = context.Adverts.ToList().SingleOrDefault(x => x.AdvertId == id);

                if (Advert == null)
                {
                    return false;
                }
            }

            return true;
        }

        private RedirectToRouteResult RedirectOnError()
        {
            return RedirectToAction("Index");
        }


    }
}