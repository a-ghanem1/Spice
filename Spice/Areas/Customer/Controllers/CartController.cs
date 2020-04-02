﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Utility;
using Spice.ViewModels;

namespace Spice.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderDetailsCart DetailCart { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            DetailCart = new OrderDetailsCart() 
            { 
                OrderHeader= new Models.OrderHeader()
            };

            DetailCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                DetailCart.CartList = cart.ToList();
            }

            foreach (var list in DetailCart.CartList)
            {
                list.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);
                DetailCart.OrderHeader.OrderTotal = DetailCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);
                list.MenuItem.Description = SD.ConvertToRawHtml(list.MenuItem.Description);
                if (list.MenuItem.Description.Length > 100)
                {
                    list.MenuItem.Description = list.MenuItem.Description.Substring(0, 99) + "...";
                }
            }

            DetailCart.OrderHeader.OrderTotalOriginal = DetailCart.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                DetailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == DetailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                DetailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, DetailCart.OrderHeader.OrderTotalOriginal);
            }

            return View(DetailCart);
        }

        public IActionResult AddCoupon()
        {
            if (DetailCart.OrderHeader.CouponCode == null)
            {
                DetailCart.OrderHeader.CouponCode = "";
            }
            HttpContext.Session.SetString(SD.ssCouponCode, DetailCart.OrderHeader.CouponCode);

            return RedirectToAction(nameof(Index));
        }
    }
}