using PinoHeladeria.Application.DTOs.UsersDtos;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<IEnumerable<UserDto>> GetActiveUsersAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetByNameAsync(string name);
        Task<UserDto> AddAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task<UserDto> UpdateStatusAsync(int id, UpdateUserStatusDto dto);
        Task  UpdatePassWordAsync( int id, UpdatePasswordDto dto);
    }

}
