using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Queries
{
    public class GetCartByIdQuery : IRequest<CartDto>
    {
        public Guid CartId { get; set; }
    }

    public class GetCartByUserIdQuery : IRequest<CartDto>
    {
        public int UserId { get; set; }
    }

    public class GetAllCartsQuery : IRequest<List<CartDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetCartsByDateRangeQuery : IRequest<List<CartDto>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}