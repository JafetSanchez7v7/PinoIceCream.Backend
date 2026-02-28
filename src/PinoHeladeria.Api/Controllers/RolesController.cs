using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs.RolesDtos;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;
        public RolesController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllRolesAsync();
            var meta = new
            {
                RolesAmount = response.Count()
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Roles Obtenidos exitosamente", meta);
            return Ok(apiResp);
        }

        [HttpGet("Actives")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetActiveRoles()
        {
            var response = await _service.GetActiveRolesAsync();
            var meta = new
            {
                RolesAmount = response.Count()
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Roles Activos Obtenidos exitosamente", meta);
            return Ok(apiResp);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetRoleByIdAsync(id);
            var meta = new
            {
                Details = $"rol con id {id} obtenido exitosamente"
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Roles Obtenidos exitosamente", meta);
            return Ok(apiResp);
        }

        [HttpGet("{name}/byName")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetByName(string name)
        {
            var response = await _service.GetByNameAsync(name);
            var meta = new
            {
                Details = $"Rol con nombre {name} obtenido exitosamente"
            };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Roles Obtenidos exitosamente", meta);
            return Ok(apiResp);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> AddRole([FromBody] CreateRoleDto dto)
        {
            var response = await _service.AddAsync(dto);
            var meta = new { Details = "Rol creado exitosamente" };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(201, response, "Exito", meta);
            return CreatedAtAction(nameof(GetById), new { id = response.RoleId }, apiResp);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            var response = await _service.UpdateAsync(id, dto);
            var meta = new { Details = $"Rol con id {id} actualizado exitosamente" };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Exito", meta);
            return Ok(apiResp);

        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRoleStatusDto dto)
        {
            var response = await _service.UpdateStatusAsync(id, dto);
            var meta = new { Details = $"Rol con id {id} actualizado exitosamente" };
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Exito", meta);
            return Ok(apiResp);
        }



    }
}
