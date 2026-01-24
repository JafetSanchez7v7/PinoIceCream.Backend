using AutoMapper;
using AutoMapper.Configuration.Annotations;
using PinoHeladeria.Application.DTOs.PurchasesDtos;
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
    public class PurchasesService : IPurchasesService
    {
        private readonly IPurchasesRepository _purchasesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapperService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISuppliersRepository _supplierRepository;
        public PurchasesService(IProductRepository p, IPurchasesRepository pu, IMapper map, IUnitOfWork un, IInventoryRepository inv, ISuppliersRepository sup)
        {
            _purchasesRepository = pu;
            _unitOfWork = un;
            _mapperService = map;
            _inventoryRepository = inv;
            _productRepository = p;
            _supplierRepository = sup;
        }

        public async Task<PurchaseDto> AddAsync(CreatePurchaseDto dto)
        {
            var errors = new List<string>();
            //Validacion de proveedor
            var existingSup = await _supplierRepository.GetByIdAsync(dto.SupplierId);
            if (existingSup == null )
            {
                errors.Add("Id del proveedor invalido, no existe o no se encontro");
                throw new ErrorValidationException(errors);
            }
            if(!existingSup.IsActive)
            {
                errors.Add($" no se puede asignar la compra al proveedor con id {existingSup.SupplierId} ya que esta inactivo");
                throw new ErrorValidationException(errors);
            }
            
            //aqui validamos la existencia de los productos
            var productIdsToBuy = dto.PurchaseDetails.Select(d => d.ProductId).Distinct().ToList();
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
                    errors.Add($"Producto con id: {product.ProductId} está inactivo o no existe.");
                    throw new ErrorValidationException(errors);
                }
                // si estan activos validamos que si su proveedor esta correcto
                if(product.SupplierId != dto.SupplierId)
                {
                    errors.Add($"el proveedor del producto con id: {product.ProductId} no coincide con el proveedor registrado, si cambiara de proveedor por favor actualize el catalogo");
                    throw new ErrorValidationException(errors);
                }
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var purchase = _mapperService.Map<Purchases>(dto);
                if (purchase.PurchaseDetails != null && purchase.PurchaseDetails.Count > 0)
                {
                    foreach (var detail in purchase.PurchaseDetails)
                    {
                        
                       
                        //Logica de inventario
                        var inventory = await _inventoryRepository.UpdateProductStockAsync(detail.ProductId);
                        if (inventory == null)
                        {

                            inventory = new Inventories
                            {
                                ProductId = detail.ProductId,
                                Quantity = detail.Quantity,
                                PurchasePrice = detail.PurchasePrice,
                                SalePrice = detail.PurchasePrice * 1.10m,
                                UpdatedAt = DateTime.UtcNow
                            };
                            await _inventoryRepository.AddAsync(inventory);
                        }
                        else
                        {
                            inventory.Quantity += detail.Quantity;
                            inventory.PurchasePrice = detail.PurchasePrice;
                            inventory.SalePrice = detail.PurchasePrice * 1.10m;
                            inventory.UpdatedAt = DateTime.UtcNow;
                        }
                        detail.Total = detail.Quantity * detail.PurchasePrice;
                    }
                }
                //calculo total y retorno
                 purchase.PurchaseTotal = purchase.PurchaseDetails.Sum(d => d.Total);
                purchase.PurchaseDate = DateTime.UtcNow;
                var addedPurchase = await _purchasesRepository.AddPurchaseAsync(purchase);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return _mapperService.Map<PurchaseDto>(addedPurchase);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                if(ex is ErrorValidationException)
                {
                    throw;
                }
                throw new DataBaseException("Ocurrio un error al intentar registrar la compra, por favor intente de nuevo " + ex.Message);
            }


        }
    }
}
