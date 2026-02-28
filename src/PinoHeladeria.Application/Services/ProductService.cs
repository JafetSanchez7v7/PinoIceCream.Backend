using AutoMapper;
using PinoHeladeria.Application.DTOs.ProductDtos;
using PinoHeladeria.Application.Exceptions;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.Services_Interfaces;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PinoHeladeria.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       
        public ProductService(IMapper mapper,IProductRepository productRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, ISuppliersRepository suppliersRepository)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       public async Task <IEnumerable<ProductDto>>GetAllAsync()
        { 
            var products = await _productRepository.GetAllAsync();
            if (!products.Any())
            {
                throw new NoContentException("No products found.");
            }
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        public async Task<ProductDto> GetByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException($"Product with ID {productId} not found.");
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> GetByNameAsync(string productName)
        {
            var product = await _productRepository.GetByNameAsync(productName);
            if (product == null)
                throw new NotFoundException($"Product with name {productName} not found.");
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto>AddAsync(CreateProductDto dto)
        {
            var errors = new List<string>();
            
            // Validting Category if it exist or its active
            var category = await _unitOfWork.CategoriesI.FindCatAsync(dto.CategoryId);
            if (category == null)
                errors.Add($"Category with Id:{dto.CategoryId} does not exist");
            else if (!category.IsActive)
                errors.Add($"Category with Id:{dto.CategoryId} is not Active");

            // Same as Category
            var supplier = await _unitOfWork.SuppliersI.GetByIdAsync(dto.SupplierId);
            if (supplier == null)
                errors.Add($"Supplier with Id:{dto.SupplierId} does not exist");
            else if (!supplier.IsActive)
                errors.Add($"Supplier with Id:{dto.SupplierId} is not Active");

            if (!errors.Any())
            {
                var existentProduct = await _unitOfWork.ProductsI.GetByNameAsync(dto.ProductName);
                if (existentProduct != null)
                    throw new ConflictException($"Product with name {dto.ProductName} already exists.");
            }

            if (errors.Any())
                throw new ErrorValidationException(errors); 

            //HERE IT IS HAHAHAHA
            var productEntity = _mapper.Map<Products>(dto);
            var addedProduct = await _productRepository.AddAsync(productEntity);       
            await _unitOfWork.SaveChangesAsync();
            //Returning the created product
            //here i do this 'cause i need to return the product with its generated ID with the charged names
            var createdProduct = await _productRepository.GetByIdAsync(addedProduct.ProductId);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task<ProductDto> UpdateAsync(int productId, UpdateProductDto dto)
        {
            var errors = new List<string>();
            //Here we validate if the product exists and we get it to update
            var productToUpdate = await _productRepository.GetToUpdateAsync(productId);
            if (productToUpdate == null)
                throw new NotFoundException($"Product with ID {productId} not found.");
            //Then we validate if the Category and Supplier exist
            // Validting Category if it exist or its active
            var category = await _unitOfWork.CategoriesI.FindCatAsync(dto.CategoryId);
            if (category == null)
                errors.Add($"Category with Id:{dto.CategoryId} does not exist");
            else if (!category.IsActive)
                errors.Add($"Category with Id:{dto.CategoryId} is not Active");

            // Same as Category
            var supplier = await _unitOfWork.SuppliersI.GetByIdAsync(dto.SupplierId);
            if (supplier == null)
                errors.Add($"Supplier with Id:{dto.SupplierId} does not exist");
            else if (!supplier.IsActive)
                errors.Add($"Supplier with Id:{dto.SupplierId} is not Active");
            //Now we proceed to update the product
            if (!errors.Any())
            {
                var existentProduct = await _productRepository.GetByNameAsync(dto.ProductName);
                if (existentProduct != null && existentProduct.ProductId != productId)
                    throw new ConflictException($"Product with name {dto.ProductName} already exists.");
            }
            if (errors.Any())
                throw new ErrorValidationException(errors);

            productToUpdate.ProductName = dto.ProductName;
            productToUpdate.CategoryId = dto.CategoryId;
            productToUpdate.SupplierId = dto.SupplierId;
            productToUpdate.Description = dto.Description;
            productToUpdate.IsActive = dto.IsActive.Value;
            //Here i save the changes and return the updated product
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductDto>(productToUpdate);

        }

        public async Task<ProductDto> UpdateStatusAsync(int productId, UpdateStatusDto dto)
        {
            // Getting the product to update and validating its existence
            var productToUpdate = await _productRepository.GetToUpdateAsync(productId);
            if (productToUpdate == null)
                throw new NotFoundException($"Product with ID {productId} not found.");
            //Validating the update
            if (productToUpdate.IsActive == dto.IsActive)
            {
                var error = new List<string>();
                error.Add($"El estado del producto con id {productId} ya es {(dto.IsActive ? "activo" : "inactivo")}");
                throw new ErrorValidationException(error);
            }
            // Updating the IsActive status
            productToUpdate.IsActive = dto.IsActive;
            //returning the updated product
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductDto>(productToUpdate);
        }

        public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
        {
            var activeProducts = await _productRepository.GetActiveProductsAsync();
            if (activeProducts == null || !activeProducts.Any())
                throw new NoContentException("No active products found.");
            return _mapper.Map<IEnumerable<ProductDto>>(activeProducts);
        }
    }
}
