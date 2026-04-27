using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface ISalesRepository
    {
        Task<Sales>AddAsync(Sales sales);
        Task<Sales> FindByIdAsync(int saleId);
       Task<IEnumerable<Sales>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Sales>> GetByCustomerAsync(int customerId);

    }
}
