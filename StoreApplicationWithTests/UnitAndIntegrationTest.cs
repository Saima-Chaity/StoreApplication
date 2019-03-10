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

        //Integration Test
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

        //Unit Test
        [Fact]
        public static void UnitTestProductList()
        {
            var mockProductRepo = new Mock<IProductRepository>();

            mockProductRepo.Setup(mpr => mpr.ProductList())
                .Returns(new List<Product>{
                    new Product(), new Product(), new Product(),
                    new Product(), new Product(), new Product()
                });

            var controller = new TestController(mockProductRepo.Object);

            var result = Assert.IsType<ViewResult>(controller.ProductCount());

            var model = Assert.IsType<List<Product>>(result.Model);

            int expected = 6;
            int actual = model.Count;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void UnitTestProductDetails()
        {
            var mockProductRepo = new Mock<IProductRepository>();

            mockProductRepo.Setup(mpr => mpr.ProductDetails(It.IsAny<int>()))
                .Returns(new Product { ProductId = 1, ProductName = "Olay Complete Moisturizer with SPF 15, 177 mL" });

            var controller = new TestController(mockProductRepo.Object);

            var result = Assert.IsType<ViewResult>(controller.ExpectedProductName(1));

            var model = Assert.IsType<Product>(result.Model);

            string expected = "Olay Complete Moisturizer with SPF 15, 177 mL";
            string actual = model.ProductName;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void UnitTestSaveProduct()
        {
            var mockProductRepo = new Mock<IProductRepository>();

            Product product = new Product();
            mockProductRepo.Setup(mpr => mpr.SaveProductToSql(It.IsAny<Product>()))
                .Returns(new Product { ProductId = 1, ProductName = "Olay Complete Moisturizer with SPF 15, 177 mL" });

            var controller = new TestController(mockProductRepo.Object);

            var result = Assert.IsType<ViewResult>(controller.SaveDataToSQL(product));

            var model = Assert.IsType<Product>(result.Model);

            string expectedName = "Olay Complete Moisturizer with SPF 15, 177 mL";
            string actualName = model.ProductName;
            Assert.Equal(expectedName, actualName);
        }

        [Fact]
        public static void UnitTestSearchProduct()
        {
            var mockProductRepo = new Mock<IProductRepository>();

            mockProductRepo.Setup(mpr => mpr.SearchString(It.IsAny<string>()))
                .Returns(new Product { ProductName = "Aveeno" });

            var controller = new TestController(mockProductRepo.Object);

            var result = Assert.IsType<ViewResult>(controller.SearchProduct("Aveeno"));

            var model = Assert.IsType<Product>(result.Model);

            string expectedName = "Aveeno";
            string actualName = model.ProductName;
            Assert.Equal(expectedName, actualName);
        }

    }
}
