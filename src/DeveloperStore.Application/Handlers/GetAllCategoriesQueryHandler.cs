using DeveloperStore.Application.Queries;
using DeveloperStore.Domain.Interfaces;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<string>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllCategoriesQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<string>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllCategoriesAsync();
        }
    }
}