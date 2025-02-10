using Ambev.Application.DTOs.Sales;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.Sales;

[ExcludeFromCodeCoverage]
public class SaleMapperProfile : Profile
{
    public SaleMapperProfile()
    {
        CreateMap<SaleItemGetDTO, SaleItem>().ReverseMap();
        CreateMap<SaleItemGetDetailDTO, SaleItem>().ReverseMap();
        CreateMap<SaleItemPostDTO, SaleItem>().ReverseMap();

        CreateMap<SaleGetDetailResponseDTO, Sale>().ReverseMap();
        CreateMap<SaleGetResponseDTO, Sale>().ReverseMap();
        CreateMap<SalePutRequestDTO, Sale>().ReverseMap();
        CreateMap<SalePutResponseDTO, Sale>().ReverseMap();
        CreateMap<SalePostRequestDTO, Sale>().ReverseMap();
        CreateMap<SalePostResponseDTO, Sale>().ReverseMap();
    }
}