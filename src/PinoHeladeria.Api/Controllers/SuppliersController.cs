using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs.SuppliersDto;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {

        private readonly ISuppliersService _service;
        public SuppliersController(ISuppliersService sup)
        {
            _service = sup;

        }

        [HttpGet]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllSuppliersAsync();
            var meta = new
            {
                detail = "Lista de Proveedores obtenida",
                TotalAmount = response.Count()
            };

            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Proveedores obtenidos con exito", meta);
            return Ok(ApiResponse);
        }

        [HttpGet("ById/{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            var meta = new
            {
                detail = $"Proveedor con id: {response.SupplierId} Encontrado"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Proveedor obtenido con exito", meta);
            return Ok(ApiResponse);
        }
        [HttpGet("ByName/{name}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetByName(string name)
        {
            var response = await _service.GetByNameAsync(name);
            var meta = new
            {
                detail = $"Proveedor con nombre: {response.SuplierName} Encontrado"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Proveedor obtenido con exito", meta);
            return Ok(ApiResponse);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> Create([FromBody] CreateSupplierDto supplierDto)
        {
            var response = await _service.AddAsync(supplierDto);
            var meta = new
            {
                detail = $"Proveedor con id: {response.SupplierId} Creado"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(201, response, "Proveedor creado con exito", meta);
            return CreatedAtAction(nameof(GetById), new { id = response.SupplierId }, ApiResponse);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto supplierDto)
        {
            var response = await _service.UpdateAsync(id, supplierDto);
            var meta = new
            {
                detail = $"Proveedor con id: {response.SupplierId} Actualizado"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Proveedor actualizado con exito", meta);
            return Ok(ApiResponse);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusSupDto status)
        {
            var response = await _service.UpdateStatusAsync(id, status);
            var meta = new
            {
                detail = $"Proveedor con id: {response.SupplierId} Actualizado"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Estado del proveedor actualizado con exito", meta);
            return Ok(ApiResponse);
        }
        [HttpGet("active")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetActive()
        {
            var response = await _service.GetActiveAsync();
            var meta = new
            {
                detail = "Lista de Proveedores activos obtenida",
                TotalAmount = response.Count()
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Proveedores activos obtenidos con exito", meta);
            return Ok(ApiResponse);
        }
    }

}
