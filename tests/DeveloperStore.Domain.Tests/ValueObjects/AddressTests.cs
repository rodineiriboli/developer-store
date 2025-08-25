using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Domain.Tests.ValueObjects
{
    [Trait("ValueObjects", "Address")]
    public class AddressTests
    {
        [Fact]
        public void Constructor_ShouldCreateAddress_WithValidParameters()
        {
            // Arrange
            var geoLocation = new GeoLocation("40.7128", "-74.0060");

            // Act
            var address = new Address("New York", "Broadway", 123, "10001", geoLocation);

            // Assert
            Assert.Equal("New York", address.City);
            Assert.Equal("Broadway", address.Street);
            Assert.Equal(123, address.Number);
            Assert.Equal("10001", address.ZipCode);
            Assert.Equal(geoLocation, address.GeoLocation);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameAddress()
        {
            // Arrange
            var geoLocation = new GeoLocation("40.7128", "-74.0060");
            var address1 = new Address("New York", "Broadway", 123, "10001", geoLocation);
            var address2 = new Address("New York", "Broadway", 123, "10001", geoLocation);

            // Act & Assert
            Assert.True(address1.Equals(address2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentAddress()
        {
            // Arrange
            var geoLocation1 = new GeoLocation("40.7128", "-74.0060");
            var geoLocation2 = new GeoLocation("34.0522", "-118.2437");

            var address1 = new Address("New York", "Broadway", 123, "10001", geoLocation1);
            var address2 = new Address("Los Angeles", "Sunset Blvd", 456, "90001", geoLocation2);

            // Act & Assert
            Assert.False(address1.Equals(address2));
        }
    }
}