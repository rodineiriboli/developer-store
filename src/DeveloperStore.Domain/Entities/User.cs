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
                throw new DomainException("Senha é obrigatória");

            if (password.Length < 6)
                throw new DomainException("Senha deve ter pelo menos 6 caracteres");

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
            // workFactor = 12 é um bom balance entre segurança e performance
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
                throw new DomainException("Usuário já está inativo");

            Status = UserStatus.Inactive;
            AddDomainEvent(new UserDeactivatedEvent(Id));
        }

        public void Activate()
        {
            if (Status == UserStatus.Active)
                throw new DomainException("Usuário já está ativo");

            Status = UserStatus.Active;
            AddDomainEvent(new UserActivatedEvent(Id));
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new DomainException("Email é obrigatório");

            if (string.IsNullOrWhiteSpace(Username))
                throw new DomainException("Username é obrigatório");

            if (!Email.Contains("@"))
                throw new DomainException("Formato de email inválido");

            if (Name == null)
                throw new DomainException("Nome é obrigatório");

            if (Address == null)
                throw new DomainException("Endereço é obrigatório");
        }
    }
}