using AutoMapper;
using AutoMapper.Configuration.Annotations;
using PinoHeladeria.Application.DTOs.SalesDtos;
using PinoHeladeria.Application.Exceptions;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.Services_Interfaces;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISalesRepository _service;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly ICustomersRepository _customersRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SaleService(ISalesRepository sale, IInventoryRepository inv, IProductRepository prod, ICustomersRepository cus, IMapper map, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = sale;
            _inventoryRepository = inv;
            _productRepository = prod;
            _customersRepository = cus;
            _mapper = map;
        }

        public async Task<SalesDto>AddAsync(CreateSaleDto sales)
        {
            var errors = new List<string>();
            // validar el cliente
            var existingCustomer = await _customersRepository.GetByIdAsync(sales.CustomerId);
            if (existingCustomer == null || !existingCustomer.IsActive)
            {
                errors.Add($"Id del cliente :{sales.CustomerId} ivalido no se encontro o esta inactivo");
                throw new ErrorValidationException(errors);
            }
            //validamos la existencia y actividad de los productos
            var productIdsToBuy = sales.Details.Select(d => d.ProductId).Distinct().ToList();
            var productsInCatalog = await _productRepository.GetWhereAsync(p => productIdsToBuy.Contains(p.ProductId));

            if (productsInCatalog.Count() != productIdsToBuy.Count)
            {
                var foundIds = productsInCatalog.Select(p => p.ProductId);
                var missingIds = productIdsToBuy.Except(foundIds);

                errors.Add($"No puedes realizar la compra: los siguientes IDs de productos no existen en el catálogo: {string.Join(", ", missingIds)}");
                throw new ErrorValidationException(errors);
            }
            foreach (var product in productsInCatalog)
            {
                if (product == null || !product.IsActive)
                {
                    errors.Add($"producto con el id :{product.ProductId}, no existe o se encuentra inactivo");
                    throw new ErrorValidationException(errors);
                }
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var saleEntity = _mapper.Map<Sales>(sales);
                if (saleEntity.SalesDetails != null && saleEntity.SalesDetails.Count > 0)
                {
                    foreach(var detail in saleEntity.SalesDetails)
                    {
                        var inventory
                    }
                }
            }
        }
}
