using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "Auth")]
    public class ChangePasswordCommandHandlerTests
    {
        private readonly IAuthService _authService;
        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            _authService = Substitute.For<IAuthService>();
            _handler = new ChangePasswordCommandHandler(_authService);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenPasswordChangeSucceeds()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new ChangePasswordCommand
            {
                UserId = userId,
                ChangePasswordRequest = new ChangePasswordRequestDto
                {
                    CurrentPassword = "oldPassword",
                    NewPassword = "newPassword123"
                }
            };

            _authService.ChangePasswordAsync(userId, command.ChangePasswordRequest).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            await _authService.Received(1).ChangePasswordAsync(userId, command.ChangePasswordRequest);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCurrentPasswordIsWrong()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new ChangePasswordCommand
            {
                UserId = userId,
                ChangePasswordRequest = new ChangePasswordRequestDto
                {
                    CurrentPassword = "wrongPassword",
                    NewPassword = "newPassword123"
                }
            };

            _authService.ChangePasswordAsync(userId, command.ChangePasswordRequest)
                .ThrowsAsync(new UnauthorizedAccessException("Senha atual incorreta"));

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenNewPasswordIsInvalid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new ChangePasswordCommand
            {
                UserId = userId,
                ChangePasswordRequest = new ChangePasswordRequestDto
                {
                    CurrentPassword = "oldPassword",
                    NewPassword = "123" // Senha muito curta
                }
            };

            _authService.ChangePasswordAsync(userId, command.ChangePasswordRequest)
                .ThrowsAsync(new ArgumentException("Nova senha deve ter pelo menos 6 caracteres"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}