using StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.Repositories
{
    public class CartRepo
    {
        public StoreDBContext db;
        public string userID { get; set; }
        public IEnumerable<Product> productQuery { get; set; }

        public CartRepo(StoreDBContext db)
        {
            this.db = db;
        }

        public IEnumerable<Product> SaveProductToCart(int id, string userID)
        {

            UserCart userCart = new UserCart();

            userCart.ProductId = id;
            userCart.Id = userID;
            userCart.Quantity += 1;

            db.UserCart.Add(userCart);
            db.SaveChanges();

            GetAllProductsFromCart(userID);
            return productQuery;
        }

        public IEnumerable<Product> GetAllProductsFromCart(string userID)
        {

            var userQuery = from p in db.UserCart
                            where p.Id == userID
                            select p;

            productQuery = from p in db.Product
                           from q in userQuery
                           where p.ProductId == q.ProductId
                           select p;

            return productQuery;
        }
    }
}
