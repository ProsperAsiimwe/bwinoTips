using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Models;
using BwinoTips.WebUI.Infrastructure.Helpers;
using BwinoTips.WebUI.Models;
using BwinoTips.WebUI.Models.Carts;
using BwinoTips.WebUI.Models.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Controllers
{
    [RoutePrefix("Cart")]
    public class CartController : BaseController
    {
        public CartController()
        {
        }

        //[Compress]
        public ViewResult Index(string returnUrl)
        {

            var cart = GetCart();
        
            var model = new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl };

            return View(model);
        }

        //[Compress]
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(GetCart());
        }

        //[Compress]
        public ActionResult Checkout()
        {

            var cart = GetCart();

            if (cart.Lines.Count() <= 0)
            {
                ShowError("Sorry, ticket is empty");
                return RedirectToAction("Index", new { returnUrl = Request.UrlReferrer });
            }

            var model = new TicketViewModel()
            {
             
                TotalAmount = cart.computeTotalValue(),
              
            };

            return View(model);
        }

        //[Compress]
        [HttpPost]
        public async Task<ActionResult> Checkout(TicketViewModel model)
        {
            if (GetCart().Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your ticket is empty!");
            }
            if (ModelState.IsValid)
            {
                // save ticket and create ticket items 

                var helper = new TicketHelper();
                var result = await helper.UpsertTicket(Infrastructure.UpsertMode.Admin, model, GetCart());
                if (result.i_RecordId() > 0)
                {
                    ShowSuccess(result.ErrorMsg);
                    GetCart().Clear();
                }
                else
                {
                    ShowError(result.ErrorMsg);
                }

                return RedirectToAction("Index", "Ticket");
            }
            else
            {
                return View(model);
            }

        }

        //[Compress]
        [Route("Add/{ExclusiveTipId:int}/{quantity:int}")]
        public ActionResult Add(int ExclusiveTipId, int quantity)
        {
            ExclusiveTip tip = context.ExclusiveTips.FirstOrDefault(p => p.ExclusiveTipId == ExclusiveTipId);

            if (tip == null)
            {
                return Content("Problem selecting game.");
            }

            GetCart().AddItem(tip, quantity > 0 ? quantity : 1);

            return PartialView("Summary", GetCart());
        }

        [Route("AddItem/{ExclusiveTipId:int}/{qty:int}")]
        public ActionResult AddItem(int ExclusiveTipId, int qty)
        {
            ExclusiveTip tip = context.ExclusiveTips.FirstOrDefault(p => p.ExclusiveTipId == ExclusiveTipId);

            if (tip == null)
            {
                return Content("Problem selecting game.");
            }

            var cart = GetCart();
            cart.AddItem(tip, qty > 0 ? qty : 1);

            var item = cart.GetItem(ExclusiveTipId);

            return Json(new { total = string.Format("{0} Odds:", cart.computeTotalValue().ToString()), view = RenderRazorViewToString(ControllerContext, "Partials/_Item", item) });

        }

        //[Compress]
        public RedirectToRouteResult AddToCart(int ExclusiveTipId, int quantity, string returnUrl)
        {
            ExclusiveTip tip = context.ExclusiveTips.FirstOrDefault(p => p.ExclusiveTipId == ExclusiveTipId);
            //var CurrQty = tip.CurrentQuantity;

            //if (!GetCart().Validate(ExclusiveTipId, tip.CurrentQuantity, quantity, tip.IsService))
            //{
            //    ShowError("Quantity selected is below your Branch's stock level.");
            //    return RedirectToAction("Index", "tips");
            //}

            if (tip != null)
            {
                GetCart().AddItem(tip, quantity > 0 ? quantity : 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        //[Compress]
        public RedirectToRouteResult RemoveFromCart(int ExclusiveTipId, string returnUrl)
        {
            ExclusiveTip tip = context.ExclusiveTips.FirstOrDefault(p => p.ExclusiveTipId == ExclusiveTipId);

            if (tip != null)
            {
                GetCart().RemoveItem(tip);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];

            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }

        
            return cart;
        }

        //[Compress]/*pdt.CurrentQuantity < quantity*/
        [Route("{ExclusiveTipId:int}/{quantity:int}/Qty")]
        public ActionResult UpdateCost(int ExclusiveTipId, int quantity)
        {
            var pdt = context.ExclusiveTips.FirstOrDefault(p => p.ExclusiveTipId == ExclusiveTipId);

            //if (!GetCart().Validate(ExclusiveTipId, pdt.CurrentQuantity, quantity, pdt.IsService))
            //{
            //    return Content("Quantity selected for tip exceeds that available.");
            //}

            var tip = GetCart().Lines.FirstOrDefault(p => p.ExclusiveTip.ExclusiveTipId == ExclusiveTipId);

            if (tip != null)
            {
                tip.Quantity = quantity;              
                return Json(new { cost = string.Format("{0} Odd", tip.Odd.ToString()), total = string.Format("{0} Odds", GetCart().computeTotalValue().ToString()) });
            }

            return null;
        }

        //[Compress] 0772121219 eddy
        [Route("{ExclusiveTipId:int}/{discount}/{percent}/Disc")]
        public JsonResult UpdateDisc(int ExclusiveTipId, double discount, int percent)
        {
            var tip = GetCart().Lines.FirstOrDefault(p => p.ExclusiveTip.ExclusiveTipId == ExclusiveTipId);

            if (tip != null)
            {
               
                return Json(new { cost = string.Format("{0} Odd", tip.Odd.ToString()), total = string.Format("{0} Odds", GetCart().computeTotalValue().ToString()) });
            }

            return null;
        }
     

    }
}

