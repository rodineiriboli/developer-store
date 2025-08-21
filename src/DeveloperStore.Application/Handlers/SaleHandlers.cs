using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using MediatR;

namespace DeveloperStore.Application.Handlers
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSaleCommandHandler(
            ISaleRepository saleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _saleRepository = saleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var customerInfo = new CustomerInfo(
                request.SaleDto.Customer.CustomerId,
                request.SaleDto.Customer.Name,
                request.SaleDto.Customer.Email);

            var branchInfo = new BranchInfo(
                request.SaleDto.Branch.BranchId,
                request.SaleDto.Branch.Name,
                request.SaleDto.Branch.Location);

            var sale = new Sale(
                request.SaleDto.SaleNumber,
                request.SaleDto.SaleDate,
                customerInfo,
                branchInfo);

            foreach (var itemDto in request.SaleDto.Items)
            {
                var productInfo = new ProductInfo(
                    itemDto.Product.ProductId,
                    itemDto.Product.Name,
                    itemDto.Product.Description);

                sale.AddItem(productInfo, itemDto.Quantity, itemDto.UnitPrice);
            }

            await _saleRepository.AddAsync(sale);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<SaleDto>(sale);
        }
    }
}