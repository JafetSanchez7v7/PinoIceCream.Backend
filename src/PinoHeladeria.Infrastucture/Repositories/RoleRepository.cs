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
    public class RoleRepository : IRoleRepository
    {
        private readonly MyAppDbContext _context;
        public RoleRepository(MyAppDbContext myAppDbContext     )
        {
            _context = myAppDbContext;
        }

        public async Task<IEnumerable<Roles>>GetAllAsync()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Roles>> GetActiveRoles()
        {
            return await _context.Roles.AsNoTracking().Where(r => r.IsActive).ToListAsync();
        }

        public async Task<Roles> GetByIdAsync(int id)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r=> r.RoleId == id);
        }   
        
        public async Task<Roles> GetByNameAsync(string name)
        {
            return await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleName == name);
        }

        public async Task<Roles>AddAsync(Roles roles)
        {
            var result = await _context.Roles.AddAsync(roles);

            return result.Entity;
        }

        public async Task<Roles> GetToUpdate(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<IEnumerable<string>> GetRolesByUserId(int id)
        {
            
            var roles = await _context.Database
                .SqlQuery<string>($"EXEC dbo.USP_GetUserRolesByUser @UserId = {id}")
                .ToListAsync();
            return roles;
        }
    }
}
