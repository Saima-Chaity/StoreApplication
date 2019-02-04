using System;
using System.Collections.Generic;

namespace StoreApplication.Models
{
    public partial class Product
    {
        public Product()
        {
            UserCart = new HashSet<UserCart>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public byte[] ProductImage { get; set; }
        public decimal Price { get; set; }

        public ICollection<UserCart> UserCart { get; set; }
    }
}
