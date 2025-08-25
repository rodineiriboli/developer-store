using DeveloperStore.Application.Commands;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.API.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartDto>>> GetAllCarts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCartsQuery { Page = page, PageSize = pageSize };
            var carts = await _mediator.Send(query);
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> GetCartById(Guid id)
        {
            var query = new GetCartByIdQuery { CartId = id };
            var cart = await _mediator.Send(query);
            return Ok(cart);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartDto>> GetCartByUserId(int userId)
        {
            var query = new GetCartByUserIdQuery { UserId = userId };
            var cart = await _mediator.Send(query);
            return Ok(cart);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<List<CartDto>>> GetCartsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetCartsByDateRangeQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                Page = page,
                PageSize = pageSize
            };
            var carts = await _mediator.Send(query);
            return Ok(carts);
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCart([FromBody] CreateCartDto createCartDto)
        {
            var command = new CreateCartCommand { CartDto = createCartDto };
            var cart = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CartDto>> UpdateCart(Guid id, [FromBody] UpdateCartDto updateCartDto)
        {
            var command = new UpdateCartCommand { CartId = id, CartDto = updateCartDto };
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CartDto>> DeleteCart(Guid id)
        {
            var command = new DeleteCartCommand { CartId = id };
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpPost("{id}/items")]
        public async Task<ActionResult<CartDto>> AddCartItem(Guid id, [FromBody] AddCartItemDto addItemDto)
        {
            var command = new AddCartItemCommand { CartId = id, ItemDto = addItemDto };
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpPut("{id}/items/{productId}")]
        public async Task<ActionResult<CartDto>> UpdateCartItem(Guid id, int productId, [FromBody] UpdateCartItemDto updateItemDto)
        {
            var command = new UpdateCartItemCommand
            {
                CartId = id,
                ProductId = productId,
                ItemDto = updateItemDto
            };
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpDelete("{id}/items/{productId}")]
        public async Task<ActionResult<CartDto>> RemoveCartItem(Guid id, int productId)
        {
            var command = new RemoveCartItemCommand { CartId = id, ProductId = productId };
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpPost("{id}/clear")]
        public async Task<ActionResult<CartDto>> ClearCart(Guid id)
        {
            var command = new ClearCartCommand { CartId = id };
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }
    }
}