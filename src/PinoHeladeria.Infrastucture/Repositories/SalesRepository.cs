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
        public SalesRepository(MyAppDbContext con) {
        _context = con;
        }

        public async Task<Sales>AddAsync(Sales sales)
        {
           var result = await _context.Sales.AddAsync(sales);

            return result.Entity;
        }
    }
}
