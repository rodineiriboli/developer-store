using DeveloperStore.Application.Queries;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace DeveloperStore.Application.Handlers
{
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetCartByUserIdQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByUserIdAsync(request.UserId);
            if (cart == null)
                throw new KeyNotFoundException($"Cart for user ID {request.UserId} not found");

            return _mapper.Map<CartDto>(cart);
        }
    }
}