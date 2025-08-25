using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RemoveCartItemCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.CartId);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID {request.CartId} not found");

            cart.RemoveProduct(request.ProductId);

            await _cartRepository.UpdateAsync(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CartDto>(cart);
        }
    }
}