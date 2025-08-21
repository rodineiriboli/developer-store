using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class UpdateSaleCommand : IRequest<SaleDto>
    {
        public Guid SaleId { get; set; }
        public CreateSaleDto SaleDto { get; set; }
    }
}