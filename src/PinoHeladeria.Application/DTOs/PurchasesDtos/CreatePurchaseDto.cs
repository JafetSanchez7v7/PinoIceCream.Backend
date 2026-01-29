using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.PurchasesDtos
{
    public class CreatePurchaseDto
    {

        [Required(ErrorMessage = "El Id del proveedor es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del proveedor debe ser un número positivo")]

        public int SupplierId { get; set; }


        [Required(ErrorMessage = "Los detalles de la compra son obligatorios")]
        [MinLength(1, ErrorMessage = "Debe haber al menos un detalle de compra")]
        public List<CreatePurchaseDetailDto> PurchaseDetails { get; set; }
    }
}
