using AutoMapper;
using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Application.DTOs.SuppliersDto;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class SuppliersProfile : Profile
    {
        public SuppliersProfile()
        {
            CreateMap<Suppliers, SuppliersDto>();
            // Create
            CreateMap<CreateSupplierDto, Suppliers>()
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            CreateMap<UpdateCategoryDto, SuppliersDto>()
               .ForAllMembers(opt =>
               opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
