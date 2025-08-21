using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class RemoveItemCommand : IRequest<SaleDto>
    {
        public Guid SaleId { get; set; }
        public int ProductId { get; set; }
    }
}