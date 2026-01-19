using PinoHeladeria.Application.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface ICustomersService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto> GetByIdAsync(int customerId);
        Task<CustomerDto> GetByNameAsync(string customerName);
        Task<CustomerDto> AddAsync(CreateCustomerDto dto);
        Task<CustomerDto> UpdateAsync(int id,UpdateCustomerDto dto);
        Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync();
        Task<CustomerDto> UpdateStatusAsync(int id, UpdateCustomerStatusDto dto);
    }
}
