using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface IPurchasesRepository
    {
        Task<Purchases> AddPurchaseAsync(Purchases purchase);
    }
}
