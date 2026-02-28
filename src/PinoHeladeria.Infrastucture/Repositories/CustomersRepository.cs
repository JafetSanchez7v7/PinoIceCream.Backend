using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using PinoHeladeria.Infrastucture.CacheKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly MyAppDbContext _context;
        private readonly IMemoryCache _cache;
        public CustomersRepository(MyAppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache; 
        }
        public async Task<IEnumerable<Customers>> GetAllAsync()
        {
            string key = $"{CustomersCacheKeys.CustomerList}";
            if(!_cache.TryGetValue(key, out List<Customers> cachedCustomers))
            {
                cachedCustomers = await _context.Customers
                    .AsNoTracking()
                    .ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cachedCustomers, cacheEntryOptions);
            }
            return cachedCustomers;
        }

        public async Task<Customers?> GetByIdAsync(int customerId)
        {
            string key = $"{CustomersCacheKeys.CustomerByIdKey}{customerId}";
            if (!_cache.TryGetValue(key, out Customers? cachedCustomer))
            {
                cachedCustomer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(2));
                _cache.Set(key, cachedCustomer, cacheEntryOptions);
            }
            return cachedCustomer;
        }
        public async Task<Customers?> GetByNameAsync(string customerName)
        {
            string key = $"{CustomersCacheKeys.CustomerByName}{customerName}";
            if(!_cache.TryGetValue(key, out Customers? cachedCustomer))
            {
                cachedCustomer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CustomerName == customerName);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(2));
                _cache.Set(key, cachedCustomer, cacheEntryOptions);
            }
            return cachedCustomer;
        }
        public async Task<Customers> AddAsync(Customers customer)
        {
            var entityEntry = await _context.Customers.AddAsync(customer);
            _cache.Remove($"{CustomersCacheKeys.CustomerList}");
            return entityEntry.Entity;
        }
        public async Task<IEnumerable<Customers>> GetActiveCustomersAsync()
        {
            string key = $"{CustomersCacheKeys.ActiveList}";
            if(!_cache.TryGetValue(key, out List<Customers> cachedActiveCustomers))
            {
                cachedActiveCustomers = await _context.Customers
                    .AsNoTracking()
                    .Where(c => c.IsActive)
                    .ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cachedActiveCustomers, cacheEntryOptions);
            }
            return cachedActiveCustomers;
        }
        public async Task<Customers?> GetToUpdate(int customerId)
        {
            var returned = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            return returned;
        }

        public async Task UpdateAsync(Customers customer, string oldName)
        {
            _context.Customers.Update(customer);
            _cache.Remove($"{CustomersCacheKeys.CustomerByIdKey}{customer.CustomerId}");
            _cache.Remove($"{CustomersCacheKeys.CustomerByName}{oldName}");
            _cache.Remove($"{CustomersCacheKeys.CustomerList}");
            _cache.Remove($"{CustomersCacheKeys.ActiveList}");
        }

    }

}