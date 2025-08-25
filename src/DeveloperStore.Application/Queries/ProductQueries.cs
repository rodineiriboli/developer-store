using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }
    }

    public class GetProductByTitleQuery : IRequest<ProductDto>
    {
        public string Title { get; set; }
    }

    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetProductsByCategoryQuery : IRequest<List<ProductDto>>
    {
        public string Category { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetProductsByPriceRangeQuery : IRequest<List<ProductDto>>
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllCategoriesQuery : IRequest<List<string>>
    {
    }
}