using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Application.Tests.Mappings
{
    public class UserMappingProfileTests
    {
        private readonly IMapper _mapper;

        public UserMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapUserToUserDto()
        {
            // Arrange
            var geoLocation = new GeoLocation("40.7128", "-74.0060");
            var address = new Address("New York", "Broadway", 123, "10001", geoLocation);
            var name = new Name("John", "Doe");

            var user = new User(
                "john.doe@email.com",
                "johndoe",
                "Password123",
                name,
                address,
                "+1234567890",
                UserStatus.Active,
                UserRole.Customer
            );

            // Act
            var userDto = _mapper.Map<UserDto>(user);

            // Assert
            Assert.Equal(user.Email, userDto.Email);
            Assert.Equal(user.Username, userDto.Username);
            Assert.Equal(user.Phone, userDto.Phone);
            Assert.Equal(user.Status, userDto.Status);
            Assert.Equal(user.Role, userDto.Role);
            Assert.Equal(user.Name.FirstName, userDto.Name.FirstName);
            Assert.Equal(user.Name.LastName, userDto.Name.LastName);
        }

        [Fact]
        public void ShouldMapNameToNameDto()
        {
            // Arrange
            var name = new Name("John", "Doe");

            // Act
            var nameDto = _mapper.Map<NameDto>(name);

            // Assert
            Assert.Equal(name.FirstName, nameDto.FirstName);
            Assert.Equal(name.LastName, nameDto.LastName);
        }

        [Fact]
        public void ShouldMapAddressToAddressDto()
        {
            // Arrange
            var geoLocation = new GeoLocation("40.7128", "-74.0060");
            var address = new Address("New York", "Broadway", 123, "10001", geoLocation);

            // Act
            var addressDto = _mapper.Map<AddressDto>(address);

            // Assert
            Assert.Equal(address.City, addressDto.City);
            Assert.Equal(address.Street, addressDto.Street);
            Assert.Equal(address.Number, addressDto.Number);
            Assert.Equal(address.ZipCode, addressDto.ZipCode);
            Assert.Equal(address.GeoLocation.Lat, addressDto.GeoLocation.Lat);
            Assert.Equal(address.GeoLocation.Long, addressDto.GeoLocation.Long);
        }
    }
}