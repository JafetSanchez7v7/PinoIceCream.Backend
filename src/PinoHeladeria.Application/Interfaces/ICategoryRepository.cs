using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Categories>> GetAllCategoriesAsync();
        Task<Categories>FindCatAsync(int categoryId);
        Task<Categories>FindByNameAsync(string categoryName);
        Task<Categories> AddAsync(Categories category);
        Task<IEnumerable<Categories>> GetActiveCategoriesAsync();
        Task<Categories> FindAsTrackingAsync(int id);

    }
}
