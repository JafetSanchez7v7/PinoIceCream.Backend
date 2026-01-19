using PinoHeladeria.Application.DTOs;
using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> FindCatAsync(int categoryId);
         Task<CategoryDto> FindByNameAsync(string categoryName);
         Task<CategoryDto> AddAsync(CreateCategoryDto category);
         Task<CategoryDto> UpdateAsync(int categoryId, UpdateCategoryDto dto);
         Task<CategoryDto> UpdateStatusAsync(int categoryId, UpdateStatusCatDto dto);

        Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
    }
}
