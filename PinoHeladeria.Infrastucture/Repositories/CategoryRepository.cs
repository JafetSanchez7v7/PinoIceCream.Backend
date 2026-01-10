using Microsoft.EntityFrameworkCore;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
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
        private readonly  MyAppDbContext _context;
        public CategoryRepository(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categories>>GetAllCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking().ToListAsync();    
        }
        public async Task<Categories>FindCatAsync(int categoryId)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<Categories> FindByNameAsync(string categoryName)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());
        }

        public async Task<Categories> AddAsync(Categories category)
        {
            var result = await _context.Categories.AddAsync(category);
            return result.Entity;
        }
        
        public async Task<IEnumerable<Categories>>GetActiveCategoriesAsync()
        {
            var returnedCategories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return returnedCategories;
        }
    }

}
