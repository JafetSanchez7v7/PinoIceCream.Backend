using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface ICustomersRepository
    {
        Task<IEnumerable<Customers>> GetAllAsync();
        Task<Customers?> GetByIdAsync(int customerId);
        Task<Customers?> GetByNameAsync(string customerName);
        Task<Customers>AddAsync(Customers customer);
        Task<IEnumerable<Customers>> GetActiveCustomersAsync();
        Task<Customers?> GetToUpdate(int customerId);
    }
}
