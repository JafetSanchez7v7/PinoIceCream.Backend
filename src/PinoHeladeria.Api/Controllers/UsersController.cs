using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs.UsersDtos;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService ser)
        {
            _userService = ser;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            var meta = new
            {
                TotalAmount = users.Count()
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, users, "Lista de usuarios obtenida con exito", meta);
            return Ok(ApiResponse);
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _userService.GetActiveUsersAsync();
            var meta = new
            {
                TotalAmount = users.Count()
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, users, "Lista de usuarios activos obtenida con exito", meta);
            return Ok(ApiResponse);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var users = await _userService.GetByIdAsync(id);
            var meta = new
            {
                Details = "Usuario obtenido correctamente"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, users, $"usuario con id {id} obtenido correctamente", meta);
            return Ok(ApiResponse);
        }
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var users = await _userService.GetByNameAsync(name);
            var meta = new
            {
                Details = "Usuario obtenido correctamente"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, users, $"usuario con nombre {name} obtenido correctamente", meta);
            return Ok(ApiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateUserDto dto)
        {
           var response = await _userService.AddAsync(dto);
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(201, response, "Usuario creado con exito");
            return CreatedAtAction(nameof(GetById), new { id = response.UserId }, ApiResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateUserDto dto)
        {
            var response = await _userService.UpdateAsync(id, dto);
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Usuario actualizado con exito");
            return Ok(ApiResponse);
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatusAsync(int id, UpdateUserStatusDto dto)
        {
            var response = await _userService.UpdateStatusAsync(id, dto);
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Estado del usuario actualizado con exito");
            return Ok(ApiResponse);
        }
        [HttpPatch("{id}/password")]
        public async Task<IActionResult> UpdatePassWordAsync(int id, UpdatePasswordDto dto)
        {
            await _userService.UpdatePassWordAsync(id, dto);
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create<object>(200, null, "Contraseña del usuario actualizada con exito");
            return Ok(ApiResponse);
        }
    }

}
