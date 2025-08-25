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
    public class GetAllUsersQueryHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly GetAllUsersQueryHandler _handler;

        public GetAllUsersQueryHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllUsersQueryHandler(_userRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnUsersList_WhenUsersExist()
        {
            // Arrange
            var query = new GetAllUsersQuery { Page = 1, PageSize = 10 };

            var users = new List<User> { CreateValidUser(), CreateValidUser() };
            _userRepository.GetAllAsync(1, 10).Returns(users);
            _mapper.Map<List<UserDto>>(users).Returns(new List<UserDto>
            {
                new UserDto { Id = users[0].Id },
                new UserDto { Id = users[1].Id }
            });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var query = new GetAllUsersQuery { Page = 1, PageSize = 10 };

            _userRepository.GetAllAsync(1, 10).Returns(new List<User>());
            _mapper.Map<List<UserDto>>(Arg.Any<List<User>>()).Returns(new List<UserDto>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
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