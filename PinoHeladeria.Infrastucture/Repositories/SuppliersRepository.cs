using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class SuppliersRepository : ISuppliersRepository

    {
        private readonly MyAppDbContext _context;
        public SuppliersRepository(MyAppDbContext con) {
            _context = con;
        }

        public async Task<IEnumerable<Suppliers>> GetAllAsync()
        { 
        var SupplierList = await _context.Suppliers.AsNoTracking().ToListAsync();
            return SupplierList;
        }
        public async Task<Suppliers>GetByIdAsync(int id)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SupplierId == id);
            return supplier;
        }

        public async Task<Suppliers> GetByNameAsync(string name)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SuplierName.ToLower() == name.ToLower());
            return supplier;
        }

        public async Task<Suppliers> AddAsync(Suppliers supplier)
        {
            var createdSupplier = await _context.Suppliers.AddAsync(supplier);
            return createdSupplier.Entity;
        }

        public async Task<IEnumerable<Suppliers>> GetActiveAsync()
        {
            var activeSuppliers = await _context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.SuplierName)
                .ToListAsync();
            return activeSuppliers;
        }

    }
}
