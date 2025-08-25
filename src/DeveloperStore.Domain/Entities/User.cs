using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities
{
    public class User : EntityBase, IAggregateRoot
    {
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public Name Name { get; private set; }
        public Address Address { get; private set; }
        public string Phone { get; private set; }
        public UserStatus Status { get; private set; }
        public UserRole Role { get; private set; }

        private User() { } // For EF

        public User(string email, string username, string password, Name name,
                   Address address, string phone, UserStatus status, UserRole role)
        {
            Email = email;
            Username = username;
            Name = name;
            Address = address;
            Phone = phone;
            Status = status;
            Role = role;

            SetPassword(password);
            Validate();
            AddDomainEvent(new UserCreatedEvent(this));
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password is required");

            if (password.Length < 6)
                throw new DomainException("Password must be at least 6 characters long");

            PasswordHash = HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public void Update(string email, string username, Name name, Address address,
                         string phone, UserStatus status, UserRole role)
        {
            Email = email;
            Username = username;
            Name = name;
            Address = address;
            Phone = phone;
            Status = status;
            Role = role;

            Validate();
            AddDomainEvent(new UserModifiedEvent(Id));
        }

        public void ChangePassword(string newPassword)
        {
            SetPassword(newPassword);
            AddDomainEvent(new UserPasswordChangedEvent(Id));
        }

        public void Deactivate()
        {
            if (Status == UserStatus.Inactive)
                throw new DomainException("User is already inactive");

            Status = UserStatus.Inactive;
            AddDomainEvent(new UserDeactivatedEvent(Id));
        }

        public void Activate()
        {
            if (Status == UserStatus.Active)
                throw new DomainException("User is already active");

            Status = UserStatus.Active;
            AddDomainEvent(new UserActivatedEvent(Id));
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new DomainException("Email is required");

            if (string.IsNullOrWhiteSpace(Username))
                throw new DomainException("Username is required");

            if (!Email.Contains("@"))
                throw new DomainException("Invalid email format");

            if (Name == null)
                throw new DomainException("Name is required");

            if (Address == null)
                throw new DomainException("Address is required");
        }
    }
}