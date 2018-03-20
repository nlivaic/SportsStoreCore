using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests {
    public class ProductControllerTests {
        [Fact]
        public void Can_Send_Pagination_View_Model() {
                // Arrange
                Mock<IProductRepository> mock = new Mock<IProductRepository>();
                mock.SetupGet(m => m.Products).Returns(new List<Product> {
                    new Product { ProductId = 1, Name = "P1" },
                    new Product { ProductId = 2, Name = "P2" },
                    new Product { ProductId = 3, Name = "P3" },
                    new Product { ProductId = 4, Name = "P4" },
                    new Product { ProductId = 5, Name = "P5" },
                    new Product { ProductId = 6, Name = "P6" },
                    new Product { ProductId = 7, Name = "P7" },
                    new Product { ProductId = 8, Name = "P8" }
                    }.AsQueryable<Product>());
                ProductController target = new ProductController(mock.Object);
                target.PageSize = 3;

                // Act
                ProductsListViewModel result = (target.List(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel;
                Product[] products = result.Products.ToArray();
            
                // Assert
                Assert.NotNull(result);
                Assert.Equal(8, result.PagingInfo.TotalItems);
                Assert.Equal(3, result.PagingInfo.ItemsPerPage);
                Assert.Equal(2, result.PagingInfo.CurrentPage);
                Assert.Equal(3, result.PagingInfo.TotalPages);

                Assert.Equal(4, products[0].ProductId);
                Assert.Equal(5, products[1].ProductId);
                Assert.Equal(6, products[2].ProductId);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model_With_Category() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(new List<Product> {
                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductId = 3, Name = "P3", Category = "Cat3" },
                new Product { ProductId = 4, Name = "P4", Category = "Cat1" },
                new Product { ProductId = 5, Name = "P5", Category = "Cat1" },
                new Product { ProductId = 6, Name = "P6", Category = "Cat3" },
                new Product { ProductId = 7, Name = "P7", Category = "Cat1" },
                new Product { ProductId = 8, Name = "P8", Category = "Cat1" }
            }.AsQueryable<Product>());
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            // Act
            ProductsListViewModel result = (target.List("Cat1", 2) as ViewResult).ViewData.Model as ProductsListViewModel;
            Product[] products = result.Products.ToArray();
        
            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.PagingInfo.TotalItems);
            Assert.Equal(3, result.PagingInfo.ItemsPerPage);
            Assert.Equal(2, result.PagingInfo.CurrentPage);
            Assert.Equal(2, result.PagingInfo.TotalPages);

            Assert.Equal(7, products[0].ProductId);
            Assert.Equal(8, products[1].ProductId);
        }

        [Fact]
        public void Can_Paginate() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(new List<Product> {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" }
                }.AsQueryable<Product>());
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            // Act - Get second page.
            Product[] result = (((target.List(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel).Products as IEnumerable<Product>).ToArray();
            
            // Assert
            Assert.True(result.Count() == 2);
            Assert.Equal("P4", result[0].Name);
            Assert.Equal("P5", result[1].Name);
        }

        [Fact]
        public void Can_Filter_By_Category() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(new List<Product> {
                new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductId = 3, Name = "P3", Category = "Cat3" },
                new Product { ProductId = 4, Name = "P4", Category = "Cat1" },
                new Product { ProductId = 5, Name = "P5", Category = "Cat1" },
                new Product { ProductId = 6, Name = "P6", Category = "Cat3" },
                new Product { ProductId = 7, Name = "P7", Category = "Cat1" }
                }.AsQueryable<Product>());
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            string category = "Cat1";
            
            // Act
            Product[] result = ((target.List(category, 2) as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();
                        
            // Assert
            Assert.True(result.Count() == 1);
            Assert.Equal(7, result[0].ProductId);
        }
    }
}