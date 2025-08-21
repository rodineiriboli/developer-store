using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class CancelSaleCommand : IRequest<SaleDto>
    {
        public Guid SaleId { get; set; }
    }
}