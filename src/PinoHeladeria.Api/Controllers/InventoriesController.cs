using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoriesController(IInventoryService inv)
        {
         _inventoryService = inv;   
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetAll()
        {
           var response = await _inventoryService.GetAllAsync();
            var meta = new
            {
            Details="Inventarios retornados Correctamente",
            TotalAmount = $"Cantida de inventarios : {response.Count()}"
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200,response,"Exito", meta);
            return Ok(apiResp);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult>ById(int id)
        {
            var response = await _inventoryService.GetByIdAsync(id);
            var meta = new
            {
                Details = $"Inventario con Id :{id} Retornado Exitosamente"
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Exito", meta);
            return Ok(apiResp);
        }
        [HttpGet("{name}/ ByProductName")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult>ByProductName(string name)
        {
            var response = await _inventoryService.GetByProductNameAsync(name);
            var meta = new
            {
                Details = $" Inventario de producto con nombre {name}"
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Exito", meta);
            return Ok(apiResp);
        }

        [HttpGet("{productId}/ ByProductId")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> ByProductId(int id)
        {
            var response = await _inventoryService.GetByProductIdAsync(id);
            var meta = new
            {
                Details = $"Inventario Del producto con Id :{id} Retornado Exitosamente"
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Exito", meta);
            return Ok(apiResp);
        }


    }

}
