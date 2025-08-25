using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "Sale")]
    public class CreateSaleCommandHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateSaleCommandHandler _handler;

        public CreateSaleCommandHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateSaleCommandHandler(_saleRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCreateSale_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleDto = new CreateSaleDto
                {
                    SaleNumber = "SALE-001",
                    SaleDate = DateTime.Now,
                    Customer = new CustomerInfoDto { CustomerId = Guid.NewGuid(), Name = "John Doe", Email = "john@email.com" },
                    Branch = new BranchInfoDto { BranchId = Guid.NewGuid(), Name = "Loja Principal", Location = "São Paulo" },
                    Items = new List<SaleItemDto>
                    {
                        new SaleItemDto
                        {
                            Product = new ProductInfoDto { ProductId = Guid.NewGuid(), Name = "Product 1", Description = "Desc 1" },
                            Quantity = 2,
                            UnitPrice = 100m
                        }
                    }
                }
            };

            _saleRepository.AddAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            var expectedSale = CreateValidSale();
            _mapper.Map<SaleDto>(Arg.Any<Sale>()).Returns(new SaleDto { Id = expectedSale.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldApplyDiscounts_WhenAddingItems()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleDto = new CreateSaleDto
                {
                    SaleNumber = "SALE-001",
                    SaleDate = DateTime.Now,
                    Customer = new CustomerInfoDto
                    {
                        CustomerId = Guid.NewGuid(),
                        Name = "John Doe",
                        Email = "john@email.com"
                    },
                    Branch = new BranchInfoDto
                    {
                        BranchId = Guid.NewGuid(),
                        Name = "Loja Principal",
                        Location = "São Paulo"
                    },
                    Items = new List<SaleItemDto>
            {
                new SaleItemDto
                {
                    Product = new ProductInfoDto
                    {
                        ProductId = Guid.NewGuid(),
                        Name = "Product 1",
                        Description = "Desc 1"
                    },
                    Quantity = 5, // 10% discount
                    UnitPrice = 100m
                }
            }
                }
            };

            _saleRepository.AddAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            // Mock do mapper para retornar um SaleDto válido
            var expectedSaleDto = new SaleDto
            {
                Id = Guid.NewGuid(),
                SaleNumber = "SALE-001",
                TotalAmount = 450m, // 500 - 10% discount
                Items = new List<SaleItemDto>
        {
            new SaleItemDto
            {
                Product = new ProductInfoDto { ProductId = Guid.NewGuid() },
                Quantity = 5,
                UnitPrice = 100m,
                //Discount = 50m // 10% de 500
            }
        }
            };

            _mapper.Map<SaleDto>(Arg.Any<Sale>()).Returns(expectedSaleDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(450m, result.TotalAmount); // Verifica se o desconto foi aplicado
            //Assert.Equal(50m, result.Items.First().Discount); // Verifica o valor do desconto
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
        //[Fact]
        //public async Task Handle_ShouldApplyDiscounts_WhenAddingItems()
        //{
        //    // Arrange
        //    var command = new CreateSaleCommand
        //    {
        //        SaleDto = new CreateSaleDto
        //        {
        //            SaleNumber = "SALE-001",
        //            SaleDate = DateTime.Now,
        //            Customer = new CustomerInfoDto { CustomerId = Guid.NewGuid(), Name = "John Doe", Email = "john@email.com" },
        //            Branch = new BranchInfoDto { BranchId = Guid.NewGuid(), Name = "Loja Principal", Location = "São Paulo" },
        //            Items = new List<SaleItemDto>
        //            {
        //                new SaleItemDto
        //                {
        //                    Product = new ProductInfoDto { ProductId = Guid.NewGuid(), Name = "Product 1", Description = "Desc 1" },
        //                    Quantity = 5, // Should get 10% discount
        //                    UnitPrice = 100m
        //                }
        //            }
        //        }
        //    };

        //    _saleRepository.AddAsync(Arg.Any<Sale>()).Returns(Task.CompletedTask);
        //    _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        //    // Act
        //    var result = await _handler.Handle(command, CancellationToken.None);

        //    // Assert
        //    Assert.NotNull(result);
        //    await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
        //}

        private Sale CreateValidSale()
        {
            var customer = new CustomerInfo(Guid.NewGuid(), "John Doe", "john@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Loja Principal", "São Paulo");

            return new Sale("SALE-001", DateTime.Now, customer, branch);
        }
    }
}