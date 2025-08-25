using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Events
{
    public class ProductCreatedEvent : DomainEventBase
    {
        public Guid ProductId { get; }
        public string Title { get; }
        public decimal Price { get; }
        public string Category { get; }

        public ProductCreatedEvent(Product product)
        {
            ProductId = product.Id;
            Title = product.Title;
            Price = product.Price;
            Category = product.Category;
        }
    }

    public class ProductModifiedEvent : DomainEventBase
    {
        public Guid ProductId { get; }

        public ProductModifiedEvent(Guid productId)
        {
            ProductId = productId;
        }
    }

    public class ProductPriceUpdatedEvent : DomainEventBase
    {
        public Guid ProductId { get; }
        public decimal NewPrice { get; }

        public ProductPriceUpdatedEvent(Guid productId, decimal newPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
        }
    }

    public class ProductRatingUpdatedEvent : DomainEventBase
    {
        public Guid ProductId { get; }
        public Rating NewRating { get; }

        public ProductRatingUpdatedEvent(Guid productId, Rating newRating)
        {
            ProductId = productId;
            NewRating = newRating;
        }
    }
}