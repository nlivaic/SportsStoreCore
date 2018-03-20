using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests {
    public class CartTests {
        [Fact]
        public void Can_Add_New_Lines() {
            // Arrange - setup products
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };
            Product p3 = new Product { ProductId = 3, Name = "P3" };
            Product p4 = new Product { ProductId = 4, Name = "P4" };
            // Arrange - new cart
            Cart target = new Cart();
            // Arrange - add to cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p3, 1);
            target.AddItem(p4, 1);

            // Act
            CartLine[] result = target.Lines.ToArray();

            // Assert
            Assert.True(result.Length == 4);
            Assert.Equal(result[0].Product, p1);
            Assert.True(result[0].Quantity == 1);
            Assert.Equal(result[1].Product, p2);
            Assert.True(result[1].Quantity == 1);
            Assert.Equal(result[2].Product, p3);
            Assert.True(result[2].Quantity == 1);
            Assert.Equal(result[3].Product, p4);
            Assert.True(result[3].Quantity == 1);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines() {
            // Arrange - setup products
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };
            // Arrange - new cart
            Cart target = new Cart();
            // Arrange - add to cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);

            // Act
            CartLine[] result = (target.Lines as List<CartLine>).ToArray();
            
            // Assert
            Assert.True(result[0].Quantity == 4);
            Assert.True(result[1].Quantity == 1);
        }

        [Fact]
        public void Can_Remove_Line() {
            // Arrange - setup products
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };
            Product p3 = new Product { ProductId = 3, Name = "P3" };
            Product p4 = new Product { ProductId = 4, Name = "P4" };
            // Arrange - new cart
            Cart target = new Cart();
            // Arrange - add to cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p3, 1);
            target.AddItem(p4, 1);

            // Act
            target.RemoveLine(p2);
            target.RemoveLine(p3);
            CartLine[] result = target.Lines.ToArray();
            
            // Assert
            Assert.True(result.Length == 2);
            Assert.Equal(result[0].Product, p1);
            Assert.Equal(result[1].Product, p4);
            Assert.True(result.Where(c => c.Product.ProductId == 2).Count() == 0);
            Assert.True(result.Where(c => c.Product.ProductId == 3).Count() == 0);
        }

        [Fact]
        public void Can_Calculate_Total() {
            // Arrange - setup products
            Product p1 = new Product { ProductId = 1, Name = "P1", Price = 123.45M };
            Product p2 = new Product { ProductId = 2, Name = "P2", Price = 44.99M };
            Product p3 = new Product { ProductId = 3, Name = "P3", Price = 37.75M };
            Product p4 = new Product { ProductId = 4, Name = "P4", Price = 14.39M };
            // Arrange - new cart
            Cart target = new Cart();
            
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p3, 3);
            target.AddItem(p4, 2);
                        
            // Assert
            Assert.True(target.ComputeTotalValue() == 310.47M);
        }

        [Fact]
        public void Can_Clear_Contents() {
            // Arrange - setup products
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };
            Product p3 = new Product { ProductId = 3, Name = "P3" };
            Product p4 = new Product { ProductId = 4, Name = "P4" };
            // Arrange - new cart
            Cart target = new Cart();
            // Arrange - add to cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p3, 3);
            target.AddItem(p4, 2);

            // Act
            target.Clear();

            // Assert
            Assert.True(target.Lines.Count() == 0);
        }
    }
}