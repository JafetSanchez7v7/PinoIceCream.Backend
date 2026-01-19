using PinoHeladeria.Application.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int productId);
        Task<ProductDto> GetByNameAsync(string productName);
        Task<ProductDto> AddAsync(CreateProductDto dto);
        Task<ProductDto> UpdateAsync(int productId, UpdateProductDto dto);
        Task<ProductDto> UpdateStatusAsync(int productId, UpdateStatusDto dto);
        Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
    }
}
