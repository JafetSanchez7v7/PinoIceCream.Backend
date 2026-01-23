using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.SalesDtos
{
    public class CreateSaleDto
    {
        [Required(ErrorMessage= "CustomerId is required")]
        public int CustomerId {  get; set; }
        [Required(ErrorMessage= "Sale Details Are Required ")]
        [MinLength(1, ErrorMessage = "it has to be at least one Sale Detail")]
        public List<CreateSaleDetailDto> Details { get; set; }
        
    }
}
