using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");

            var existingProduct = await _productRepository.GetByTitleAsync(request.ProductDto.Title);
            if (existingProduct != null && existingProduct.Id != request.ProductId)
                throw new DomainException("Product title already exists");

            var rating = new Rating(request.ProductDto.Rating.Rate, request.ProductDto.Rating.Count);

            product.Update(
                request.ProductDto.Title,
                request.ProductDto.Price,
                request.ProductDto.Description,
                request.ProductDto.Category,
                request.ProductDto.Image,
                rating
            );

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProductDto>(product);
        }
    }
}