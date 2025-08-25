using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Tests.Entities
{
    [Trait("Entity", "Sale")]
    public class SaleTests
    {
        private readonly CustomerInfo _validCustomer;
        private readonly BranchInfo _validBranch;
        private readonly ProductInfo _validProduct;

        public SaleTests()
        {
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            _validCustomer = new CustomerInfo(customerId, "John Doe", "john@email.com");
            _validBranch = new BranchInfo(branchId, "Loja Principal", "São Paulo");
            _validProduct = new ProductInfo(productId, "Product 1", "Description 1");
        }

        [Fact]
        public void Constructor_ShouldCreateSale_WithValidParameters()
        {
            // Arrange & Act
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);

            // Assert
            Assert.Equal("SALE-001", sale.SaleNumber);
            Assert.Equal(_validCustomer, sale.Customer);
            Assert.Equal(_validBranch, sale.Branch);
            Assert.Equal(0, sale.TotalAmount);
            Assert.False(sale.IsCancelled);
            Assert.Empty(sale.Items);
        }

        [Fact]
        public void AddItem_ShouldAddProduct_WhenQuantityIsValid()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);

            // Act
            sale.AddItem(_validProduct, 2, 100m);

            // Assert
            Assert.Single(sale.Items);
            Assert.Equal(200m, sale.TotalAmount); // 2 * 100
        }

        [Fact]
        public void AddItem_ShouldApplyDiscount_WhenQuantityIs4()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);

            // Act
            sale.AddItem(_validProduct, 4, 100m); // 10% discount

            // Assert
            var item = sale.Items.First();
            Assert.Equal(40m, item.Discount); // 10% of 400
            Assert.Equal(360m, sale.TotalAmount); // 400 - 40
        }

        [Fact]
        public void AddItem_ShouldApplyDiscount_WhenQuantityIs10()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);

            // Act
            sale.AddItem(_validProduct, 10, 100m); // 20% discount

            // Assert
            var item = sale.Items.First();
            Assert.Equal(200m, item.Discount); // 20% of 1000
            Assert.Equal(800m, sale.TotalAmount); // 1000 - 200
        }

        [Fact]
        public void AddItem_ShouldThrowException_WhenQuantityExceeds20()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.AddItem(_validProduct, 21, 100m));
        }

        [Fact]
        public void AddItem_ShouldUpdateQuantity_WhenProductAlreadyExists()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);
            sale.AddItem(_validProduct, 2, 100m);

            // Act
            sale.AddItem(_validProduct, 3, 100m);

            // Assert
            Assert.Single(sale.Items);
            Assert.Equal(5, sale.Items.First().Quantity);
            Assert.Equal(450m, sale.TotalAmount); // (5 * 100) * 0.5 //Desconto de 10% se qtd maior que 4 items
        }

        [Fact]
        public void RemoveItem_ShouldRemoveProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);
            sale.AddItem(_validProduct, 2, 100m);
            sale.AddItem(new ProductInfo(productId, "Product 2", "Description 2"), 1, 50m);

            // Act
            sale.RemoveItem(_validProduct.ProductId);

            // Assert
            Assert.Single(sale.Items);
            Assert.Equal(50m, sale.TotalAmount);
        }

        [Fact]
        public void Cancel_ShouldMarkAsCancelled()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);
            sale.AddItem(_validProduct, 2, 100m);

            // Act
            sale.Cancel();

            // Assert
            Assert.True(sale.IsCancelled);
        }

        [Fact]
        public void Cancel_ShouldThrowException_WhenAlreadyCancelled()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);
            sale.Cancel();

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.Cancel());
        }

        [Fact]
        public void AddItem_ShouldThrowException_WhenSaleIsCancelled()
        {
            // Arrange
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);
            sale.Cancel();

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.AddItem(_validProduct, 2, 100m));
        }

        [Fact]
        public void ApplyDiscounts_ShouldApplyCorrectDiscounts_ForMultipleItems()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var sale = new Sale("SALE-001", DateTime.Now, _validCustomer, _validBranch);
            var product2 = new ProductInfo(productId, "Product productId", "Description productId");

            // Act
            sale.AddItem(_validProduct, 5, 100m); // 10% discount
            sale.AddItem(product2, 15, 200m); // 20% discount

            // Assert
            Assert.Equal(2, sale.Items.Count);
            Assert.Equal(50m, sale.Items.ToList()[0].Discount); // 10% of 500
            Assert.Equal(600m, sale.Items.ToList()[1].Discount); // 20% of 3000
            Assert.Equal(2850m, sale.TotalAmount); // (500-50) + (3000-600)
        }
    }
}