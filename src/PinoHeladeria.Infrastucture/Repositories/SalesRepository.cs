using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly MyAppDbContext _context;
        private readonly IMemoryCache _cache;
        public SalesRepository(MyAppDbContext con, IMemoryCache cache) {
            _cache = cache;
            _context = con;
        }

        public async Task<Sales>AddAsync(Sales sales)
        {
           var result = await _context.Sales.AddAsync(sales);

            return result.Entity;
        }

        public async Task<Sales> FindByIdAsync(int saleId)
        {

            var sale = await _context.Sales.FirstOrDefaultAsync(s=> s.SaleId == saleId);
            return sale;
        }

        public async Task<IEnumerable<Sales>> GetByCustomer(int customerId)
        {
            var sale = await _context.Sales
                .AsNoTracking()
                .Where(s => s.CustomerId == customerId)
               .ToListAsync();


            return sale ;
        }
        public async Task<IEnumerable<Sales>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
           var sales = await _context.Sales
                .AsNoTracking()
                .Where(s=> s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();
            return sales;
        }


    }
}
