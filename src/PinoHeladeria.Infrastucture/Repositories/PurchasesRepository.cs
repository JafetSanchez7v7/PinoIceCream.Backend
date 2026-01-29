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
    public class PurchasesRepository : IPurchasesRepository
    {
        private readonly MyAppDbContext _context;
        public PurchasesRepository(MyAppDbContext context)
        {
            _context = context;
        }
        public async Task<Purchases> AddPurchaseAsync(Purchases purchase)
        {
            var result = await _context.Purchases.AddAsync(purchase);
            return result.Entity;
        }
    }
}
