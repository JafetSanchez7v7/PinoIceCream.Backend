using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.CustomerDto
{
    public class UpdateCustomerStatusDto
    {
        [Required(ErrorMessage ="Status Is Required")]
        public bool IsActive { get; set; }
    }
}
