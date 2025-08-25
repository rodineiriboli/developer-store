using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Tests.Entities
{
    [Trait("Entity", "Sale Item")]
    public class SaleItemTests
    {
        private readonly ProductInfo _validProduct;

        public SaleItemTests()
        {
            var productId = Guid.NewGuid();
            _validProduct = new ProductInfo(productId, "Product productId", "Description productId");
        }

        [Fact]
        public void Constructor_ShouldCreateSaleItem_WithValidParameters()
        {
            // Arrange & Act
            var saleItem = new SaleItem(_validProduct, 2, 100m);

            // Assert
            Assert.Equal(_validProduct, saleItem.Product);
            Assert.Equal(2, saleItem.Quantity);
            Assert.Equal(100m, saleItem.UnitPrice);
            Assert.Equal(0m, saleItem.Discount);
            Assert.Equal(200m, saleItem.TotalPrice); // 2 * 100
        }

        [Fact]
        public void ApplyDiscount_ShouldSetDiscount()
        {
            // Arrange
            var saleItem = new SaleItem(_validProduct, 2, 100m);

            // Act
            saleItem.ApplyDiscount(20m);

            // Assert
            Assert.Equal(20m, saleItem.Discount);
            Assert.Equal(180m, saleItem.TotalPrice); // 200 - 20
        }

        [Fact]
        public void UpdateQuantity_ShouldUpdateQuantity()
        {
            // Arrange
            var saleItem = new SaleItem(_validProduct, 2, 100m);

            // Act
            saleItem.UpdateQuantity(5);

            // Assert
            Assert.Equal(5, saleItem.Quantity);
            Assert.Equal(500m, saleItem.TotalPrice); // 5 * 100
        }

        [Fact]
        public void UpdateQuantity_ShouldThrowException_WhenQuantityExceeds20()
        {
            // Arrange
            var saleItem = new SaleItem(_validProduct, 2, 100m);

            // Act & Assert
            Assert.Throws<DomainException>(() => saleItem.UpdateQuantity(21));
        }

        [Fact]
        public void TotalPrice_ShouldCalculateCorrectly()
        {
            // Arrange
            var saleItem = new SaleItem(_validProduct, 3, 100m);
            saleItem.ApplyDiscount(30m);

            // Assert
            Assert.Equal(270m, saleItem.TotalPrice); // (3 * 100) - 30
        }
    }
}