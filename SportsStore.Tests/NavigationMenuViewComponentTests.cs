using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Namespace {
    public class NavigationMenuViewComponentTests {
        [Fact]
        public void Can_Select_Categories() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(
                new Product[] {
                    new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                    new Product { ProductId = 2, Name = "P2", Category = "Cat2" },
                    new Product { ProductId = 3, Name = "P3", Category = "Cat3" },
                    new Product { ProductId = 4, Name = "P4", Category = "Cat1" },
                    new Product { ProductId = 5, Name = "P5", Category = "Cat1" },
                    new Product { ProductId = 6, Name = "P6", Category = "Cat3" },
                    new Product { ProductId = 7, Name = "P7", Category = "Cat1" },
                    new Product { ProductId = 8, Name = "P8", Category = "Cat1" }
                }.AsQueryable()
            );
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            
            // Act
            string[] results = ((target.Invoke() as ViewViewComponentResult).ViewData.Model as IEnumerable<string>).ToArray();

            // Assert
            Assert.True(results.Count() == 3);
            Assert.True(Enumerable.SequenceEqual(new string[] { "Cat1", "Cat2", "Cat3" }, results));
        }

        [Fact]
        public void Indicates_Selected_Category() {
            // Arrange
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(
                new Product[] {
                    new Product { ProductId = 1, Name = "P1", Category = "Cat1" },
                    new Product { ProductId = 2, Name = "P2", Category = "Cat2" }
                }.AsQueryable()
            );
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext {
                ViewContext = new ViewContext {
                    RouteData = new RouteData()
                }
            };
            target.RouteData.Values["category"] = "Cat2";
            
            // Act
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            
            // Assert
            Assert.Equal("Cat2", result);
        }
    }
}