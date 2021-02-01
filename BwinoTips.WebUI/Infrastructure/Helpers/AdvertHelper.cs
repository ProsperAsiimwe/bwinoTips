using MagicApps.Infrastructure.Services;
using MagicApps.Models;
using Microsoft.AspNet.Identity.Owin;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using BwinoTips.WebUI.Models.Adverts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    public class AdvertHelper
    {
        private ApplicationDbContext db;
        private ApplicationUserManager UserManager;

        int AdvertId;

        public Advert Advert { get; private set; }

        public string ServiceUserId { get; set; }

        public AdvertHelper()
        {
            Set();
        }

        public AdvertHelper(int AdvertId)
        {
            Set();

            this.AdvertId = AdvertId;
            this.Advert = db.Adverts.Find(AdvertId);
        }

        public AdvertHelper(Advert Advert)
        {
            Set();

            this.AdvertId = Advert.AdvertId;
            this.Advert = Advert;
        }

        private void Set()
        {
            this.db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            this.UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public async Task<UpsertModel> DeletePhoto()
        {
            var upsert = new UpsertModel();

          
            db.Adverts.Remove(Advert);
            db.Entry(Advert).State = System.Data.Entity.EntityState.Modified;
            DeletePic();

            await db.SaveChangesAsync();

            upsert.ErrorMsg = string.Format("Photo: '{0}' terminated successfully", Advert.AdvertId);
            upsert.RecordId = Advert.AdvertId.ToString();
           
            return upsert;
        }

        private bool UploadImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    string folder = @"~/Content/Adverts";
                    FileService.CreateFolder(folder);

                    string fileName = Path.GetFileNameWithoutExtension((file as HttpPostedFileBase).FileName);

                    if (Path.GetExtension((file as HttpPostedFileBase).FileName).ToLower() == ".jpg"
                        || Path.GetExtension((file as HttpPostedFileBase).FileName).ToLower() == ".png"
                        || Path.GetExtension((file as HttpPostedFileBase).FileName).ToLower() == ".gif"
                        || Path.GetExtension((file as HttpPostedFileBase).FileName).ToLower() == ".jpeg")
                    {
                        string extension = Path.GetExtension((file as HttpPostedFileBase).FileName).Substring(1);
                        folder = ConfigurationManager.AppSettings["Settings.Site.ImgFolder"];
                        fileName = string.Format("{0}.{1}", AdvertId, extension);
                        string path = Path.Combine(folder, fileName);
                        string dbPath = Path.Combine(folder, Advert.FileName);

                        FileService.DeleteFile(path);
                        FileService.DeleteFile(dbPath);

                        file.SaveAs(path);
                        string imageFolder = "~/Content/Adverts";
                        string imageFolderPath = Path.Combine(imageFolder, fileName);

                        Advert.FileName = fileName;
                        //Product.FolderPath = imageFolderPath;
                        db.Entry(Advert).State = System.Data.Entity.EntityState.Modified;

                    }
                }
                
            }
            else {
            }
        
            return true;
        }

        private bool DeletePic()
        {
           
            var Pic = string.Format(@"~/Content/Adverts/{0}", AdvertId);
            var folder = ConfigurationManager.AppSettings["Settings.Site.ImgFolder"];
            folder = string.Format(@"~/Content/Adverts", folder);

            string path = Path.Combine(folder, string.Format("{0}", Pic));

            FileService.DeleteFile(path);
          
            return true;
        }

        //public async Task<UpsertModel> UpsertAdvert(UpsertMode mode, AdViewModel model)
        //{
        //    var upsert = new UpsertModel();
        //    string title;

        //    System.Text.StringBuilder builder;

        //    // Apply changes
        //    Advert = model.ParseAsEntity(Advert);

        //    builder = new System.Text.StringBuilder();

        //    if (model.AdvertId == 0)
        //    {
        //        db.Adverts.Add(Advert);

        //        title = "Advert Created";
        //        builder.Append("An Advert record has been created for:").AppendLine();
        //    }
        //    else
        //    {
        //        db.Entry(Advert).State = System.Data.Entity.EntityState.Modified;

        //        title = "Advert Updated";
        //        builder.Append("Changes have been made to the Advert details.");

        //        if (mode == UpsertMode.Admin)
        //        {
        //            builder.Append(" (by the Admin)");
        //        }

        //        builder.Append(":").AppendLine();
        //    }

        //    await db.SaveChangesAsync();

        //    AdvertId = Advert.AdvertId;

        //    if (model.AdImages != null)
        //    {
        //        UploadImage(model.AdImages);
        //    }

        //    await db.SaveChangesAsync();

        //    if (model.productId == 0)
        //    {
        //        upsert.ErrorMsg = "Product record created successfully";
        //    }
        //    else
        //    {
        //        upsert.ErrorMsg = "Product record updated successfully";
        //    }

        //    upsert.RecordId = Product.productId.ToString();
         
        //    return upsert;
        //}
    }
}