using Microsoft.EntityFrameworkCore;
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
    public class UsersRepository : IUsersRepository
    {
        private readonly MyAppDbContext _context;
        public UsersRepository(MyAppDbContext con)
        {
            _context = con;
        }

        public async Task<IEnumerable<SsUsers>> GetAllAsync()
        {
            return await _context.SsUsers.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SsUsers>>GetActiveUsersAsync()
        {
            return await _context.SsUsers.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }
        public async Task<SsUsers?>GetByIdAsync(int id)
        {
            return await _context.SsUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task<SsUsers> AddAsync(SsUsers user)
        {
            var returned = await _context.SsUsers.AddAsync(user);
            return returned.Entity;
        }
        public async Task<SsUsers> GetByNameAsync(string name)
        {
            return await _context.SsUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserName.ToLower() == name.ToLower());
        }

        public async  Task<SsUsers>GetToUpdate(int id)
        {
            return await _context.SsUsers.FirstOrDefaultAsync(x => x.UserId == id);
        }
    }
}
