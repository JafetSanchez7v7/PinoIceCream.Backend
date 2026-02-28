using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using PinoHeladeria.Infrastucture.CacheKeys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly MyAppDbContext _context;
        private readonly IMemoryCache _cache;
        public InventoryRepository(MyAppDbContext con, IMemoryCache cache)
        {
            _cache = cache;
            _context = con;
        }

        public async Task <IEnumerable<Inventories>>GetAllAsync()
        {
            var key = $"{InventoryCacheKeys.InventoryList}";
            if(!_cache.TryGetValue(key, out List<Inventories> cachedInventory))
            {
                cachedInventory = await _context.Inventories.AsNoTracking()
                                            .Include(p => p.Product)
                                            .ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            }
            return cachedInventory;
        }

        public async Task<Inventories> GetByIdAsync(int id)
        {
            var key = $"{InventoryCacheKeys.InventoryByIdKey}{id}";
            if (!_cache.TryGetValue(key, out Inventories cache))
            {
                cache = await _context.Inventories.AsNoTracking()
                                        .Include(p => p.Product)
                                        .FirstOrDefaultAsync(i => i.InventoryId == id);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cache, cacheEntryOptions);
            }

            return cache;

        }

        public async Task<IEnumerable<Inventories>>GetByStockFilterAsync(int up, int down)
        {
            var key = $"{InventoryCacheKeys.InventoryByStockFilter}";
            if (!_cache.TryGetValue(key, out List<Inventories> cachedInventory))
            {
                cachedInventory = await _context.Inventories.AsNoTracking()
                                                       .Include(p => p.Product)
                                                       .Where(i => i.Quantity <= up && i.Quantity >= down)
                                                       .ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cachedInventory, cacheEntryOptions);
            }
            return cachedInventory;


        }

        public async Task<Inventories>GetByProductNameAsync(string name)
        {
            var returned = await _context.Inventories.AsNoTracking()
                                                   .Include(p=> p.Product)
                                                   .FirstOrDefaultAsync(i=> i.Product.ProductName == name);

            return returned;    
        }

        public async Task<Inventories>GetByProductIdAsync(int id)
        {
            var returned = await _context.Inventories.AsNoTracking()
                                                   .Include(p=> p.Product)
                                                   .FirstOrDefaultAsync(i=> i.ProductId == id);
            return returned;
        }
        public async Task<Inventories> UpdateProductStockAsync(int id)
        {
            var returned = await _context.Inventories
                                         .FirstOrDefaultAsync(i => i.ProductId == id);
            return returned;
        }
        public async Task<Inventories> AddAsync(Inventories inv)
        {
            var result = await _context.Inventories.AddAsync(inv);
            return result.Entity;
        }

    }
}
