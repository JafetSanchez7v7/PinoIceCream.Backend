using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Domain.Entities
{
    public class Products
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        //Navigation Properties
        public virtual Categories? Category { get; set; }
        public virtual Suppliers? Supplier { get; set; }

    }
}
