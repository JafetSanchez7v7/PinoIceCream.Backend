using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Domain.Entities
{
    public class SalesDetails
    {
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public int ProductId {  get; set; }
        public int Quantity { get; set; }
        public decimal Total {get; set; }
        [ForeignKey("SaleId")]
        public virtual Sales Sales { get; set; }
    }
}
