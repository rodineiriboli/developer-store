namespace DeveloperStore.Domain.Events
{
    public class ItemCancelledEvent : DomainEventBase
    {
        public Guid SaleId { get; }
        public Guid ProductId { get; } // Alterado de Guid para int

        public ItemCancelledEvent(Guid saleId, Guid productId) // Alterado parâmetro
        {
            SaleId = saleId;
            ProductId = productId;
        }
    }
}