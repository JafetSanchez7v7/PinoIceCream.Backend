using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs.SalesDtos;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _salesService;
        public SalesController(ISaleService salesService)
        {
            _salesService = salesService;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSaleDto sale)
        {
            var result = await _salesService.AddAsync(sale);
            var meta = new
            {
               DetailsAmount = result.SalesDetails.Count
            };
            var ApiResp = HelpersOfAppResp.ApiResponseMaker.Create(201, result, "Sale Created", meta);
            return CreatedAtAction(nameof(GetById), new { id = result.SaleId }, ApiResp);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
           // var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, result, "Sale retrieved successfully");
            return Ok();
        }
    }
}
