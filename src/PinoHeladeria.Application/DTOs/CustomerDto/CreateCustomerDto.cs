using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.CustomerDto
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage ="Name Is Required")]
        [StringLength(30, ErrorMessage = "Name Cannot Exceed 30 Characters"), MinLength(5, ErrorMessage ="Name length cant be less than 5 chars")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(50, ErrorMessage = "Description Cannot Exceed 50 Characters"), MinLength(5, ErrorMessage = "Name length cant be less than 5 chars")]
        public string CustomerDescription { get; set; }
    }
}
