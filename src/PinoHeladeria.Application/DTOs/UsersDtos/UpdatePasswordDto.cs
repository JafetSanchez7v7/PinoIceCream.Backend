using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.UsersDtos
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(8, ErrorMessage = "Password cannot be less than 8 chars"), MaxLength(20, ErrorMessage = "Password cannot be longer than 20 chars")]
        public string Password { get; set; }
        [Required(ErrorMessage = "New password Is Required")]
        [MinLength(8, ErrorMessage = "new password cannot be less than 8 chars"), MaxLength(20, ErrorMessage = "Password cannot be longer than 20 chars")]
        public string NewPassword { get; set; }
    }
}
