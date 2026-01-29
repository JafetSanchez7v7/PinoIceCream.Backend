using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.CustomerDto
{
    public class UpdateCustomerDto
    {
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        public string CustomerName { get; set; } = null!;

        [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres.")]
        public string CustomerDescription { get; set; }
        [Required(ErrorMessage = "IsActive is Required")]
        public bool IsActive { get; set; }
    }
}
