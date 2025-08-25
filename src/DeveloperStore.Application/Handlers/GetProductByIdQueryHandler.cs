using DeveloperStore.Application.Queries;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace DeveloperStore.Application.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");

            return _mapper.Map<ProductDto>(product);
        }
    }
}