using Microsoft.EntityFrameworkCore;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyAppDbContext _context;
        public ProductRepository(MyAppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            var returned = await _context.Products.
                AsNoTracking()
                .Include(c=> c.Category).
                Include(s => s.Supplier)
                .ToListAsync();
            return returned;
        }
        public async Task<Products> GetByIdAsync(int productId)
        {
            var returned = await _context.Products.
                AsNoTracking().
                Include(c=> c.Category).
                Include(s => s.Supplier).
                FirstOrDefaultAsync(p => p.ProductId == productId);
            return returned;
        }
        public async Task<Products> GetByNameAsync(string productName)
        {
            var returned = await _context.Products.
                AsNoTracking().
                Include(c => c.Category).
                Include(s => s.Supplier).
                FirstOrDefaultAsync(p => p.ProductName == productName);
            return returned;
        }
        public async Task<Products> AddAsync(Products product)
        {
            var result = await _context.Products.AddAsync(product);
            return result.Entity;
        }

        public async Task<IEnumerable<Products>> GetActiveProductsAsync()
        {
            return await _context.Products.
                AsNoTracking().
                Include(c => c.Category).
                Include(s => s.Supplier).
                Where(p => p.IsActive).ToListAsync();
        }
        public async Task<Products> GetToUpdateAsync(int id)
        {
            var returned = await _context.Products.
                Include(c => c.Category).
                Include(s => s.Supplier).
                FirstOrDefaultAsync(p => p.ProductId == id);
            return returned;
        }
        public async Task<IEnumerable<Products>> GetWhereAsync(Expression<Func<Products, bool>> predicate)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(s => s.Supplier)
                .Where(predicate) // Ahora EF sí entiende cómo traducir esto a SQL
                .ToListAsync();   // Ahora sí te dejará usar el Async
        }

        public async Task<IEnumerable<Products>> GetActiveProductsByIdsAsync(List<int> productIds)
        {
            return await _context.Products.Where(p => p.IsActive && productIds.Contains(p.ProductId)).ToListAsync();

        }

    }

}
