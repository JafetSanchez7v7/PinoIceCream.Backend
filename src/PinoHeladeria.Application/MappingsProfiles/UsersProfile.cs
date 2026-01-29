using AutoMapper;
using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Application.DTOs.UsersDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<SsUsers, UserDto>().ReverseMap()
                .ForMember(dest=> dest.PasswordHash, opt => opt.Ignore());

            CreateMap<CreateUserDto, SsUsers>().
                ForMember(dest => dest.UserId, opt => opt.Ignore()).
                ForMember(dest => dest.PasswordHash,opt=> opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password))).    
                ForMember(dest=> dest.IsActive, opt => opt.MapFrom(_ =>true));

            CreateMap<UpdateUserDto, SsUsers>().
                ForAllMembers(opt =>
                //solo mapea si no es nulo
               opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
