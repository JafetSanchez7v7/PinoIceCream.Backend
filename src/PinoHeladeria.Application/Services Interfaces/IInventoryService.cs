using PinoHeladeria.Application.DTOs.InventoryDto;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllAsync();
        Task<InventoryDto> GetByIdAsync(int id);
        Task<InventoryDto> GetByProductIdAsync(int id);
        Task<IEnumerable<InventoryDto>> GetByStockFilterAsync(int filter);
        Task<InventoryDto> GetByProductNameAsync(string name);
    }
}
