using DeveloperStore.Application.Commands;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllProductsQuery { Page = page, PageSize = pageSize };
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var query = new GetProductByIdQuery { ProductId = id };
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<ProductDto>> GetProductByTitle(string title)
        {
            var query = new GetProductByTitleQuery { Title = title };
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCategory(
            string category,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetProductsByCategoryQuery { Category = category, Page = page, PageSize = pageSize };
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpGet("price-range")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetProductsByPriceRangeQuery
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Page = page,
                PageSize = pageSize
            };
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<string>>> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            var command = new CreateProductCommand { ProductDto = createProductDto };
            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            var command = new UpdateProductCommand { ProductId = id, ProductDto = updateProductDto };
            var product = await _mediator.Send(command);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDto>> DeleteProduct(Guid id)
        {
            var command = new DeleteProductCommand { ProductId = id };
            var product = await _mediator.Send(command);
            return Ok(product);
        }

        [HttpPatch("{id}/price")]
        public async Task<ActionResult<ProductDto>> UpdateProductPrice(Guid id, [FromBody] UpdateProductPriceDto priceDto)
        {
            var command = new UpdateProductPriceCommand { ProductId = id, Price = priceDto.Price };
            var product = await _mediator.Send(command);
            return Ok(product);
        }

        [HttpPatch("{id}/rating")]
        public async Task<ActionResult<ProductDto>> UpdateProductRating(Guid id, [FromBody] UpdateProductRatingDto ratingDto)
        {
            var command = new UpdateProductRatingCommand
            {
                ProductId = id,
                Rating = new RatingDto { Rate = ratingDto.Rate, Count = ratingDto.Count }
            };
            var product = await _mediator.Send(command);
            return Ok(product);
        }
    }
}