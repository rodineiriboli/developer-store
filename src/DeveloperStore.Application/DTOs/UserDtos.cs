using DeveloperStore.Domain.Enums;

namespace DeveloperStore.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public NameDto Name { get; set; }
        public AddressDto Address { get; set; }
        public string Phone { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
    }
}