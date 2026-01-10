using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.SuppliersDto
{
    public class UpdateStatusSupDto
    {
        [Required(ErrorMessage = "Status IsActive is required")]
        public bool IsActive { get; set; }
    }
}
