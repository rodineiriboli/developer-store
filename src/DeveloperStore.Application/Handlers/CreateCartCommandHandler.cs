using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            if (await _cartRepository.ExistsForUserAsync(request.CartDto.UserId))
                throw new DomainException("User already has a cart");

            var cart = new Cart(request.CartDto.UserId, request.CartDto.Date);

            foreach (var itemDto in request.CartDto.Products)
            {
                cart.AddProduct(itemDto.ProductId, itemDto.Quantity);
            }

            await _cartRepository.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CartDto>(cart);
        }
    }
}