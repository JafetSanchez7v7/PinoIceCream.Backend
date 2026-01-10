using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.SuppliersDto
{
    public class CreateSupplierDto
    {

        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(25, ErrorMessage = "El nombre no puede exceder los 25 caracteres")]
        public string SuplierName { get; set; }

        [Required(ErrorMessage = "Phone is Required")]
        [MaxLength(8, ErrorMessage = "El telefono no puede exceder los 8 caracteres")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Location is Required")]
        [MaxLength(50, ErrorMessage = "La ubicacion no puede exceder los 50 caracteres")]
        public string Location { get; set; }


    }
}
