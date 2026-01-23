using PinoHeladeria.Application.DTOs.SalesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface ISaleService
    {
        Task<SalesDto> Addsync(CreateSaleDto sales);
    }
}
