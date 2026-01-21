using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs.PurchasesDtos;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchasesService _purchasesService;
        public PurchasesController(IPurchasesService ser)
        {
            _purchasesService = ser;
        }
        [HttpPost]
        public async Task<IActionResult> AddPurchase([FromBody] CreatePurchaseDto dto)
        {
            var result = await _purchasesService.AddAsync(dto);
            var meta = new
            {
               DetailsAmount = result.PurchaseDetails.Count
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200,result,"Compra realizada con exito",meta);
            return CreatedAtAction(nameof(AddPurchase), new { id = result.PurchaseId }, apiResp);
        }
    }
}
