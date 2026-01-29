using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.PurchasesDtos
{
    public class CreatePurchaseDetailDto
    {
        [Required(ErrorMessage = "Product Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "The Id has to be a non negative number")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity has to be a non negative number")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "The Purchase Price is Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price has to be a non negative number")]
        public double PurchasePrice { get; set; }
    }
}
