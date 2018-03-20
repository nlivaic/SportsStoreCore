using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System.Linq;
using Xunit;

namespace SportsStore.Tests {
    public class OrderControllerTests {
        [Fact]
        public void Cannot_Checkout_Empty_Cart() {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            OrderController target = new OrderController(mock.Object, cart);

            // Act
            ViewResult result = target.Checkout(null) as ViewResult;
            
            // Assert - order has not been saved.
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // Assert - default view returned.
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Assert - model is invalidated.
            Assert.True(result.ViewData.ModelState.IsValid == false);
            // Assert - proper error message is returned.
            Assert.True(result.ViewData.ModelState[""].Errors[0].ErrorMessage == "Your cart is empty.");
        }

        [Fact]
        public void Cannot_Checkout_With_Invalid_Shipping_Details() {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("error", "error");          // Manually invalidate the model.
            
            // Act
            ViewResult result = target.Checkout(order) as ViewResult;
            
            // Assert - order has not been saved.
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // Assert - default view is returned.
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Assert - model is invalidated.
            Assert.True(result.ViewData.ModelState.IsValid == false);
        }

        [Fact]
        public void Can_Ship_With_Valid_Shipping_Details() {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Product product = new Product();
            cart.AddItem(product, 1);
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            // Act
            RedirectToActionResult result = target.Checkout(order) as RedirectToActionResult;
            
            // Assert - order has been saved.
            mock.Verify(m => m.SaveOrder(It.Is<Order>(o => o == order)), Times.Once);
            // Assert - order is enriched with line items
            Assert.True(order.Lines.Count == 1);
            Assert.Same(product, order.Lines.ToArray()[0].Product);
            // Assert - Completed view is returned.
            Assert.True(result.ActionName == "Completed");
        }
    }
}