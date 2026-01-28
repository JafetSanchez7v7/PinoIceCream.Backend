using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.UsersDtos
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "User Name is Required")]
        [MinLength(4, ErrorMessage = "User Name cannot be less than 4 chars"), MaxLength(20, ErrorMessage = "User Name cannot be longer than 20 chars")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "old password is Required")]
        [MinLength(8, ErrorMessage = "Password cannot be less than 8 chars"), MaxLength(20, ErrorMessage = "Password cannot be longer than 20 chars")]

        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(8, ErrorMessage = "Password cannot be less than 8 chars"), MaxLength(20, ErrorMessage = "Password cannot be longer than 20 chars")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="IsActive Is Required")]
        public bool IsActive { get; set; }
  
    }
}
