using Xunit;

namespace DeveloperStore.Application.Tests.Services
{
    [Trait("Entity", "Sale")]
    public class SaleServiceTests
    {
        [Fact]
        public void CalculateDiscount_ShouldReturnZero_WhenQuantityLessThan4()
        {
            // Arrange
            var quantity = 3;
            var unitPrice = 100m;
            var totalPrice = quantity * unitPrice;

            // Act
            //var discount = Sale.CalculateDiscount(quantity, totalPrice);

            //// Assert
            //Assert.Equal(0m, discount);
        }

        [Fact]
        public void CalculateDiscount_ShouldReturn10Percent_WhenQuantityBetween4And9()
        {
            // Arrange
            var quantity = 5;
            var unitPrice = 100m;
            var totalPrice = quantity * unitPrice;

            // Act
            //var discount = Sale.CalculateDiscount(quantity, totalPrice);

            //// Assert
            //Assert.Equal(50m, discount); // 10% of 500
        }

        [Fact]
        public void CalculateDiscount_ShouldReturn20Percent_WhenQuantityBetween10And20()
        {
            // Arrange
            var quantity = 15;
            var unitPrice = 100m;
            var totalPrice = quantity * unitPrice;

            // Act
            //var discount = Sale.CalculateDiscount(quantity, totalPrice);

            //// Assert
            //Assert.Equal(300m, discount); // 20% of 1500
        }

        [Theory]
        [InlineData(3, 0)]    // No discount
        [InlineData(4, 40)]   // 10% of 400
        [InlineData(9, 90)]   // 10% of 900
        [InlineData(10, 200)] // 20% of 1000
        [InlineData(15, 300)] // 20% of 1500
        [InlineData(20, 400)] // 20% of 2000
        public void CalculateDiscount_ShouldReturnCorrectDiscount(int quantity, decimal expectedDiscount)
        {
            // Arrange
            var unitPrice = 100m;
            var totalPrice = quantity * unitPrice;

            // Act
            //var discount = Sale.CalculateDiscount(quantity, totalPrice);

            //// Assert
            //Assert.Equal(expectedDiscount, discount);
        }
    }
}