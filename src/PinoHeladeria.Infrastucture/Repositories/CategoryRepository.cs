using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using PinoHeladeria.Infrastucture.CacheKeys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyAppDbContext _context;
        private readonly IMemoryCache _cache;
       
        public CategoryRepository(MyAppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Categories>>GetAllCategoriesAsync()
        {
            string key = $"{CategoryCacheKeys.CategoryList}";
            if(!_cache.TryGetValue(key, out List<Categories> cachedCategories))
            {
                cachedCategories = await _context.Categories
                    .AsNoTracking()
                    .ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(key, cachedCategories, cacheEntryOptions);
            }
                
                return cachedCategories;

        }
        public async Task<Categories>FindCatAsync(int categoryId)
        {
            var key = $"{CategoryCacheKeys.CategoryByIdKey}{categoryId}";
            if (!_cache.TryGetValue(key, out Categories cachedCategory))
            {
                cachedCategory = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(2));
                _cache.Set(key, cachedCategory, cacheEntryOptions);
            }
            return cachedCategory;
        }

        public async Task<Categories> FindByNameAsync(string categoryName)
        {
            var key = $"{CategoryCacheKeys.CategoryByNameKey}{categoryName}";

            if (!_cache.TryGetValue(key, out Categories category))
            {
                category = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CategoryName == categoryName);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(categoryName, category, cacheEntryOptions);
            }
            return category;
        }

        public async Task<Categories> AddAsync(Categories category)
        {
            var result = await _context.Categories.AddAsync(category);
            _cache.Remove(CategoryCacheKeys.CategoryList);
            _cache.Remove(CategoryCacheKeys.ActiveCategories);
            return result.Entity;

            
        }
        
        public async Task<IEnumerable<Categories>>GetActiveCategoriesAsync()
        {
            if(!_cache.TryGetValue(CategoryCacheKeys.ActiveCategories, out IEnumerable<Categories> cachedActiveCategories))
            {
                cachedActiveCategories = await _context.Categories
                    .AsNoTracking()
                    .Where(c => c.IsActive)
                    .ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(CategoryCacheKeys.ActiveCategories, cachedActiveCategories, cacheEntryOptions);
            }
            return cachedActiveCategories;

        }

        public async Task<Categories>FindAsTrackingAsync(int id)
        {
            var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == id);

            return category;
                
           
        }

        public async Task UpdateAsync( Categories category, string oldName)
        {
            await _context.SaveChangesAsync();
            _cache.Remove(CategoryCacheKeys.CategoryList);
            _cache.Remove($"{CategoryCacheKeys.CategoryByIdKey}{category.CategoryId}");
            _cache.Remove($"{CategoryCacheKeys.CategoryByNameKey}{oldName}");
            _cache.Remove($"{CategoryCacheKeys.CategoryByNameKey}{category.CategoryName}");

        }
    }

}
