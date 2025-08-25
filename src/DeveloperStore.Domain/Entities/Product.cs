using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities
{
    public class Product : EntityBase, IAggregateRoot
    {
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string Image { get; private set; }
        public Rating Rating { get; private set; }

        private Product() { } // For EF

        public Product(string title, decimal price, string description, string category, string image, Rating rating)
        {
            Title = title;
            Price = price;
            Description = description;
            Category = category;
            Image = image;
            Rating = rating;

            Validate();
            AddDomainEvent(new ProductCreatedEvent(this));
        }

        public void Update(string title, decimal price, string description, string category, string image, Rating rating)
        {
            Title = title;
            Price = price;
            Description = description;
            Category = category;
            Image = image;
            Rating = rating;

            Validate();
            AddDomainEvent(new ProductModifiedEvent(Id));
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new DomainException("Price must be greater than zero");

            Price = newPrice;
            AddDomainEvent(new ProductPriceUpdatedEvent(Id, newPrice));
        }

        public void UpdateRating(Rating newRating)
        {
            Rating = newRating;
            AddDomainEvent(new ProductRatingUpdatedEvent(Id, newRating));
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new DomainException("Title is required");

            if (Price <= 0)
                throw new DomainException("Price must be greater than zero");

            if (string.IsNullOrWhiteSpace(Description))
                throw new DomainException("Description is required");

            if (string.IsNullOrWhiteSpace(Category))
                throw new DomainException("Category is required");

            if (Rating == null)
                throw new DomainException("Rating is required");
        }
    }
}