using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Tests.Entities
{
    [Trait("Entity", "User")]
    public class UserTests
    {
        private readonly Name _validName;
        private readonly Address _validAddress;
        private readonly GeoLocation _validGeoLocation;

        public UserTests()
        {
            _validGeoLocation = new GeoLocation("40.7128", "-74.0060");
            _validAddress = new Address("New York", "Broadway", 123, "10001", _validGeoLocation);
            _validName = new Name("John", "Doe");
        }

        [Fact]
        public void Constructor_ShouldCreateUser_WithValidParameters()
        {
            // Arrange & Act
            var user = new User(
                "john.doe@email.com",
                "johndoe",
                "Password123",
                _validName,
                _validAddress,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            );

            // Assert
            Assert.Equal("john.doe@email.com", user.Email);
            Assert.Equal("johndoe", user.Username);
            Assert.Equal(_validName, user.Name);
            Assert.Equal(_validAddress, user.Address);
            Assert.Equal("+1234567890", user.Phone);
            Assert.Equal(UserStatus.Active, user.Status);
            Assert.Equal(UserRole.Customer, user.Role);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid-email")]
        public void Constructor_ShouldThrowException_WhenEmailIsInvalid(string invalidEmail)
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new User(
                invalidEmail,
                "johndoe",
                "Password123",
                _validName,
                _validAddress,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            ));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_ShouldThrowException_WhenUsernameIsInvalid(string invalidUsername)
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new User(
                "john.doe@email.com",
                invalidUsername,
                "Password123",
                _validName,
                _validAddress,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            ));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new User(
                "john.doe@email.com",
                "johndoe",
                "Password123",
                null,
                _validAddress,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            ));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenAddressIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new User(
                "john.doe@email.com",
                "johndoe",
                "Password123",
                _validName,
                null,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            ));
        }

        [Fact]
        public void Update_ShouldUpdateUserProperties_WithValidParameters()
        {
            // Arrange
            var user = CreateValidUser();
            var newName = new Name("Jane", "Smith");
            var newGeoLocation = new GeoLocation("34.0522", "-118.2437");
            var newAddress = new Address("Los Angeles", "Sunset Blvd", 456, "90001", newGeoLocation);

            // Act
            user.Update(
                "jane.smith@email.com",
                "janesmith",
                newName,
                newAddress,
                "+0987654321",
                UserStatus.Inactive,
                UserRole.Admin
            );

            // Assert
            Assert.Equal("jane.smith@email.com", user.Email);
            Assert.Equal("janesmith", user.Username);
            Assert.Equal(newName, user.Name);
            Assert.Equal(newAddress, user.Address);
            Assert.Equal("+0987654321", user.Phone);
            Assert.Equal(UserStatus.Inactive, user.Status);
            Assert.Equal(UserRole.Admin, user.Role);
        }

        [Fact]
        public void Deactivate_ShouldSetStatusToInactive()
        {
            // Arrange
            var user = CreateValidUser();

            // Act
            user.Deactivate();

            // Assert
            Assert.Equal(UserStatus.Inactive, user.Status);
        }

        [Fact]
        public void Deactivate_ShouldThrowException_WhenUserIsAlreadyInactive()
        {
            // Arrange
            var user = CreateValidUser();
            user.Deactivate();

            // Act & Assert
            Assert.Throws<DomainException>(() => user.Deactivate());
        }

        [Fact]
        public void Activate_ShouldSetStatusToActive()
        {
            // Arrange
            var user = CreateValidUser();
            user.Deactivate(); // Ensure user is inactive first

            // Act
            user.Activate();

            // Assert
            Assert.Equal(UserStatus.Active, user.Status);
        }

        [Fact]
        public void Activate_ShouldThrowException_WhenUserIsAlreadyActive()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            Assert.Throws<DomainException>(() => user.Activate());
        }

        [Fact]
        public void Update_ShouldThrowException_WhenEmailIsInvalid()
        {
            // Arrange
            var user = CreateValidUser();

            // Act & Assert
            Assert.Throws<DomainException>(() => user.Update(
                "invalid-email",
                "johndoe",
                _validName,
                _validAddress,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            ));
        }

        private User CreateValidUser()
        {
            return new User(
                "john.doe@email.com",
                "johndoe",
                "Password123",
                _validName,
                _validAddress,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            );
        }
    }
}