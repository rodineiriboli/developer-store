using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events
{
    public class SaleModifiedEvent : DomainEventBase
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; }

        public SaleModifiedEvent(Sale sale)
        {
            SaleId = sale.Id;
            SaleNumber = sale.SaleNumber;
        }
    }
}
