using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Queries
{
    public class GetSalesByCustomerQuery : IRequest<List<SaleDto>>
    {
        public int CustomerId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}