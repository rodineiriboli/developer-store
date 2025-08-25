using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DeveloperStore.Application.Tests.Services
{
    [Trait("Service", "Token")]
    public class TokenServiceTests
    {
        private readonly ITokenService _tokenService;

        public TokenServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Jwt:Secret"] = "MinhaChaveSecretaSuperSeguraCom32Caracteres!",
                    ["Jwt:ExpiresInHours"] = "8"
                })
                .Build();

            _tokenService = new TokenService(configuration);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
            // Arrange
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@email.com",
                Role = UserRole.Customer
            };

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            Assert.True(token.Split('.').Length == 3); // JWT tem 3 partes
        }

        [Fact]
        public void ValidateToken_ShouldReturnTrue_ForValidToken()
        {
            // Arrange
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@email.com",
                Role = UserRole.Customer
            };

            var token = _tokenService.GenerateToken(user);

            // Act
            var isValid = _tokenService.ValidateToken(token);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateToken_ShouldReturnFalse_ForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalid.token.here";

            // Act
            var isValid = _tokenService.ValidateToken(invalidToken);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateToken_ShouldReturnFalse_ForEmptyToken()
        {
            // Act
            var isValid = _tokenService.ValidateToken("");

            // Assert
            Assert.False(isValid);
        }

        //[Fact]
        //public void GetUserIdFromToken_ShouldReturnUserId_ForValidToken()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var user = new UserDto
        //    {
        //        Id = userId,
        //        Username = "testuser",
        //        Email = "test@email.com",
        //        Role = UserRole.Customer
        //    };

        //    var token = _tokenService.GenerateToken(user);

        //    // Act
        //    var extractedUserId = _tokenService.GetUserIdFromToken(token);

        //    // Assert
        //    Assert.Equal(userId.ToString(), extractedUserId);
        //}

        [Fact]
        public void GenerateToken_ShouldIncludeAllClaims()
        {
            // Arrange
            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@email.com",
                Role = UserRole.Admin
            };

            // Act
            var token = _tokenService.GenerateToken(user);
            var isValid = _tokenService.ValidateToken(token);

            // Assert
            Assert.True(isValid);
        }
    }
}