using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.Models
{
    public class UserSearch
    {
        [Required(ErrorMessage = "Please enter a product name.")]
        public string searchString { get; set; }
    }
}
