using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events
{
    public class SaleCancelledEvent : DomainEventBase
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }

        public SaleCancelledEvent(Sale sale)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
        }
    }
}
