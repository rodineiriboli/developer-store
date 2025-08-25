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
    [Trait("CommandHandler", "Sale")]
    public class CancelSaleCommandHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CancelSaleCommandHandler _handler;

        public CancelSaleCommandHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CancelSaleCommandHandler(_saleRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCancelSale_WhenSaleExists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { SaleId = saleId };

            var sale = CreateValidSale();
            _saleRepository.GetByIdAsync(saleId).Returns(sale);
            _saleRepository.UpdateAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            _mapper.Map<SaleDto>(Arg.Any<Sale>()).Returns(new SaleDto { Id = saleId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(sale.IsCancelled); // Should be cancelled
            await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSaleNotFound()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleId = Guid.NewGuid() };
            _saleRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenSaleAlreadyCancelled()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var command = new CancelSaleCommand { SaleId = saleId };

            var sale = CreateValidSale();
            sale.Cancel(); // Already cancelled
            _saleRepository.GetByIdAsync(saleId).Returns(sale);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() =>
                _handler.Handle(command, CancellationToken.None));
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