using AutoMapper;
using PinoHeladeria.Application.DTOs.PurchasesDtos;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.MappingsProfiles
{
    public class PurchasesProfile : Profile
    {
        public PurchasesProfile() {


            CreateMap<CreatePurchaseDto, Purchases>().
                ForMember(dest => dest.PurchaseId, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<CreatePurchaseDetailDto, PurchaseDetails>().
                ForMember(dest => dest.PDetailId, opt => opt.Ignore());

            CreateMap<PurchaseDetails, PurchasesDetailsDto>().ReverseMap();
            CreateMap<Purchases, PurchaseDto>().ReverseMap();
        }
    }
}
