using System;
using System.Collections.Generic;

namespace StoreApplication.Models
{
    public partial class UserCart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
