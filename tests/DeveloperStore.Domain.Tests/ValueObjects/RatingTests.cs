using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Domain.Tests.ValueObjects
{
    [Trait("ValueObjects", "Rating")]
    public class RatingTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(2.5, 100)]
        [InlineData(5, 1000)]
        public void Constructor_ShouldCreateRating_WithValidParameters(decimal rate, int count)
        {
            // Arrange & Act
            var rating = new Rating(rate, count);

            // Assert
            Assert.Equal(rate, rating.Rate);
            Assert.Equal(count, rating.Count);
        }

        [Theory]
        [InlineData(-1, 100)]
        [InlineData(6, 100)]
        [InlineData(5.1, 100)]
        public void Constructor_ShouldThrowException_WhenRateIsInvalid(decimal invalidRate, int count)
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new Rating(invalidRate, count));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenCountIsNegative()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new Rating(4, -1));
        }

        [Theory]
        [InlineData(4.5, 100, 4.5, 100, true)]
        [InlineData(4.5, 100, 4.0, 100, false)]
        [InlineData(4.5, 100, 4.5, 50, false)]
        public void Equals_ShouldReturnCorrectResult(decimal rate1, int count1, decimal rate2, int count2, bool expected)
        {
            // Arrange
            var rating1 = new Rating(rate1, count1);
            var rating2 = new Rating(rate2, count2);

            // Act & Assert
            Assert.Equal(expected, rating1.Equals(rating2));
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameValue_ForEqualObjects()
        {
            // Arrange
            var rating1 = new Rating(4.5m, 100);
            var rating2 = new Rating(4.5m, 100);

            // Act & Assert
            Assert.Equal(rating1.GetHashCode(), rating2.GetHashCode());
        }
    }
}