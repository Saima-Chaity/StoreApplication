using StoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.Interfaces
{
    public interface IProductRepository
    {
        List<Product> ProductList();
        Product ProductDetails(int id);
        Product SaveProductToSql(Product product);
        Product SearchString(string userInput);
    }

}
