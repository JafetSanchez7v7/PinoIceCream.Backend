using AutoMapper;
using PinoHeladeria.Application.DTOs.ProductDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<Products, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.SuplierName));

            // Create
            CreateMap<CreateProductDto, Products>()
               .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            CreateMap<UpdateProductDto, ProductDto>()
                .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)
            );
        }
    }
}
