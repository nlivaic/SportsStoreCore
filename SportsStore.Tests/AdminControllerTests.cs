using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests {
    public class AdminControllerTests {
        [Fact]
        public void Index_Contains_All_Products() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(new List<Product> {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" }
                }.AsQueryable<Product>());
            AdminController target = new AdminController(mock.Object);

            // Act
            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();
            
            // Assert
            Assert.True(result.Length == 3);
            Assert.True(result[0].ProductId == 1);
            Assert.True(result[1].ProductId == 2);
            Assert.True(result[2].ProductId == 3);
        }

        [Fact]
        public void Can_Edit_Product() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(new List<Product> {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" }
                }.AsQueryable<Product>());
            AdminController target = new AdminController(mock.Object);
            
            // Act
            Product result = GetViewModel<Product>(target.Edit(1));
            
            // Assert
            Assert.True(result.ProductId == 1);
            Assert.True(result.Name == "P1");
        }
        
        [Fact]
        public void Cannot_Edit_Nonexistent_Product() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(new List<Product> {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" }
                }.AsQueryable<Product>());
            AdminController target = new AdminController(mock.Object);
            
            // Act
            ViewResult viewResult = target.Edit(4) as ViewResult;
            string viewResultName = viewResult.ViewName;
            Product product = GetViewModel<Product>(viewResult);
            
            // Assert
            Assert.Null(product);
            Assert.True(viewResultName == "Index");
        }

        [Fact]
        public void Can_Save_Valid_Changes() {
            // Arrange - repository
            Mock<IProductRepository> mockRepo = new Mock<IProductRepository>();
            AdminController target = new AdminController(mockRepo.Object);
            // Arrange - TempData
            Mock<ITempDataDictionary> mockTempData = new Mock<ITempDataDictionary>();
            target.TempData = mockTempData.Object;
            // Arrange - product
            Product product = new Product { Name = "ProductName" };

            // Act
            IActionResult result = target.Edit(product) as IActionResult;
            
            // Assert - product saved.
            mockRepo.Verify(m => m.SaveProduct(product), Times.Once);
            // Assert - redirects.
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True((result as RedirectToActionResult).ActionName == nameof(AdminController.Index));
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes() {
            // Arrange - repository
            Mock<IProductRepository> mockRepo = new Mock<IProductRepository>();
            AdminController target = new AdminController(mockRepo.Object);
            // Arrange - TempData
            Mock<ITempDataDictionary> mockTempData = new Mock<ITempDataDictionary>();
            target.TempData = mockTempData.Object;
            // Arrange - product
            Product product = new Product { Name = "ProductName" };
            // Arrange - model invalid.
            target.ModelState.AddModelError("", "");

            // Act
            IActionResult result = target.Edit(product);
            Product resultProduct = GetViewModel<Product>(result);
            
            // Assert - product not saved.
            mockRepo.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            // Assert - Same view returned.
            Assert.IsType<ViewResult>(result);
            Assert.Same(product, resultProduct);
        }

        [Fact]
        public void Can_Delete_Valid_Products() {
            // Arrange - repository
            Mock<IProductRepository> mockRepo = new Mock<IProductRepository>();
            mockRepo.SetupGet(m => m.Products).Returns(new List<Product> {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" }
                }.AsQueryable<Product>());
            AdminController target = new AdminController(mockRepo.Object);
            // Arrange - TempData
            Mock<ITempDataDictionary> mockTempData = new Mock<ITempDataDictionary>();
            target.TempData = mockTempData.Object;

            // Act
            IActionResult result = target.Delete(3) as IActionResult;
            
            // Assert - product deleted.
            mockRepo.Verify(m => m.DeleteProduct(1), Times.Never);
            mockRepo.Verify(m => m.DeleteProduct(2), Times.Never);
            mockRepo.Verify(m => m.DeleteProduct(3), Times.Once);
            // Assert - redirects.
            Assert.IsType<RedirectToActionResult>(result);
            Assert.True((result as RedirectToActionResult).ActionName == nameof(AdminController.Index));
        }

        private T GetViewModel<T>(IActionResult result) where T : class {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}