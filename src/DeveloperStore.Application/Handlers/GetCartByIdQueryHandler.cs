using DeveloperStore.Application.Queries;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace DeveloperStore.Application.Handlers
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartByIdQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.CartId);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID {request.CartId} not found");

            return _mapper.Map<CartDto>(cart);
        }
    }
}