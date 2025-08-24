using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Enums;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using System.Net;
using System.Xml.Linq;

namespace DeveloperStore.Domain.Entities
{
    public class User : EntityBase, IAggregateRoot
    {
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
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
            Password = password;
            Name = name;
            Address = address;
            Phone = phone;
            Status = status;
            Role = role;

            Validate();
            AddDomainEvent(new UserCreatedEvent(this));
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
            Password = newPassword;
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