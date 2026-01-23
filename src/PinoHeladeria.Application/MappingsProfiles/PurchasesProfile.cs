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


            // 1. De DTO a Entidad (Para Guardar)
            CreateMap<CreatePurchaseDto, Purchases>()
                .ForMember(dest => dest.PurchaseId, opt => opt.Ignore()); // El ID lo genera la DB

            CreateMap<CreatePurchaseDetailDto, PurchaseDetails>()
                .ForMember(dest => dest.PDetailId, opt => opt.Ignore()); // El ID lo genera la DB

            // 2. De Entidad a Response (Para devolver al Front/Postman)
            CreateMap<Purchases, PurchaseDto>();
            CreateMap<PurchaseDetails, PurchasesDetailsDto>();
        }
    }
}
