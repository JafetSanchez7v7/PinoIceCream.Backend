using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Application.DTOs.SuppliersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface ISuppliersService
    {
        Task<IEnumerable<SuppliersDto>> GetAllSuppliersAsync();
        Task<SuppliersDto> GetByIdAsync(int id);
        Task<SuppliersDto> GetByNameAsync(string name);
        Task<SuppliersDto> AddAsync(CreateSupplierDto supplierDto);
        Task<SuppliersDto> UpdateAsync(int id, UpdateSupplierDto supplierDto);
        Task<SuppliersDto> UpdateStatusAsync(int id, UpdateStatusSupDto status);
        Task<IEnumerable<SuppliersDto>> GetActiveAsync();

    }
}
