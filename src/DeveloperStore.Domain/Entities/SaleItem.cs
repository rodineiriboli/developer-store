using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Domain.Entities
{
    public class SaleItem : EntityBase
    {
        public ProductInfo Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalPrice => (UnitPrice * Quantity) - Discount;

        private SaleItem() { } // For EF

        public SaleItem(ProductInfo product, int quantity, decimal unitPrice)
        {
            Product = product;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = 0;
        }

        public void ApplyDiscount(decimal discount)
        {
            Discount = discount;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity > 20)
                throw new DomainException("Cannot order more than 20 identical items");

            Quantity = quantity;
        }
    }
}
