using PinoHeladeria.Application.DTOs.SuppliersDto;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface ISuppliersRepository
    {
        Task<IEnumerable<Suppliers>>GetAllAsync();
        Task<Suppliers>GetByIdAsync(int id);
        Task<Suppliers> GetByNameAsync(string name);
        Task<Suppliers> AddAsync(Suppliers supplier);
        Task<IEnumerable<Suppliers>> GetActiveAsync();
    }
}
