using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Domain.Tests.ValueObjects
{
    public class GeoLocationTests
    {
        [Fact]
        public void Constructor_ShouldCreateGeoLocation_WithValidParameters()
        {
            // Arrange & Act
            var geoLocation = new GeoLocation("40.7128", "-74.0060");

            // Assert
            Assert.Equal("40.7128", geoLocation.Lat);
            Assert.Equal("-74.0060", geoLocation.Long);
        }

        [Theory]
        [InlineData("40.7128", "-74.0060", "40.7128", "-74.0060", true)]
        [InlineData("40.7128", "-74.0060", "34.0522", "-118.2437", false)]
        public void Equals_ShouldReturnCorrectResult(string lat1, string long1, string lat2, string long2, bool expected)
        {
            // Arrange
            var geo1 = new GeoLocation(lat1, long1);
            var geo2 = new GeoLocation(lat2, long2);

            // Act & Assert
            Assert.Equal(expected, geo1.Equals(geo2));
        }
    }
}