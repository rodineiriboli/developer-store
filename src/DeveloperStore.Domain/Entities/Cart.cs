using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;

namespace DeveloperStore.Domain.Entities
{
    public class Cart : EntityBase, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public DateTime Date { get; private set; }
        private readonly List<CartItem> _products = new();
        public IReadOnlyCollection<CartItem> Products => _products.AsReadOnly();

        private Cart() { } // For EF

        public Cart(Guid userId, DateTime date)
        {
            UserId = userId;
            Date = date;

            AddDomainEvent(new CartCreatedEvent(this));
        }

        public void AddProduct(Guid productId, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            var existingItem = _products.FirstOrDefault(p => p.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                _products.Add(new CartItem(productId, quantity));
            }

            AddDomainEvent(new CartModifiedEvent(Id));
        }

        public void UpdateProductQuantity(Guid productId, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            var item = _products.FirstOrDefault(p => p.ProductId == productId);
            if (item == null)
                throw new DomainException($"Product with ID {productId} not found in cart");

            item.UpdateQuantity(quantity);
            AddDomainEvent(new CartModifiedEvent(Id));
        }

        public void RemoveProduct(Guid productId)
        {
            var item = _products.FirstOrDefault(p => p.ProductId == productId);
            if (item != null)
            {
                _products.Remove(item);
                AddDomainEvent(new CartModifiedEvent(Id));
                AddDomainEvent(new CartItemRemovedEvent(Id, productId));
            }
        }

        public void ClearCart()
        {
            _products.Clear();
            AddDomainEvent(new CartModifiedEvent(Id));
        }

        public bool ContainsProduct(Guid productId)
        {
            return _products.Any(p => p.ProductId == productId);
        }

        public int GetProductQuantity(Guid productId)
        {
            return _products.FirstOrDefault(p => p.ProductId == productId)?.Quantity ?? 0;
        }
    }

    public class CartItem : EntityBase
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        private CartItem() { } // For EF

        public CartItem(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            Quantity = quantity;
        }
    }
}