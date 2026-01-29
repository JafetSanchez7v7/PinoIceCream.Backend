using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinoHeladeria.Application.DTOs.RolesDtos;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface IRoleService
    {
         Task<IEnumerable<RoleDto>> GetAllRolesAsync();
         Task<IEnumerable<RoleDto>> GetActiveRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<RoleDto> GetByNameAsync(string name);
        Task<RoleDto> AddAsync(CreateRoleDto dto);
        Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto);
        Task<RoleDto> UpdateStatusAsync(int id, UpdateRoleStatusDto dto);
    }
}
