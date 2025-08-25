using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "Item")]
    public class RemoveItemCommandHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RemoveItemCommandHandler _handler;

        public RemoveItemCommandHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new RemoveItemCommandHandler(_saleRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = new RemoveItemCommand { SaleId = saleId, ProductId = productId };

            var sale = CreateValidSale(saleId, productId);
            _saleRepository.GetByIdAsync(saleId).Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            _mapper.Map<SaleDto>(Arg.Any<Sale>()).Returns(new SaleDto { Id = saleId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(sale.Items); // Should be empty after removal
            await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSaleNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new RemoveItemCommand { SaleId = Guid.NewGuid(), ProductId = productId };
            _saleRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSaleIsCancelled()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var saleId = Guid.NewGuid();
            var sale = CreateValidSale(saleId, productId);
            var command = new RemoveItemCommand { SaleId = sale.Id, ProductId = productId };

            sale.Cancel(); // Cancel the sale
            _saleRepository.GetByIdAsync(sale.Id).Returns(sale);

            // Act & Assert
            Assert.Throws<DomainException>(() => sale.Cancel());
        }

        private Sale CreateValidSale(Guid saleId, Guid productId)
        {
            var customer = new CustomerInfo(Guid.NewGuid(), "John Doe", "john@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Loja Principal", "São Paulo");
            var sale = new Sale(saleId.ToString(), DateTime.Now, customer, branch);

            var product = new ProductInfo(productId, "Product productId", "Description productId");
            sale.AddItem(product, 2, 100m);

            return sale;
        }
    }
}