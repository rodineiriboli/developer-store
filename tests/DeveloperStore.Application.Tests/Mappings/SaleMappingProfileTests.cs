using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Application.Tests.Mappings
{
    [Trait("Mapping", "Sale")]
    public class SaleMappingProfileTests
    {
        private readonly IMapper _mapper;

        public SaleMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SaleMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapSaleToSaleDto()
        {
            // Arrange
            var customer = new CustomerInfo(Guid.NewGuid(), "John Doe", "john@email.com");
            var branch = new BranchInfo(Guid.NewGuid(), "Loja Principal", "São Paulo");
            var sale = new Sale("SALE-001", DateTime.Now, customer, branch);

            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description 1");
            sale.AddItem(product, 2, 100m);

            // Act
            var saleDto = _mapper.Map<SaleDto>(sale);

            // Assert
            Assert.Equal(sale.SaleNumber, saleDto.SaleNumber);
            Assert.Equal(sale.SaleDate, saleDto.SaleDate);
            Assert.Equal(sale.TotalAmount, saleDto.TotalAmount);
            Assert.Equal(sale.IsCancelled, saleDto.IsCancelled);
            Assert.Equal(sale.Items.Count, saleDto.Items.Count);
        }

        [Fact]
        public void ShouldMapSaleItemToSaleItemDto()
        {
            // Arrange
            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description 1");
            var saleItem = new SaleItem(product, 2, 100m);
            saleItem.ApplyDiscount(20m);

            // Act
            var saleItemDto = _mapper.Map<SaleItemDto>(saleItem);

            // Assert
            Assert.Equal(saleItem.Quantity, saleItemDto.Quantity);
            Assert.Equal(saleItem.UnitPrice, saleItemDto.UnitPrice);
            //Assert.Equal(saleItem.Discount, saleItemDto.Discount);
            Assert.Equal(saleItem.Product.ProductId, saleItemDto.Product.ProductId);
        }

        [Fact]
        public void ShouldMapCustomerInfoToCustomerInfoDto()
        {
            // Arrange
            var customerInfo = new CustomerInfo(Guid.NewGuid(), "John Doe", "john@email.com");

            // Act
            var customerInfoDto = _mapper.Map<CustomerInfoDto>(customerInfo);

            // Assert
            Assert.Equal(customerInfo.CustomerId, customerInfoDto.CustomerId);
            Assert.Equal(customerInfo.Name, customerInfoDto.Name);
            Assert.Equal(customerInfo.Email, customerInfoDto.Email);
        }

        [Fact]
        public void ShouldMapBranchInfoToBranchInfoDto()
        {
            // Arrange
            var branchInfo = new BranchInfo(Guid.NewGuid(), "Loja Principal", "São Paulo");

            // Act
            var branchInfoDto = _mapper.Map<BranchInfoDto>(branchInfo);

            // Assert
            Assert.Equal(branchInfo.BranchId, branchInfoDto.BranchId);
            Assert.Equal(branchInfo.Name, branchInfoDto.Name);
            Assert.Equal(branchInfo.Location, branchInfoDto.Location);
        }

        [Fact]
        public void ShouldMapSaleItemToSaleItemDto_WithDiscount()
        {
            // Arrange
            var product = new ProductInfo(Guid.NewGuid(), "Product 1", "Description 1");
            var saleItem = new SaleItem(product, 5, 100m);
            saleItem.ApplyDiscount(50m); // Aplica desconto

            // Act
            var saleItemDto = _mapper.Map<SaleItemDto>(saleItem);

            // Assert
            Assert.Equal(50m, saleItemDto.Discount);
            Assert.Equal(450m, saleItemDto.TotalPrice); // (5 * 100) - 50
        }
    }
}