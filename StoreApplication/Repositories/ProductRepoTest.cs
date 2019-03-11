using StoreApplication.Interfaces;
using StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.Repositories
{
    public class ProductRepoTest: IProductRepository
    {
        public StoreDBContext db;

        public ProductRepoTest(StoreDBContext db)
        {
            this.db = db;
        }

        public List<Product> ProductList()
        {
            var query = from p in db.Product
                        select p;
            return query.ToList();
        }

        public Product ProductDetails(int id)
        {
            return (db.Product.Where(p => p.ProductId == id).FirstOrDefault());
        }
        public Product SaveProductToSql(Product product)
        {
            Product newProduct = new Product();

            newProduct.ProductName = product.ProductName;

            newProduct.Price = product.Price;

            db.Product.Add(newProduct);
            db.SaveChanges();

            return newProduct;
        }

        public Product SearchString(string userInput)
        {
            var query = (from p in db.Product
                         where (p.ProductName.Contains(userInput))
                         select p).FirstOrDefault();

            return query;
        }
    }
}
