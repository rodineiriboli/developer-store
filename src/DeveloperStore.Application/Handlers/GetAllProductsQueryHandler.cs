using DeveloperStore.Application.Queries;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace DeveloperStore.Application.Handlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(request.Page, request.PageSize);
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}