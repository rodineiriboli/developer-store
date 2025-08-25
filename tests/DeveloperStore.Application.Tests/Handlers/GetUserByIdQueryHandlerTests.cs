using DeveloperStore.Application.Queries;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;
using AutoMapper;
using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "User")]
    public class GetUserByIdQueryHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetUserByIdQueryHandler(_userRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserByIdQuery { UserId = userId };

            var user = CreateValidUser();
            _userRepository.GetByIdAsync(userId).Returns(user);
            _mapper.Map<UserDto>(user).Returns(new UserDto { Id = userId });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var query = new GetUserByIdQuery { UserId = Guid.NewGuid() };
            _userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
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