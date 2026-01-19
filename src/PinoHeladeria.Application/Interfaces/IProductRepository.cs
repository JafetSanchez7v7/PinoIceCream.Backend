using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products> GetByIdAsync(int productId);
        Task<Products> GetByNameAsync(string productName);
        Task<Products> AddAsync(Products product);
        Task<IEnumerable<Products>> GetActiveProductsAsync();
        Task<Products> GetToUpdateAsync( int id);
    }
}
