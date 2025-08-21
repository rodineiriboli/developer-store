using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events
{
    public class SaleCreatedEvent : DomainEventBase
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }
        public DateTime SaleDate { get; }
        public decimal TotalAmount { get; }

        public SaleCreatedEvent(Sale sale)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
            SaleDate = sale.SaleDate;
            TotalAmount = sale.TotalAmount;
        }
    }
}
