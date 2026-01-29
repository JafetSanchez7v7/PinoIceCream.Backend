using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.RolesDtos
{
    public class UpdateRoleStatusDto
    {
        [Required(ErrorMessage = "IsActive is Required")]
        public bool IsActive { get; set; }
    }
}
