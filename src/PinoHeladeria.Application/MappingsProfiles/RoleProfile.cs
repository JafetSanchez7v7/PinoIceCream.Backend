using AutoMapper;
using PinoHeladeria.Application.DTOs.RolesDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile() { 
        
            CreateMap<Roles, RoleDto>().ReverseMap();
            // Create
            CreateMap<CreateRoleDto, Roles>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true)).
                ReverseMap();
            //Update
            CreateMap<UpdateRoleDto, Roles>().ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));




        }
    }
}
