using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities
{
    public class Sale : EntityBase, IAggregateRoot
    {
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public CustomerInfo Customer { get; private set; }
        public decimal TotalAmount { get; private set; }
        public BranchInfo Branch { get; private set; }
        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
        public bool IsCancelled { get; private set; }

        private Sale() { } // For EF

        public Sale(string saleNumber, DateTime saleDate, CustomerInfo customer, BranchInfo branch)
        {
            SaleNumber = saleNumber;
            SaleDate = saleDate;
            Customer = customer;
            Branch = branch;
            TotalAmount = 0;
            IsCancelled = false;

            AddDomainEvent(new SaleCreatedEvent(this));
        }

        public void AddItem(ProductInfo product, int quantity, decimal unitPrice)
        {
            if (IsCancelled)
                throw new DomainException("Cannot add items to a cancelled sale");

            if (quantity > 20)
                throw new DomainException("Cannot order more than 20 identical items");

            var existingItem = _items.FirstOrDefault(i => i.Product.ProductId == product.ProductId);

            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + quantity;
                if (newQuantity > 20)
                    throw new DomainException("Cannot order more than 20 identical items");

                existingItem.UpdateQuantity(newQuantity);
            }
            else
            {
                _items.Add(new SaleItem(product, quantity, unitPrice));
            }

            ApplyDiscounts();
            AddDomainEvent(new SaleModifiedEvent(this));
        }

        public void ApplyDiscounts()
        {
            foreach (var item in _items)
            {
                if (item.Quantity >= 10)
                {
                    item.ApplyDiscount(item.UnitPrice * item.Quantity * 0.20m);
                }
                else if (item.Quantity >= 4)
                {
                    item.ApplyDiscount(item.UnitPrice * item.Quantity * 0.10m);
                }
                else
                {
                    item.ApplyDiscount(0);
                }
            }

            TotalAmount = _items.Sum(i => i.TotalPrice);
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Sale is already cancelled");

            IsCancelled = true;
            AddDomainEvent(new SaleCancelledEvent(this));
        }

        public void RemoveItem(int productId) // Alterado de Guid para int
        {
            var item = _items.FirstOrDefault(i => i.Product.ProductId == productId); // Agora compara int com int
            if (item != null)
            {
                _items.Remove(item);
                ApplyDiscounts();
                AddDomainEvent(new SaleModifiedEvent(this));
                AddDomainEvent(new ItemCancelledEvent(Id, productId)); // Adicionando evento específico de item cancelado
            }
        }

        // Método adicional para obter um item específico
        public SaleItem GetItem(int productId)
        {
            return _items.FirstOrDefault(i => i.Product.ProductId == productId);
        }

        // Método para atualizar a quantidade de um item existente
        public void UpdateItemQuantity(int productId, int newQuantity)
        {
            if (IsCancelled)
                throw new DomainException("Cannot update items in a cancelled sale");

            if (newQuantity > 20)
                throw new DomainException("Cannot order more than 20 identical items");

            var item = _items.FirstOrDefault(i => i.Product.ProductId == productId);
            if (item != null)
            {
                item.UpdateQuantity(newQuantity);
                ApplyDiscounts();
                AddDomainEvent(new SaleModifiedEvent(this));
            }
        }
    }
}