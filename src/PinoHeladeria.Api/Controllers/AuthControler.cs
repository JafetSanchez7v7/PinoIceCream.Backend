using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PinoHeladeria.Application.DTOs.LoginDtos;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthControler : ControllerBase
    {
        private readonly IAuthService _login;
        public AuthControler(IAuthService log)
        {
            _login = log;
        }
        [HttpPost("login")]
        [EnableRateLimiting("Fixed ")]
        public async Task<IActionResult>Login(LoginRequestDto loginRequest)
        {
            var response = await _login.Login(loginRequest);
            var apiResp = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Bienvenido de nuevo "+ response.UserName, new {Details = "Login exitoso" });
            return Ok(apiResp);
        }
    }
}
