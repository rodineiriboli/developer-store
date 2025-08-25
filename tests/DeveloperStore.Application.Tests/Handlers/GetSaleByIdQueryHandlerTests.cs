using DeveloperStore.Application.Queries;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;
using AutoMapper;
using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "Sale")]
    public class GetSaleByIdQueryHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSaleByIdQueryHandler _handler;

        public GetSaleByIdQueryHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSaleByIdQueryHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnSale_WhenSaleExists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var query = new GetSaleByIdQuery { SaleId = saleId };

            var sale = CreateValidSale();
            _saleRepository.GetByIdAsync(saleId).Returns(sale);
            _mapper.Map<SaleDto>(sale).Returns(new SaleDto { Id = saleId });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(saleId, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSaleNotFound()
        {
            // Arrange
            var query = new GetSaleByIdQuery { SaleId = Guid.NewGuid() };
            _saleRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        private Sale CreateValidSale()
        {
            var customer = new CustomerInfo(Guid.NewGuid(), "John Doe", "john@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Loja Principal", "São Paulo");
            var sale = new Sale("SALE-001", DateTime.Now, customer, branch);

            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description 1");
            sale.AddItem(product, 2, 100m);

            return sale;
        }
    }
}