using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.PurchasesDtos
{
    public class PurchaseDto
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseTotal { get; set; }
        public List<PurchasesDetailsDto> PurchaseDetails { get; set; } = new List<PurchasesDetailsDto>();
    }
}
