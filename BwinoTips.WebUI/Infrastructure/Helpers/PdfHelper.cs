using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MagicApps.Infrastructure.Helpers;
using MagicApps.Infrastructure.Services;
using MagicApps.Models;
using Microsoft.AspNet.Identity.Owin;
using BwinoTips.Domain.Context;
using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Models;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    
    public class PdfHelper
    {
        private ApplicationDbContext db;

        public string ServiceUserId;

        public PdfHelper(string serviceUserId)
        {
            this.db = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            this.ServiceUserId = serviceUserId;
        }

        public UpsertModel CreateReference(Reference reference)
        {
            return null;
        }

        //public UpsertModel CreateSalesReport(IEnumerable<Sale> Sales)
        //{
        //    var upsert = new UpsertModel();

        //    try {
        //        string docName = String.Format(@"BwinoTips_Sales_{0}.pdf", UgandaDateTime.DateNow().Date.ToString("ddMMMyyyy"));
        //        docName = FileService.RemoveIllegalCharacters(docName, true);

        //        string destinationFolder = @"~/App_Data";

        //        //string logo = HttpContext.Current.Server.MapPath("~/Content/Imgs/BwinoTips_logo.png");

        //        // Create a temp folder if not there
        //        string temp_folder = FileService.CreateFolder(string.Format(@"{0}\Temp", destinationFolder));

        //        destinationFolder = Settings.DOCFOLDER;

        //        // Work on the Temp File
        //        string abs_TempDoc = String.Format(@"{0}\{1}", temp_folder, docName);

        //        // Save to New File
        //        string abs_NewDoc = String.Format(@"{0}\{1}", destinationFolder, docName);

        //        // Delete the old temp file
        //        FileService.DeleteFile(abs_TempDoc);

        //        // Create a document
        //        var doc = new Document(PageSize.A4);

        //        // Create the document object
        //        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(abs_TempDoc, FileMode.Create));

        //        // Events
        //        ITextEvents header = new ITextEvents();
        //        writer.PageEvent = header;

        //        // Open for editing/creation
        //        doc.Open();

        //        // Lets go!
        //        // ++++++++++++++++++++++++++++++++++++++++++++
        //        // +++++++++++++++++ START ++++++++++++++++++++
        //        PdfPTable table;
        //        //PdfPCell cell;
        //        //Paragraph paragraph;
        //        //Chunk chunk;

        //        ////BwinoTips Logo
        //        //doc.Add(ImageCell(logo));
        //        var sales = Sales.OrderBy(x => x.Date);
        //        var startDate = sales.First().Date.ToString("ddd, dd MMM yyyy");
        //        var endDate = sales.Last().Date.ToString("ddd, dd MMM yyyy");

        //        doc.Add(ParsePageHeading("BwinoTips Periodical Sales Report"));

        //        // 1. Applicant Details
        //        doc.Add(ParseHeading(string.Format("From {0} to {1}", startDate, endDate)));
        //        var cellWidths = new List<float> { 5f, 30f, 12f, 25f, 18f, 10f };
                
        //        table = SetTable(cellWidths.ToArray(), true, true);

        //        table.AddCell(LabelCell("#"));
        //        table.AddCell(LabelCell("Product Item"));
        //        table.AddCell(LabelCell("Quantity"));
        //        table.AddCell(LabelCell("Sales Revenue (Ugx)"));
        //        table.AddCell(LabelCell("Profit (Ugx)"));
        //        table.AddCell(LabelCell("Profit %"));

        //        var saleItems = Sales.SelectMany(p => p.Items);
        //        var products = saleItems.Select(k => k.Product).Distinct();
        //        double profitTotal = 0;
        //        double revenueTotal = 0;
        //        int count = 0;

        //        foreach(var product in products)
        //        {
        //            var items = saleItems.Where(p => p.ProductId == product.ProductId);
        //            var profit = items.Sum(k => k.Profit); profitTotal += profit;
        //            var revenue = items.Sum(k => k.Cost); revenueTotal += revenue;
        //            var percentage = Math.Round((profit / revenue) * 100);
        //            count++;

        //            table.AddCell(LabelCell(count.ToString()));
        //            table.AddCell(AnswerCell(product.Name));
        //            table.AddCell(AnswerCell(items.Sum(k => k.Quantity).ToString()));
        //            table.AddCell(AnswerCell(revenue.ToString("n0")));
        //            table.AddCell(AnswerCell(profit.ToString("n0")));
        //            table.AddCell(AnswerCell(percentage.ToString()));
        //        }

        //        var cell = LabelCell("Totals");
        //        cell.Colspan = 2;
        //        table.AddCell(cell);
        //        table.AddCell(LabelCell(revenueTotal.ToString("n0")));
        //        table.AddCell(LabelCell(profitTotal.ToString("n0")));
        //        table.AddCell(LabelCell(""));
                
        //        doc.Add(table);

        //        doc.Add(BoldText(string.Format("Cash Payments: {0}", Sales.Sum(p => p.AmountPaid).ToString("n0"))));
        //        doc.Add(ParseParagraph(""));
        //        doc.Add(BoldText(string.Format("Credit: {0}", Sales.Sum(p => p.Balance).ToString("n0"))));
        //        // +++++++++++++++++ FINISH +++++++++++++++++++
        //        // ++++++++++++++++++++++++++++++++++++++++++++

        //        // Close and Save the document
        //        doc.Close();
        //        doc.Dispose();
        //        writer.Dispose();

        //        // Delete the saved file
        //        FileService.DeleteFile(abs_NewDoc);

        //        // Save the temp file to the save file
        //        File.Copy(abs_TempDoc, abs_NewDoc);

        //        upsert.RecordId = Sales.Count().ToString();
        //        upsert.ErrorMsg = docName;
        //    }
        //    catch (Exception ex) {
             
        //        upsert.ErrorMsg = ex.Message;
        //    }                

        //    return upsert;
        //}

        private Paragraph ParseParagraph(string text)
        {
            Paragraph paragraph = new Paragraph(text);
            paragraph.SpacingAfter = 15;
            return paragraph;
        }

        private Paragraph ParseParagraph(Chunk chunk)
        {
            Paragraph paragraph = new Paragraph(chunk);
            paragraph.SpacingAfter = 15;
            return paragraph;
        }

        private Paragraph ParsePageHeading(string text)
        {
            //Font font = FontFactory.GetFont("Arial", Font.BOLD, Font.UNDERLINE);
            //font.Size = 24;
            Font font = FontFactory.GetFont(BaseFont.HELVETICA, 24, Font.BOLD);
            Paragraph paragraph = new Paragraph(text, font);
            paragraph.SpacingAfter = 25;
            paragraph.Alignment = Element.ALIGN_CENTER;
            return paragraph;
        }

        private Paragraph ParseHeading(string text)
        {
            Font font = FontFactory.GetFont(BaseFont.HELVETICA, 18, Font.BOLD);
            Paragraph paragraph = new Paragraph(text, font);
            paragraph.SpacingAfter = 15;
            return paragraph;
        }

        private Chunk BoldText(string text)
        {
            Font font = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD);
            return new Chunk(text, font);
        }

        private PdfPTable SetTable(int columns)
        {
            PdfPTable table = new PdfPTable(columns);

            table.WidthPercentage = 100;

            return table;
        }

        private PdfPTable SetTable(float[] columnWidths, bool percentages, bool border)
        {
            PdfPTable table = new PdfPTable(columnWidths);

            if (border)
            {
                SetTableParams(table);
            }
            else
            {
                table.DefaultCell.BorderColor = BaseColor.WHITE;
                table.DefaultCell.BorderWidth = 0;
            }

            if (percentages)
            {
                table.WidthPercentage = 100;
                //table.SetWidthPercentage(columnWidths, PageSize.A4);
            }
            else
            {
                table.TotalWidth = columnWidths.Sum();
                table.SetWidths(columnWidths);
            }

            return table;
        }

        private void SetTableParams(PdfPTable table)
        {
            table.DefaultCell.Padding = 5;
            table.DefaultCell.BorderColor = new BaseColor(204, 204, 204);
            table.DefaultCell.BorderWidth = 1;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.SpacingAfter = 20;

        }

        private PdfPCell StandardCell(string text)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));

            cell.BorderColor = new BaseColor(204, 204, 204);
            cell.BorderWidth = 1;
            cell.Padding = 5;

            return cell;
        }

        private PdfPCell AnswerCell(string text)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));

            cell.BorderColor = new BaseColor(204, 204, 204);
            cell.BorderWidth = 1;
            cell.Padding = 5;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;

            return cell;
        }

        private PdfPCell LabelCell(string text)
        {
            PdfPCell cell = StandardCell(text);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(241, 241, 241);

            return cell;
        }

        private Image ImageCell(string path)
        {
            Image img = Image.GetInstance(path);

            //resize image 
            img.ScaleToFit(140f, 70f);

            //space around image
            img.SpacingAfter = 5;
            img.SpacingBefore = 10;

            //align image
            img.Alignment = Element.ALIGN_CENTER;

            return img;
        }

        //public Activity CreateActivity(string title, string description)
        //{
        //    var activity = new Activity {
        //        Title = title,
        //        Description = description,
        //        RecordedById = ServiceUserId
        //    };
        //    db.Activities.Add(activity);
        //    return activity;
        //}
    }

    public class ITextEvents : PdfPageEventHelper
    {
        public ITextEvents()
        {
        }

        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // we will put the final number of pages in a template
        PdfTemplate template;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont baseFont = null;
        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            string error;

            try {
                PrintTime = DateTime.Now;
                baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de) {
                error = de.Message;
            }
            catch (System.IO.IOException ioe) {
                error = ioe.Message;
            }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;

            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);
            cb.SetTextMatrix(pageSize.GetLeft(35), pageSize.GetTop(20));
            cb.ShowText("BwinoTips Sales");
            cb.SetRGBColorFill(100, 100, 100);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
                "BwinoTips Tyres Limited",
                pageSize.GetRight(35),
                pageSize.GetTop(20), 0);
            cb.EndText();

            // Otherwise, changes text of main body
            cb.SetRGBColorFill(0, 0, 0);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            String text = "Page " + pageN;
            float len = baseFont.GetWidthPoint(text, 8);

            Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(100, 100, 100);
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);
            cb.SetTextMatrix(pageSize.GetLeft(35), pageSize.GetBottom(20));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetLeft(35) + len, pageSize.GetBottom(20));

            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
                "Last updated: " + PrintTime.ToString("dd/MM/yyyy HH:mm"),
                pageSize.GetRight(35),
                pageSize.GetBottom(20), 0);
            cb.EndText();

            //address on footer
            string address = String.Join(", ", Settings.COMPANY_ADDRESS);
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
                address,
                pageSize.GetLeft(250),
                pageSize.GetBottom(20), 0);
            cb.EndText();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            //template.BeginText();
            // template.SetFontAndSize(bf, 8);
            // template.SetTextMatrix(0, 0);
            // template.ShowText("" + (writer.PageNumber - 1));
            // template.EndText();
        }
    }
}