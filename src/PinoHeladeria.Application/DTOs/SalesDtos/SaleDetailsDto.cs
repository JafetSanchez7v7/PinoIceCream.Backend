using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.SalesDtos
{
    public class SaleDetailsDto
    {
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity {  get; set; }
        public decimal  Total { get; set; }
    }
}
