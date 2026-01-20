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
        [Required(ErrorMessage = "El Id del producto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del producto debe ser un número positivo")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser un número positivo")]
        public double PurchasePrice { get; set; }
    }
}
