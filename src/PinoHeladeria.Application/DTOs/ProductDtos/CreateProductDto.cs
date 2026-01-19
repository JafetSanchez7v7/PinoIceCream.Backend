using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.DTOs.ProductDtos
{
    public class CreateProductDto
    {
        [Required(ErrorMessage ="Name is Required")]
        [MaxLength(30, ErrorMessage = "Product Name can´t exceed 30 characters"), MinLength(5, ErrorMessage = "Product Name Cant have less than 5 characters")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "SupplierId is Required")]
        public int SupplierId { get; set; }
        [Required(ErrorMessage = "CategoryId is Required")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage="Description is Required")]
        [MaxLength(50, ErrorMessage = "Description can´t exceed 50 characters"), MinLength(5, ErrorMessage ="Description Cant have less than 5 characters")]
        public string Description { get; set; }
    }
}
