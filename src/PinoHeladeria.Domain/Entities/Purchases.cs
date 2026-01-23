using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Domain.Entities
{
    public class Purchases
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseTotal { get; set; }

        
        public virtual ICollection<PurchaseDetails?> PurchaseDetails { get; set; } = new List<PurchaseDetails?>();
    }
}
