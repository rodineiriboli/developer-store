using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateUserCommandHandler(_userRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldUpdateUser_WhenDataIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                UserId = userId,
                UserDto = new UpdateUserDto
                {
                    Email = "updated@email.com",
                    Username = "updateduser",
                    Name = new NameDto { FirstName = "Updated", LastName = "User" },
                    Address = new AddressDto
                    {
                        City = "Los Angeles",
                        Street = "Sunset Blvd",
                        Number = 456,
                        ZipCode = "90001",
                        GeoLocation = new GeoLocationDto { Lat = "34.0522", Long = "-118.2437" }
                    },
                    Phone = "+0987654321",
                    Status = UserStatus.Inactive,
                    Role = UserRole.Admin
                }
            };

            var existingUser = CreateValidUser();
            _userRepository.GetByIdAsync(userId).Returns(existingUser);
            _userRepository.GetByEmailAsync(Arg.Any<string>()).Returns((User)null);
            _userRepository.GetByUsernameAsync(Arg.Any<string>()).Returns((User)null);
            _userRepository.UpdateAsync(Arg.Any<User>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            _mapper.Map<UserDto>(Arg.Any<User>()).Returns(new UserDto { Id = userId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _userRepository.Received(1).UpdateAsync(Arg.Any<User>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var command = new UpdateUserCommand { UserId = Guid.NewGuid() };
            _userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailExistsForOtherUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                UserId = userId,
                UserDto = new UpdateUserDto { Email = "existing@email.com" }
            };

            var existingUser = CreateValidUser();
            var otherUser = CreateValidUser();

            _userRepository.GetByIdAsync(userId).Returns(existingUser);
            _userRepository.GetByEmailAsync(Arg.Any<string>()).Returns(otherUser);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() =>
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
                UserStatus.Active,
                UserRole.Customer
            );
        }
    }
}