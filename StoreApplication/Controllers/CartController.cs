using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApplication.Data;
using StoreApplication.Models;
using StoreApplication.Repositories;
using StoreApplication.Services;

namespace StoreApplication.Controllers
{
    public class CartController : Controller
    {

        public StoreDBContext db;
        public ApplicationDbContext userdb;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string userID { get; set; }
        public IEnumerable<Product> productQuery { get; set; }

        public CartController(StoreDBContext db,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public string SignInUser()
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                             Response);

            userID = _userManager.GetUserId(User);
            cookieHelper.Set("UserID", userID, 1);

            userID = cookieHelper.Get("UserID");

            return userID;
        }

        public IActionResult AddToCart(int id)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                             Response);

            if (!_signInManager.IsSignedIn(User))
            {
                cookieHelper.Remove("UserID");
                return Redirect("/Identity/Account/Login");
            }

            SignInUser();

            var query = new CartRepo(db).SaveProductToCart(id, userID);
            return View(query);
        }

        public IActionResult ViewCart()
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                 Response);

            if (!_signInManager.IsSignedIn(User))
            {
                cookieHelper.Remove("UserID");
                return Redirect("/Identity/Account/Login");
            }

            SignInUser();

            var query = new CartRepo(db).GetAllProductsFromCart(userID);
            return View("AddToCart", query);
        }

        public IActionResult Details(int? id)
        {
            return View(db.Product.Where(p => p.ProductId == id).FirstOrDefault());
        }

        public IActionResult Delete(int? id)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                             Response);

            userID = cookieHelper.Get("UserID");

            var query = (from p in db.UserCart
                         where p.ProductId == id && p.Id == userID
                         select p).FirstOrDefault();

            if (query != null)
            {
                db.UserCart.Remove(query);
                db.SaveChanges();
            }

            return View(query);



        }
    }
}