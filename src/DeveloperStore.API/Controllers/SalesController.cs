using DeveloperStore.Application.Commands;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<SaleDto>>> GetAllSales(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllSalesQuery { Page = page, PageSize = pageSize };
            var sales = await _mediator.Send(query);
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetSaleById(Guid id)
        {
            var query = new GetSaleByIdQuery { SaleId = id };
            var sale = await _mediator.Send(query);
            return Ok(sale);
        }

        [HttpPost]
        public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleDto createSaleDto)
        {
            var command = new CreateSaleCommand { SaleDto = createSaleDto };
            var sale = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, sale);
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<SaleDto>> CancelSale(Guid id)
        {
            var command = new CancelSaleCommand { SaleId = id };
            var sale = await _mediator.Send(command);
            return Ok(sale);
        }

        [HttpDelete("{saleId}/items/{productId}")]
        public async Task<ActionResult<SaleDto>> RemoveItem(Guid saleId, Guid productId)
        {
            var command = new RemoveItemCommand { SaleId = saleId, ProductId = productId };
            var sale = await _mediator.Send(command);
            return Ok(sale);
        }

        [HttpPut("{saleId}/items/{productId}")]
        public async Task<ActionResult<SaleDto>> UpdateItemQuantity(
            Guid saleId,
            int productId,
            [FromBody] int quantity)
        {
            var command = new UpdateItemQuantityCommand
            {
                SaleId = saleId,
                ProductId = productId,
                Quantity = quantity
            };
            var sale = await _mediator.Send(command);
            return Ok(sale);
        }
    }
}