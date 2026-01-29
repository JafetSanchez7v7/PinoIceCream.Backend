using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Roles>>GetAllAsync();
        Task<IEnumerable<Roles>> GetActiveRoles();
        Task<Roles> GetByIdAsync(int id);
        Task<Roles> GetByNameAsync(string name);
        Task<Roles>AddAsync(Roles role);
        Task<Roles> GetToUpdate(int id);

        Task<IEnumerable<string>>GetRolesByUserId(int id);

    }
}
