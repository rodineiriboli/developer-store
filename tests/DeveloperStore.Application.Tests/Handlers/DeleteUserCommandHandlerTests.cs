using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DeleteUserCommandHandler(_userRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand { UserId = userId };

            var user = CreateValidUser();
            _userRepository.GetByIdAsync(userId).Returns(user);
            _userRepository.DeleteAsync(Arg.Any<User>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            _mapper.Map<UserDto>(Arg.Any<User>()).Returns(new UserDto { Id = userId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _userRepository.Received(1).DeleteAsync(Arg.Any<User>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var command = new DeleteUserCommand { UserId = Guid.NewGuid() };
            _userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        private User CreateValidUser()
        {
            var geoLocation = new GeoLocation("40.7128", "-74.0060");
            var address = new Address("New York", "Broadway", 123, "10001", geoLocation);
            var name = new Name("John", "Doe");

            return new User(
                "john.doe@email.com",
                "johndoe",
                "Password123",
                name,
                address,
                "+1234567890",
                DeveloperStore.Domain.Enums.UserStatus.Active,
                DeveloperStore.Domain.Enums.UserRole.Customer
            );
        }
    }
}