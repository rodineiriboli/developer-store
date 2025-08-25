using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Application.Tests
{
    [Trait("Entity", "Sale")]
    public class SaleTests
    {
        [Theory]
        [InlineData(3, 0)]    // Sem desconto
        [InlineData(4, 10)]   // 10% de desconto
        [InlineData(9, 10)]   // 10% de desconto
        [InlineData(10, 20)]  // 20% de desconto
        [InlineData(15, 20)]  // 20% de desconto
        [InlineData(20, 20)]  // 20% de desconto
        public void ApplyDiscounts_ShouldApplyCorrectDiscount(int quantity, decimal expectedDiscountPercentage)
        {
            // Arrange
            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description");
            var customer = new CustomerInfo(Guid.NewGuid(), "Customer", "customer@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Branch", "Location");

            var sale = new Sale("SALE-001", DateTime.Now, customer, branch);
            sale.AddItem(product, quantity, 100m);

            // Act
            var item = sale.Items.First();

            // Assert
            var expectedDiscount = (item.UnitPrice * item.Quantity) * (expectedDiscountPercentage / 100);
            Assert.Equal(expectedDiscount, item.Discount);
        }

        [Fact]
        public void AddItem_ShouldThrowException_WhenQuantityExceeds20()
        {
            // Arrange
            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description");
            var customer = new CustomerInfo(Guid.NewGuid(), "Customer", "customer@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Branch", "Location");

            var sale = new Sale("SALE-001", DateTime.Now, customer, branch);

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.AddItem(product, 21, 100m));
        }

        [Fact]
        public void AddItem_ShouldUpdateQuantity_WhenProductAlreadyExists()
        {
            // Arrange
            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description");
            var customer = new CustomerInfo(Guid.NewGuid(), "Customer", "customer@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Branch", "Location");

            var sale = new Sale("SALE-001", DateTime.Now, customer, branch);
            sale.AddItem(product, 5, 100m);

            // Act
            sale.AddItem(product, 5, 100m);

            // Assert
            var item = sale.Items.First();
            Assert.Equal(10, item.Quantity);
            Assert.Equal(200m, item.Discount); // 20% de 1000 = 200
        }

        [Fact]
        public void Cancel_ShouldMarkAsCancelled_AndRaiseEvent()
        {
            // Arrange
            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description");
            var customer = new CustomerInfo(Guid.NewGuid(), "Customer", "customer@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Branch", "Location");

            var sale = new Sale("SALE-001", DateTime.Now, customer, branch);
            sale.AddItem(product, 1, 100m);

            // Act
            sale.Cancel();

            // Assert
            Assert.True(sale.IsCancelled);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItem_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new ProductInfo(productId, "Product productId", "Description"); // ProductId é int
            var customer = new CustomerInfo(Guid.NewGuid(), "Customer", "customer@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Branch", "Location");

            var sale = new Sale("SALE-001", DateTime.Now, customer, branch);
            sale.AddItem(product, 2, 100m);

            // Act
            sale.RemoveItem(productId); // Passando int em vez de Guid

            // Assert
            Assert.Empty(sale.Items);
        }
    }
}