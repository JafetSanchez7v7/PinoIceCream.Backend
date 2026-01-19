using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs.CustomerDto;
using PinoHeladeria.Application.DTOs.ProductDtos;
using PinoHeladeria.Application.Services;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersControllers : ControllerBase
    {
        private readonly ICustomersService _service;
        public CustomersControllers(ICustomersService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var response = await _service.GetAllAsync();
            var meta = new
            {
                Details = "Lista de Clientes Obtenida",
                TotalAmount = response.Count()
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Clientes Obtenidos Exitosamente", meta);
            return Ok(apiResponse);
        }

        [HttpGet("{id}/byId")]
        public async Task<IActionResult>GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);
            var meta = new
            {
                Details = $"Cliente con id: {response.CustomerId} Encontrado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Cliente Obtenido Exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpGet("{name}/byName")]
        public async Task<IActionResult>GetByName(string name)
        {
            var response = await _service.GetByNameAsync(name);
            var meta = new
            {
                Details = $"Cliente con nombre: {response.CustomerName} Encontrado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Cliente Obtenido Exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateCustomerDto dto)
        {
            var response = await _service.AddAsync(dto);
            var meta = new
            {
                Details = $"Cliente con id: {response.CustomerId} Creado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(201, response, "Cliente Creado Exitosamente", meta);
            return CreatedAtAction(nameof(GetById), new { id = response.CustomerId }, apiResponse);
        }
        [HttpPut]
        public async Task<IActionResult>Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            var response = await _service.UpdateAsync(id, dto);
            var meta = new
            {
                Details = $"Cliente con id: {response.CustomerId} Actualizado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Cliente Actualizado Exitosamente", meta);
            return Ok(apiResponse);
        }

        [HttpPatch("Deactivate/{id}")]
        public async Task<IActionResult> DeactivateProduct(int id, UpdateCustomerStatusDto dto)
        {
            var response = await _service.UpdateStatusAsync(id, dto);
            var meta = new
            {
                detail = $"Cliente con id: {response.CustomerId} Actualizado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, $"Cliente {(dto.IsActive ? "Activado" : "Desactivado")} exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpGet("ActiveCustomers")]
        public async Task<IActionResult> GetActiveCustomers()
        {
            var response = await _service.GetActiveCustomersAsync();
            var meta = new
            {
                Details = "Lista de Clientes Activos Obtenida",
                TotalAmount = response.Count()
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Clientes Activos Obtenidos Exitosamente", meta);
            return Ok(apiResponse);
        }
    }
}
