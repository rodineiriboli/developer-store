using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using Xunit;

namespace DeveloperStore.Domain.Tests.Entities
{
    [Trait("Entity", "Cart")]
    public class CartTests
    {
        [Fact]
        public void Constructor_ShouldCreateCart_WithValidParameters()
        {
            // Arrange & Act
            var userId = Guid.NewGuid();
            var cart = new Cart(userId, DateTime.Now);

            // Assert
            Assert.Equal(userId, cart.UserId);
            Assert.NotNull(cart.Products);
            Assert.Empty(cart.Products);
        }

        [Fact]
        public void AddProduct_ShouldAddProduct_WhenQuantityIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(userId, DateTime.Now);

            // Act
            cart.AddProduct(productId, 2);

            // Assert
            Assert.Single(cart.Products);
            Assert.Equal(productId, cart.Products.First().ProductId);
            Assert.Equal(2, cart.Products.First().Quantity);
        }

        [Fact]
        public void AddProduct_ShouldUpdateQuantity_WhenProductAlreadyExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId, 2);

            // Act
            cart.AddProduct(productId, 3);

            // Assert
            Assert.Single(cart.Products);
            Assert.Equal(productId, cart.Products.First().ProductId);
            Assert.Equal(5, cart.Products.First().Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddProduct_ShouldThrowException_WhenQuantityIsInvalid(int invalidQuantity)
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);

            // Act & Assert
            Assert.Throws<DomainException>(() => cart.AddProduct(productId, invalidQuantity));
        }

        [Fact]
        public void UpdateProductQuantity_ShouldUpdateQuantity_WhenProductExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId, 2);

            // Act
            cart.UpdateProductQuantity(productId, 5);

            // Assert
            Assert.Equal(5, cart.Products.First().Quantity);
        }

        [Fact]
        public void UpdateProductQuantity_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);

            // Act & Assert
            Assert.Throws<DomainException>(() => cart.UpdateProductQuantity(productId, 5));
        }

        [Fact]
        public void RemoveProduct_ShouldRemoveProduct_WhenProductExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId, 2);
            cart.AddProduct(productId2, 1);

            // Act
            cart.RemoveProduct(productId);

            // Assert
            Assert.Single(cart.Products);
            Assert.Equal(productId2, cart.Products.First().ProductId);
        }

        [Fact]
        public void RemoveProduct_ShouldDoNothing_WhenProductNotExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId, 2);

            // Act
            cart.RemoveProduct(Guid.NewGuid()); // Non-existent product

            // Assert
            Assert.Single(cart.Products); // Should still have the original product
        }

        [Fact]
        public void ClearCart_ShouldRemoveAllProducts()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(Guid.NewGuid(), 2);
            cart.AddProduct(Guid.NewGuid(), 1);
            cart.AddProduct(Guid.NewGuid(), 3);

            // Act
            cart.ClearCart();

            // Assert
            Assert.Empty(cart.Products);
        }

        [Fact]
        public void ContainsProduct_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId, 2);

            // Act
            var contains = cart.ContainsProduct(productId);

            // Assert
            Assert.True(contains);
        }

        [Fact]
        public void ContainsProduct_ShouldReturnFalse_WhenProductNotExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId, 2);

            // Act
            var contains = cart.ContainsProduct(Guid.NewGuid());

            // Assert
            Assert.False(contains);
        }

        [Fact]
        public void GetProductQuantity_ShouldReturnQuantity_WhenProductExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId, 2);

            // Act
            var quantity = cart.GetProductQuantity(productId);

            // Assert
            Assert.Equal(2, quantity);
        }

        [Fact]
        public void GetProductQuantity_ShouldReturnZero_WhenProductNotExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart(cartId, DateTime.Now);

            // Act
            var quantity = cart.GetProductQuantity(productId);

            // Assert
            Assert.Equal(0, quantity);
        }
    }
}