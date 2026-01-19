using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Domain.Entities
{
    public class Categories
    {
        public int CategoryId { get; set; }
        //Pascal case = CategoryName, CamelCase = categoryName
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
