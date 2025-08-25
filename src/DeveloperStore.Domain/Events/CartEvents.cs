using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events
{
    public class CartCreatedEvent : DomainEventBase
    {
        public Guid CartId { get; }
        public int UserId { get; }
        public DateTime Date { get; }

        public CartCreatedEvent(Cart cart)
        {
            CartId = cart.Id;
            UserId = cart.UserId;
            Date = cart.Date;
        }
    }

    public class CartModifiedEvent : DomainEventBase
    {
        public Guid CartId { get; }

        public CartModifiedEvent(Guid cartId)
        {
            CartId = cartId;
        }
    }

    public class CartItemRemovedEvent : DomainEventBase
    {
        public Guid CartId { get; }
        public int ProductId { get; }

        public CartItemRemovedEvent(Guid cartId, int productId)
        {
            CartId = cartId;
            ProductId = productId;
        }
    }

    public class CartClearedEvent : DomainEventBase
    {
        public Guid CartId { get; }

        public CartClearedEvent(Guid cartId)
        {
            CartId = cartId;
        }
    }
}