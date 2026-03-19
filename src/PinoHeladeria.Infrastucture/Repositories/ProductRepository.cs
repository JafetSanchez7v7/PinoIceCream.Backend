using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using PinoHeladeria.Infrastucture.CacheKeys;
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
        private readonly IMemoryCache _cache;

        public ProductRepository(MyAppDbContext con, IMemoryCache cache)
        {
            _context = con;
            _cache = cache;
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            var key = $"{ProductCacheKeys.ProductList}";
            if (!_cache.TryGetValue(key, out List<Products> cachedProducts))
            {
                cachedProducts = await _context.Products
                    .AsNoTracking()
                    .Include(c => c.Category)
                    .Include(s => s.Supplier)
                    .ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cachedProducts, cacheEntryOptions);
            }
            return cachedProducts;
        }
        public async Task<Products> GetByIdAsync(int productId)
        {
            var key = $"{ProductCacheKeys.ProductByIdKey}{productId}";
            if(!_cache.TryGetValue(key, out Products cachedId))
            {
                cachedId = await _context.Products.AsNoTracking()
                    . Include(c => c.Category)
                    .Include(s => s.Supplier)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)).
                    SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cachedId, cacheEntryOptions);
            }
            return cachedId;
        }
        public async Task<Products> GetByNameAsync(string productName)
        {
            var key = $"{ProductCacheKeys.ProductByNameKey}{productName}";
            if (!_cache.TryGetValue(key, out Products cachedProduct))
            {
                cachedProduct = await _context.Products
                    .AsNoTracking()
                    .Include(c => c.Category)
                    .Include(s => s.Supplier)
                    .FirstOrDefaultAsync(p => p.ProductName == productName);
                var cacheEntryOptions = new MemoryCacheEntryOptions().
                   SetSlidingExpiration(TimeSpan.FromMinutes(30)).
                   SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cachedProduct, cacheEntryOptions);

            }
            return cachedProduct;
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
