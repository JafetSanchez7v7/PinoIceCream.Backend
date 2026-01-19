using Microsoft.EntityFrameworkCore;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.AppDbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly MyAppDbContext _context;
        public InventoryRepository(MyAppDbContext con)
        {
            _context = con;
        }

        public async Task <IEnumerable<Inventories>>GetAllAsync()
        {
            var returned = await _context.Inventory
                                    .AsNoTracking().
                                    Include(p => p.Product)
                                    .ToListAsync();
            return returned;    
        }

        public async Task<Inventories>GetByIdAsync(int id)
        {
            var returned = await _context.Inventory.
                                AsNoTracking().
                                Include(p => p.Product)
                                .FirstOrDefaultAsync(i=> i .InventoryId == id);
            return returned;                     
        }

        public async Task<IEnumerable<Inventories>>GetByStockFilterAsync(int filter)
        {
           var list = await _context.Inventory.AsNoTracking().
                                                Include(p => p.Product).
                                                Where(i=> i.Quantity > filter).
                                                ToListAsync();
            return list;
        }

        public async Task<Inventories>GetByProductNameAsync(string name)
        {
            var returned = await _context.Inventory.AsNoTracking()
                                                   .Include(p=> p.Product)
                                                   .FirstOrDefaultAsync(i=> i.Product.ProductName == name);

            return returned;    
        }

        public async Task<Inventories>GetByProductIdAsync(int id)
        {
            var returned = await _context.Inventory.AsNoTracking()
                                                   .Include(p=> p.Product)
                                                   .FirstOrDefaultAsync(i=> i.ProductId == id);
            return returned;
        }

    }
}
