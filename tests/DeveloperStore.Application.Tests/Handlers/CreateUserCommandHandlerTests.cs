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
    [Trait("CommandHandler", "User")]
    public class CreateUserCommandHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateUserCommandHandler(_userRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                UserDto = new CreateUserDto
                {
                    Email = "john.doe@email.com",
                    Username = "johndoe",
                    Password = "Password123",
                    Name = new NameDto { FirstName = "John", LastName = "Doe" },
                    Address = new AddressDto
                    {
                        City = "New York",
                        Street = "Broadway",
                        Number = 123,
                        ZipCode = "10001",
                        GeoLocation = new GeoLocationDto { Lat = "40.7128", Long = "-74.0060" }
                    },
                    Phone = "+1234567890",
                    Status = UserStatus.Active,
                    Role = UserRole.Customer
                }
            };

            _userRepository.ExistsByEmailAsync(Arg.Any<string>()).Returns(false);
            _userRepository.ExistsByUsernameAsync(Arg.Any<string>()).Returns(false);
            _userRepository.AddAsync(Arg.Any<User>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            var expectedUser = CreateValidUser();
            _mapper.Map<UserDto>(Arg.Any<User>()).Returns(new UserDto { Id = expectedUser.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _userRepository.Received(1).AddAsync(Arg.Any<User>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailExists()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                UserDto = new CreateUserDto { Email = "existing@email.com", Username = "johndoe" }
            };

            _userRepository.ExistsByEmailAsync(Arg.Any<string>()).Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUsernameExists()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                UserDto = new CreateUserDto { Email = "new@email.com", Username = "existinguser" }
            };

            _userRepository.ExistsByEmailAsync(Arg.Any<string>()).Returns(false);
            _userRepository.ExistsByUsernameAsync(Arg.Any<string>()).Returns(true);

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