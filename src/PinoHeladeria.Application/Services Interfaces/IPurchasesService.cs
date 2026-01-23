using PinoHeladeria.Application.DTOs.PurchasesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services_Interfaces
{
    public interface IPurchasesService
    {
        Task<PurchaseDto> AddAsync(CreatePurchaseDto createPurchaseDto);
    }
}
