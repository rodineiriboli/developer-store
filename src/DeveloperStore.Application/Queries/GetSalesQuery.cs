using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Queries
{
    public class GetSaleByIdQuery : IRequest<SaleDto>
    {
        public Guid SaleId { get; set; }
    }
}