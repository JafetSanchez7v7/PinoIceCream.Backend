using Microsoft.EntityFrameworkCore;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly MyAppDbContext _context;
        public CustomersRepository(MyAppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Customers>> GetAllAsync()
        {
            var returned = await _context.Customers.AsNoTracking()
                                             .OrderBy(c => c.CustomerId).ToListAsync();

            return returned;
        }

        public async Task<Customers?> GetByIdAsync(int customerId)
        {
            var returned = await _context.Customers.AsNoTracking()
                                             .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            return returned;
        }
        public async Task<Customers?> GetByNameAsync(string customerName)
        {
            var returned = await _context.Customers.AsNoTracking()
                                             .FirstOrDefaultAsync(c => c.CustomerName.ToLower() == customerName.ToLower());
            return returned;
        }
        public async Task<Customers> AddAsync(Customers customer)
        {
            var entityEntry = await _context.Customers.AddAsync(customer);
            return entityEntry.Entity;
        }
        public async Task<IEnumerable<Customers>> GetActiveCustomersAsync()
        {
            var returned = await _context.Customers.AsNoTracking()
                                             .Where(c => c.IsActive)
                                             .OrderBy(c => c.CustomerId).ToListAsync();
            return returned;
        }
        public async Task<Customers?> GetToUpdate(int customerId)
        {
            var returned = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            return returned;
        }
    }

}