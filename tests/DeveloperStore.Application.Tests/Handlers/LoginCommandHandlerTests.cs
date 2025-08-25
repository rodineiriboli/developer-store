using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "Auth")]
    public class LoginCommandHandlerTests
    {
        private readonly IAuthService _authService;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _authService = Substitute.For<IAuthService>();
            _handler = new LoginCommandHandler(_authService);
        }

        [Fact]
        public async Task Handle_ShouldReturnLoginResponse_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Username = "testuser",
                    Password = "password123"
                }
            };

            var expectedResponse = new LoginResponseDto
            {
                Token = "jwt-token-here",
                User = new UserDto { Username = "testuser", Email = "test@email.com" },
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };

            _authService.LoginAsync(command.LoginRequest).Returns(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Token, result.Token);
            Assert.Equal(expectedResponse.User.Username, result.User.Username);
            await _authService.Received(1).LoginAsync(command.LoginRequest);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAuthServiceFails()
        {
            // Arrange
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Username = "testuser",
                    Password = "wrongpassword"
                }
            };

            _authService.LoginAsync(command.LoginRequest).ThrowsAsync(new UnauthorizedAccessException("Credenciais inválidas"));

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldPropagateExceptions_FromAuthService()
        {
            // Arrange
            var command = new LoginCommand
            {
                LoginRequest = new LoginRequestDto
                {
                    Username = "testuser",
                    Password = "password123"
                }
            };

            _authService.LoginAsync(command.LoginRequest).ThrowsAsync(new Exception("Erro interno"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}