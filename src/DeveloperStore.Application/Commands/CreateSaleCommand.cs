using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class CreateSaleCommand : IRequest<SaleDto>
    {
        public CreateSaleDto SaleDto { get; set; }
    }
}