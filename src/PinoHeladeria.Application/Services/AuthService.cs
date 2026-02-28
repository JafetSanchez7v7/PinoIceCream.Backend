using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PinoHeladeria.Application.DTOs.LoginDtos;
using PinoHeladeria.Application.Exceptions;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.Services_Interfaces;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services
{
    public class AuthService : IAuthService  
    {
        private readonly IRoleRepository _role;
        private readonly IUsersRepository _user;
        private readonly IConfiguration _configuration;
        public AuthService(IRoleRepository role, IUsersRepository user, IConfiguration con)
        {                  
            (_role, _user, _configuration) = (role, user, con);
        }
        private string GenerateJWT(SsUsers user, IEnumerable<string> roles)
        {
            // Lógica para generar el token JWT basado en el usuario y sus roles
            // Esto puede incluir la creación de claims, firma del token, etc.
           // Reemplazar con el token generado real
           var SecretKey = _configuration["JwtSettings:SecretKey"];
           var Issuer = _configuration["JwtSettings:Issuer"];
           var Audience = _configuration["JwtSettings:Audience"];
            // Creacion de los claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // Agregar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Encriptar SecretKey
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);
            // codigo para generar el token
            var token = new JwtSecurityToken(
               issuer : Issuer,
               audience: Audience,
               claims : claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var user = await _user.GetByNameAsync(dto.UserName);
            
            if (user == null )
            {
                throw new UnauthorizedException("Invalid username or password.");
            }
            var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                throw new UnauthorizedException("Invalid username or password.");
            }

            var roles = await _role.GetRolesByUserId(user.UserId);
            // creacion del jwt
             var token = GenerateJWT(user, roles);
            //mapeo de la respuesta
            var response = new LoginResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Token = token,
                Roles = roles
            };

            return response;

        }

    }
}
