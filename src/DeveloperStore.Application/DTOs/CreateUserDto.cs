using DeveloperStore.Domain.Enums;

namespace DeveloperStore.Application.DTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Senha em plain text (será hasheada)
        public NameDto Name { get; set; }
        public AddressDto Address { get; set; }
        public string Phone { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
    }
}