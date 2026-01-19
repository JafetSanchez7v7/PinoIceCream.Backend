using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.CustomerDto
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
