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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SaleService(ISalesRepository sale, IInventoryRepository inv, IProductRepository prod, ICustomersRepository cus, IMapper map, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = map;
        }

        public async Task<SalesDto> AddAsync(CreateSaleDto sales)
        {
            var errors = new List<string>();
            // validar el cliente
            var existingCustomer = await _unitOfWork.CustomersI.GetByIdAsync(sales.CustomerId);
            if (existingCustomer == null || !existingCustomer.IsActive)
            {
                errors.Add($"Id del cliente :{sales.CustomerId} ivalido no se encontro o esta inactivo");
                throw new ErrorValidationException(errors);
            }
            //validamos la existencia y actividad de los productos
            var productIdsToBuy = sales.SalesDetails.Select(d => d.ProductId).Distinct().ToList();
            var productsInCatalog = await _unitOfWork.ProductsI.GetWhereAsync(p => productIdsToBuy.Contains(p.ProductId));

            if (productsInCatalog.Count() != productIdsToBuy.Count)
            {
                var foundIds = productsInCatalog.Select(p => p.ProductId);
                var missingIds = productIdsToBuy.Except(foundIds);

                errors.Add($"No puedes realizar la venta: los siguientes IDs de productos no existen en el catálogo: {string.Join(", ", missingIds)}");
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
                    foreach (var detail in saleEntity.SalesDetails)
                    {
                        var inventory = await _unitOfWork.InventoryI.UpdateProductStockAsync(detail.ProductId);
                        if(inventory == null)
                        {
                            errors.Add("No existe inventario para este producto");
                            throw new ErrorValidationException(errors);
                        }
                        if (inventory.Quantity < detail.Quantity)
                        {
                            errors.Add($"No hay suficiente stock para el producto con id: {detail.ProductId}. Stock disponible: {inventory.Quantity}, cantidad solicitada: {detail.Quantity}");
                            throw new ErrorValidationException(errors);
                        }
                        else
                        {
                            inventory.Quantity -= detail.Quantity;
                            inventory.UpdatedAt = DateTime.UtcNow;
                        }
                        detail.Total = detail.Quantity * inventory.SalePrice;
                    }
                    saleEntity.SaleTotal = saleEntity.SalesDetails.Sum(d => d.Total);
                    saleEntity.SaleDate = DateTime.UtcNow;
                }
                var createdSale = await _unitOfWork.SalesI.AddAsync(saleEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return _mapper.Map<SalesDto>(createdSale);



            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                if (ex is ErrorValidationException)
                {
                    throw;
                }
                throw new DataBaseException("Ocurrio un error al intentar registrar la venta, por favor intente de nuevo " + ex.Message);

            }

        }
    }
}
