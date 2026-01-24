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
            // Mapeo Base para consultas
            CreateMap<Sales, SalesDto>().ReverseMap();
            CreateMap<SalesDetails, SaleDetailsDto>().ReverseMap();

            // Mapeo de Creación - DETALLE
            CreateMap<CreateSaleDetailDto, SalesDetails>()
                .ForMember(dest => dest.SaleDetailId, opt => opt.Ignore())
                .ForMember(dest => dest.Sales, opt => opt.Ignore()) // <--- IMPORTANTE
                .ReverseMap();

            // Mapeo de Creación - CABECERA
            CreateMap<CreateSaleDto, Sales>()
                .ForMember(dest => dest.SaleId, opt => opt.Ignore())
                .ForMember(dest => dest.SalesDetails, opt => opt.MapFrom(src => src.SalesDetails)) // Asegúrate que se llame así en el DTO
                .ReverseMap();
        }
    }
}
