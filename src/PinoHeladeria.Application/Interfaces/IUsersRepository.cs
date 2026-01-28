using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<SsUsers>> GetAllAsync();
        Task<IEnumerable<SsUsers>> GetActiveUsersAsync();
        Task<SsUsers?>GetByIdAsync(int id);
        Task<SsUsers> GetByNameAsync(string name);
        Task<SsUsers> AddAsync(SsUsers users);
        Task<SsUsers> GetToUpdate(int id);

    }
}
