using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events
{
    public class UserCreatedEvent : DomainEventBase
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Username { get; }

        public UserCreatedEvent(User user)
        {
            UserId = user.Id;
            Email = user.Email;
            Username = user.Username;
        }
    }

    public class UserModifiedEvent : DomainEventBase
    {
        public Guid UserId { get; }

        public UserModifiedEvent(Guid userId)
        {
            UserId = userId;
        }
    }

    public class UserPasswordChangedEvent : DomainEventBase
    {
        public Guid UserId { get; }

        public UserPasswordChangedEvent(Guid userId)
        {
            UserId = userId;
        }
    }

    public class UserDeactivatedEvent : DomainEventBase
    {
        public Guid UserId { get; }

        public UserDeactivatedEvent(Guid userId)
        {
            UserId = userId;
        }
    }

    public class UserActivatedEvent : DomainEventBase
    {
        public Guid UserId { get; }

        public UserActivatedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}