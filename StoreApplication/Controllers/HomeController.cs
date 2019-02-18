using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApplication.Data;
using StoreApplication.Models;
using StoreApplication.Repositories;
using StoreApplication.Services;

namespace StoreApplication.Controllers
{
    public class HomeController : Controller
    {
        public StoreDBContext db;
        public ApplicationDbContext userdb;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(StoreDBContext db,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index(string sortOrder, UserSearch userSearch, int? page)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                                         Response);
            if (_signInManager.IsSignedIn(User))
            {
                string userID = _userManager.GetUserId(User);
                cookieHelper.Set("UserID", userID, 1);
                HttpContext.Session.SetString("UserID", userID);
                ViewBag.userId = HttpContext.Session.GetString("UserID");
            }
            if (!_signInManager.IsSignedIn(User))
            {
                ViewBag.userId = "";
            }
            if (User.Identity.Name != null)
            {
                var userName = User.Identity.Name;
                if (HttpContext.Session.GetString(userName) == null)
                {
                    string name = userName;
                    ViewBag.name = name;
                    HttpContext.Session.SetString("username", name);

                }
                else
                {
                    ViewBag.name = HttpContext.Session.GetString(userName);
                }
            }

            string sort = String.IsNullOrEmpty(sortOrder) ? "title_asc" : sortOrder;
            string search = String.IsNullOrEmpty(userSearch.searchString) ? "" : userSearch.searchString;

            ViewData["CurrentSort"] = sort;
            ViewData["CurrentFilter"] = search;
            int pageSize = 3;

            var query = new ProductRepo(db).GetAll(sort, userSearch);
            return View(PaginatedList<Product>.Create(query, page ?? 1, pageSize));
        }

        public IActionResult ClearCookie(string key)
        {
            CookieHelper cookieHelper = new CookieHelper(_httpContextAccessor, Request,
                                                         Response);
            cookieHelper.Remove(key);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
