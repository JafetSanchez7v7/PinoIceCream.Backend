using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventories>>GetAllAsync();
        Task<Inventories> GetByIdAsync(int id);
        Task<Inventories> GetByProductIdAsync(int id);
        Task<IEnumerable<Inventories>> GetByStockFilterAsync(int up, int down);
        Task<Inventories> GetByProductNameAsync(string name);
        Task<Inventories> UpdateProductStockAsync(int id);
        Task<Inventories> AddAsync(Inventories inv);
    }

}
