using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.CategoryDtos
{
    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "El nombre debe tener entre 4 a 50 caracteres")]
        public string CategoryName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre debe tener menos de 100  caracteres")]
        public string? Description { get; set; }
    }
}
