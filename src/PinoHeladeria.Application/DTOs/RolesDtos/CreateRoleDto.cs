using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.RolesDtos
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [MaxLength(20, ErrorMessage = "El nombre del rol no puede exceder los 20 caracteres"), MinLength(4, ErrorMessage = "El nombre del rol no puede ser menor de 4 caracteres")]
        public string RoleName { get; set; }
    }
}
