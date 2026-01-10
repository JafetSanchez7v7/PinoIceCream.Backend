using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Domain.Entities
{
    public class Suppliers
    {
        public int SupplierId { get; set; }
        public string? SuplierName { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public bool IsActive { get; set; }
    }
}
