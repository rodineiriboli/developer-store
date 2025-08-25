using Castle.Core.Configuration;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using DeveloperStore.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace DeveloperStore.Application.Tests.Services
{
    [Trait("Service", "Auth")]
    public class AuthServiceTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _tokenService = Substitute.For<ITokenService>();
            _emailService = Substitute.For<IEmailService>();
            _configuration = Substitute.For<IConfiguration>();

            _authService = new AuthService(_userRepository, _tokenService, _emailService, _configuration);
        }

        //[Fact]
        //public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        //{
        //    // Arrange
        //    var loginRequest = new LoginRequestDto
        //    {
        //        Username = "testuser",
        //        Password = "password123"
        //    };

        //    var user = CreateValidUser();
        //    _userRepository.GetByUsernameAsync("testuser").Returns(user);
        //    _tokenService.GenerateToken(Arg.Any<UserDto>()).Returns("jwt-token");

        //    // Act
        //    var result = await _authService.LoginAsync(loginRequest);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("jwt-token", result.Token);
        //    Assert.Equal("testuser", result.User.Username);
        //}

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Username = "nonexistent",
                Password = "password123"
            };

            _userRepository.GetByUsernameAsync("nonexistent").Returns((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _authService.LoginAsync(loginRequest));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsWrong()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            var user = CreateValidUser();
            _userRepository.GetByUsernameAsync("testuser").Returns(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _authService.LoginAsync(loginRequest));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserIsInactive()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                Username = "inactiveuser",
                Password = "password123"
            };

            var user = CreateValidUser();
            user.Deactivate(); // Tornar usuário inativo
            _userRepository.GetByUsernameAsync("inactiveuser").Returns(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _authService.LoginAsync(loginRequest));
        }

        //[Fact]
        //public async Task ChangePasswordAsync_ShouldReturnTrue_WhenPasswordChangeSucceeds()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var request = new ChangePasswordRequestDto
        //    {
        //        CurrentPassword = "oldPassword123",
        //        NewPassword = "newPassword456"
        //    };

        //    var user = CreateValidUser();
        //    _userRepository.GetByIdAsync(userId).Returns(user);

        //    // Act
        //    var result = await _authService.ChangePasswordAsync(userId, request);

        //    // Assert
        //    Assert.True(result);
        //    await _userRepository.Received(1).UpdateAsync(user);
        //}

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new ChangePasswordRequestDto
            {
                CurrentPassword = "oldPassword",
                NewPassword = "newPassword"
            };

            _userRepository.GetByIdAsync(userId).Returns((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _authService.ChangePasswordAsync(userId, request));
        }

        private User CreateValidUser()
        {
            var geoLocation = new GeoLocation("40.7128", "-74.0060");
            var address = new Address("New York", "Broadway", 123, "10001", geoLocation);
            var name = new Name("John", "Doe");

            return new User(
                "test@email.com",
                "testuser",
                "password123", // Senha correta
                name,
                address,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            );
        }
    }
}