namespace DeveloperStore.Domain.Events
{
    public class ItemCancelledEvent : DomainEventBase
    {
        public Guid SaleId { get; }
        public int ProductId { get; } // Alterado de Guid para int

        public ItemCancelledEvent(Guid saleId, int productId) // Alterado parâmetro
        {
            SaleId = saleId;
            ProductId = productId;
        }
    }
}