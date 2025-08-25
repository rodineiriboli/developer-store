using DeveloperStore.Application.DTOs;
using MediatR;

namespace DeveloperStore.Application.Commands
{
    public class CreateCartCommand : IRequest<CartDto>
    {
        public CreateCartDto CartDto { get; set; }
    }

    public class UpdateCartCommand : IRequest<CartDto>
    {
        public Guid CartId { get; set; }
        public UpdateCartDto CartDto { get; set; }
    }

    public class DeleteCartCommand : IRequest<CartDto>
    {
        public Guid CartId { get; set; }
    }

    public class AddCartItemCommand : IRequest<CartDto>
    {
        public Guid CartId { get; set; }
        public AddCartItemDto ItemDto { get; set; }
    }

    public class UpdateCartItemCommand : IRequest<CartDto>
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public UpdateCartItemDto ItemDto { get; set; }
    }

    public class RemoveCartItemCommand : IRequest<CartDto>
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
    }

    public class ClearCartCommand : IRequest<CartDto>
    {
        public Guid CartId { get; set; }
    }
}