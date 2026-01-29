using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.LoginDtos
{
    public class LoginResponseDto
    {
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public string  Token { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
