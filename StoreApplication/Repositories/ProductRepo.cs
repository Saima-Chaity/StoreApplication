﻿using Microsoft.AspNetCore.Http;
using StoreApplication.Data;
using StoreApplication.Interfaces;
using StoreApplication.Models;
using StoreApplication.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Text;

namespace StoreApplication.Repositories
{
    public class ProductRepo
    {
        public StoreDBContext db;

        public ProductRepo(StoreDBContext db)
        {
            this.db = db;
        }

        public IQueryable<Product> GetAll(string sortOrder, UserSearch userSearch)
        {

            if (!String.IsNullOrEmpty(userSearch.searchString))
            {

                var query = from p in db.Product
                            where (p.ProductName.Contains(userSearch.searchString))
                            select p;

                return query;

            }
            else
            {
                var query = from p in db.Product
                            select p;

                switch (sortOrder)
                {
                    case "title_asc":
                        query =
                            query.OrderBy(p => p.ProductName);
                        break;
                    case "title_desc":
                        query =
                            query.OrderByDescending(p => p.ProductName);
                        break;
                    case "price_desc":
                        query =
                      query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        query =
                            query.OrderBy(p => p.Price);
                        break;
                };
                return query;
            }
        }


        public Product SaveProduct(string productName, string productImage, decimal price)
        {

            string imagePath = @"C:\" + productImage;

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);

            Product newProduct = new Product();

            newProduct.ProductName = productName;
            newProduct.ProductImage = Convert.FromBase64String(base64String);
            newProduct.Price = price;

            db.Product.Add(newProduct);
            db.SaveChanges();

            return newProduct;
        }

        public Product UpdateProduct(int productId, string productName, string productImage, decimal price)
        {

            Product product = (from p in db.Product
                               where p.ProductId == productId
                               select p).FirstOrDefault();


            if (product != null)
            {
                string imagePath = @"C:\" + productImage;

                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);

                product.ProductName = productName;
                product.ProductImage = Convert.FromBase64String(base64String);
                product.Price = price;

                db.Product.Update(product);
                db.SaveChanges();
            }

            return product;
        }
    }
}
