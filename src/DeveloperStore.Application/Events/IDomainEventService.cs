using DeveloperStore.Domain.Events;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Application.Events
{
    public interface IDomainEventService
    {
        Task Publish(DomainEventBase domainEvent);
    }

    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;

        public DomainEventService(ILogger<DomainEventService> logger)
        {
            _logger = logger;
        }

        public Task Publish(DomainEventBase domainEvent)
        {
            // Simula a publicação de eventos (em produção, publicaria para um message broker)
            switch (domainEvent)
            {
                case SaleCreatedEvent saleCreated:
                    _logger.LogInformation("SaleCreatedEvent: Sale {SaleNumber} created with total {TotalAmount}",
                        saleCreated.SaleNumber, saleCreated.TotalAmount);
                    break;

                case SaleModifiedEvent saleModified:
                    _logger.LogInformation("SaleModifiedEvent: Sale {SaleNumber} modified",
                        saleModified.SaleNumber);
                    break;

                case SaleCancelledEvent saleCancelled:
                    _logger.LogInformation("SaleCancelledEvent: Sale {SaleNumber} cancelled",
                        saleCancelled.SaleNumber);
                    break;

                case ItemCancelledEvent itemCancelled:
                    _logger.LogInformation("ItemCancelledEvent: Product {ProductId} removed from sale {SaleId}",
                        itemCancelled.ProductId, itemCancelled.SaleId);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}