using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Domain.Tests.Entities
{
    [Trait("Entity", "Auth")]
    public class UserAuthTests
    {
        private readonly Name _validName;
        private readonly Address _validAddress;

        public UserAuthTests()
        {
            var geoLocation = new GeoLocation("40.7128", "-74.0060");
            _validAddress = new Address("New York", "Broadway", 123, "10001", geoLocation);
            _validName = new Name("John", "Doe");
        }

        [Fact]
        public void SetPassword_ShouldHashPassword_WhenPasswordIsValid()
        {
            // Arrange
            var user = new User("test@email.com", "testuser", "password123", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer);

            // Assert
            Assert.NotNull(user.PasswordHash);
            Assert.NotEqual("password123", user.PasswordHash);
            Assert.True(user.PasswordHash.Length > 0);
        }

        [Fact]
        public void SetPassword_ShouldThrowException_WhenPasswordIsTooShort()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
                new User("test@email.com", "testuser", "123", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer));
        }

        [Fact]
        public void SetPassword_ShouldThrowException_WhenPasswordIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() =>
                new User("test@email.com", "testuser", "", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer));
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var password = "correctPassword123";
            var user = new User("test@email.com", "testuser", password, _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer);

            // Act
            var result = user.VerifyPassword(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User("test@email.com", "testuser", "correctPassword123", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer);

            // Act
            var result = user.VerifyPassword("wrongPassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsNull()
        {
            // Arrange
            var user = new User("test@email.com", "testuser", "password123", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer);

            // Act
            var result = user.VerifyPassword(null);

            // Assert
            Assert.False(result);
        }

        //[Fact]
        //public void VerifyPassword_ShouldReturnFalse_WhenPasswordHashIsNull()
        //{
        //    // Arrange
        //    var user = new User("test@email.com", "testuser", "password123", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer);

        //    // Simular PasswordHash nulo (via reflection para teste)
        //    //var passwordHashField = typeof(User).GetField("PasswordHash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        //    var passwordHashProperty = typeof(User).GetProperty("PasswordHash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        //    passwordHashProperty.SetValue(user, null);

        //    // Act
        //    var result = user.VerifyPassword("password123");

        //    // Assert
        //    Assert.False(result);
        //}

        [Fact]
        public void ChangePassword_ShouldUpdatePasswordHash()
        {
            // Arrange
            var user = new User("test@email.com", "testuser", "oldPassword123", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer);
            var oldHash = user.PasswordHash;
            var newPassword = "newPassword456";

            // Act
            user.ChangePassword(newPassword);

            // Assert
            Assert.NotNull(user.PasswordHash);
            Assert.NotEqual(oldHash, user.PasswordHash);
            Assert.True(user.VerifyPassword(newPassword));
            Assert.False(user.VerifyPassword("oldPassword123"));
        }

        [Fact]
        public void ChangePassword_ShouldThrowException_WhenNewPasswordIsInvalid()
        {
            // Arrange
            var user = new User("test@email.com", "testuser", "oldPassword123", _validName, _validAddress, "+1234567890", UserStatus.Active, UserRole.Customer);

            // Act & Assert
            Assert.Throws<DomainException>(() => user.ChangePassword("123"));
        }
    }
}