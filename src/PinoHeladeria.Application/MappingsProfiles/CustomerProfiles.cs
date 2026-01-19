using AutoMapper;
using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Application.DTOs.CustomerDto;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class CustomerProfiles : Profile
    {
        public CustomerProfiles()
        {
            CreateMap<CustomerDto, Customers>();
            // Create
            CreateMap<CreateCustomerDto, Customers>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            CreateMap<UpdateCustomerDto, CustomerDto>()
                .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
