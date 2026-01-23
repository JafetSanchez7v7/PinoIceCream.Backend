using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.SalesDtos
{
    public class SalesDto
    {
        public int SaleId { get ; set; }
        public int CustomerId { get; set; }
        public decimal SaleTotal { get; set; }
        public List<SaleDetailsDto> DetailsDtos { get; set; }

    }
}
