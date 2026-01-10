using AutoMapper;
using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class CategoryProfiles : Profile
    {
        public CategoryProfiles()
        {
            CreateMap<Categories, CategoryDto>();
            // Create
            CreateMap<CreateCategoryDto, Categories>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            CreateMap<UpdateCategoryDto, CategoryDto>()
                .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)
            );
        }
        
    }
}
