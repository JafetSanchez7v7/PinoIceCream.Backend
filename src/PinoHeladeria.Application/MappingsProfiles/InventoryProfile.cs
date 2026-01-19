using AutoMapper;
using PinoHeladeria.Application.DTOs.InventoryDto;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventories, InventoryDto>().
                ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
        }
    }
}
