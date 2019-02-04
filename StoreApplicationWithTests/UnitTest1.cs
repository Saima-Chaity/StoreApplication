using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using StoreApplication.Controllers;
using StoreApplication.Interfaces;
using StoreApplication.Models;
using StoreApplication.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PassionProjectWithTest
{
    public static class DbOptionsFactory
    {
        static DbOptionsFactory()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Path.GetFullPath("../../../")))
                .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = config["ConnectionStrings:DefaultConnection"];

            DbContextOptions = new DbContextOptionsBuilder<StoreDBContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public static DbContextOptions<StoreDBContext> DbContextOptions { get; }


        [Fact]
        public static void IndexViewHas5Products()
        {
            using (var context = new StoreDBContext(DbOptionsFactory.DbContextOptions))
            {
                // Instantiate Class with DbContext parameter
                var controller = new TestController(context);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
                int expected = 10;
                Assert.Equal(expected, model.Count());
            }
        }

        [Fact]
        public static void ProductDetails()
        {
            using (var context = new StoreDBContext(DbOptionsFactory.DbContextOptions))
            {
                var controller = new TestController(context);

                var result = controller.Details(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Product>(viewResult.Model);
                string expected = "Olay Complete Moisturizer with SPF 15, 177 mL";
                Assert.Equal(expected, model.ProductName);
            }
        }

    }
}
