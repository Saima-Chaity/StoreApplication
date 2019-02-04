using Microsoft.AspNetCore.Http;
using StoreApplication.Data;
using StoreApplication.Models;
using StoreApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.Repositories
{
    public class ProductRepo
    {
        public StoreDBContext db;

        public ProductRepo(StoreDBContext db)
        {
            this.db = db;
        }

        public IQueryable<Product> GetAll(string sortOrder, string searchString)
        {

            if (!String.IsNullOrEmpty(searchString))
            {

                var query = from p in db.Product
                            where (p.ProductName.StartsWith(searchString))
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


    }
}
