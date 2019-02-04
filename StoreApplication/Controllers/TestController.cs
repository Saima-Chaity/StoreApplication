using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreApplication.Interfaces;
using StoreApplication.Models;
using StoreApplication.Repositories;

namespace StoreApplication.Controllers
{
    public class TestController : Controller
    {
        public StoreDBContext db;
        public IProductRepository productRepository;
        //Integration test
        public TestController(StoreDBContext db)
        {
            this.db = db;
        }

        public TestController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var query = new ProductRepo(db).ProductList();

            return View(db.Product);
        }

        public IActionResult Details(int id)
        {
            return View(db.Product.Where(p => p.ProductId == id).FirstOrDefault());
        }

        //Unit Test
        public IActionResult ProductCount()
        {
            var products = productRepository.ProductList();
            return View(products);
        }

        public IActionResult ExpectedProductName(int id)
        {
            var products = productRepository.ProductDetails(id);
            return View(products);
        }

        public IActionResult SaveDataToSQL(Product product)
        {
            var query = productRepository.SaveProductToSql(product);

            return View(query);
        }

        public IActionResult SearchProduct(string userInput)
        {
            var query = productRepository.SearchString(userInput);

            return View(query);
        }
    }
}