using AutoMapper;
using PinoHeladeria.Application.DTOs.SalesDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class SalesProfile : Profile
    {
        public SalesProfile()
        {
            CreateMap<SalesDto, Sales>();
            CreateMap<SaleDetailsDto, SalesDetails>();
            //Creacion
            CreateMap<CreateSaleDetailDto, SalesDetails>().
                ForMember(dest => dest.SaleDetailId, opt => opt.Ignore());
            //Master
            CreateMap<CreateSaleDto, Sales>().
                ForMember(dest=> dest.SaleId, opt => opt.Ignore());
        }
    }
}
