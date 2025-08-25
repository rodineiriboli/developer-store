using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public CreateProductDto ProductDto { get; set; }
    }

    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }
        public UpdateProductDto ProductDto { get; set; }
    }

    public class DeleteProductCommand : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }
    }

    public class UpdateProductPriceCommand : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateProductRatingCommand : IRequest<ProductDto>
    {
        public Guid ProductId { get; set; }
        public RatingDto Rating { get; set; }
    }
}