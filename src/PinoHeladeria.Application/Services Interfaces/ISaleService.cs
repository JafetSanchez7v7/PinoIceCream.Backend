using PinoHeladeria.Application.DTOs.SalesDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface ISaleService
    {
        Task<SalesDto> AddAsync(CreateSaleDto sales);
       // Task<SalesDto> FindByIdAsync(int saleId);
       // Task<IEnumerable<SalesDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
       // Task<IEnumerable<SalesDto>> GetByCustomerAsync(int customerId);
    }
}
