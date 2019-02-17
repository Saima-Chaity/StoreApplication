using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApplication.Data;
using StoreApplication.Models;
using StoreApplication.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace StoreApplication.Controllers
{
    public class ProductController : Controller
    {
        public StoreDBContext db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string userID { get; set; }
        public IEnumerable<Product> productQuery { get; set; }

        public ProductController(StoreDBContext db,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            return View();
        }

        public IActionResult Details(int? id)
        {
            return View(db.Product.Where(p => p.ProductId == id).FirstOrDefault());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (User.Identity.Name == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            return View(db.Product.Where(p => p.ProductId == id).FirstOrDefault());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SaveDataToSQL(string productName, string productImage, decimal price)
        {
            var query = new ProductRepo(db).SaveProduct(productName, productImage, price);

            return View(query);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UpdateAndSaveToSQL(int productId, string productName, string productImage, decimal price)
        {

            var query = new ProductRepo(db).UpdateProduct(productId, productName, productImage, price);
            return View(query);
        }
    }
}