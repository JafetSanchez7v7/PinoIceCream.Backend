using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.SalesDtos
{
    public class CreateSaleDetailDto
    {
        [Required(ErrorMessage = "ProductId Is Required")]
        [Range(1, int .MaxValue, ErrorMessage = "The Id has to be a non negative number")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "ProductId Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The Quantity has to be a non negative number")]
        public int Quantity { get; set; }
    }
}
