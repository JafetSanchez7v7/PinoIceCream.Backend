using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Domain.Entities
{
    public class Sales
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public decimal SaleTotal { get; set; }

        public virtual ICollection<SalesDetails?> SalesDetails { get; set; } = new List<SalesDetails?>();

    }
}
