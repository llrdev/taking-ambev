using Ambev.Application.DTOs.Sales;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.Sales;

[ExcludeFromCodeCoverage]
public static class SaleMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(cfg =>
        cfg.AddProfile<SaleMapperProfile>()).CreateMapper();

    public static List<SaleGetResponseDTO> ToDTO(this List<Sale> entities)
    {
        return _mapper.Map<List<SaleGetResponseDTO>>(entities);
    }

    public static SaleGetDetailResponseDTO ToDetailDTO(this Sale entity)
    {
        return _mapper.Map<SaleGetDetailResponseDTO>(entity);
    }

    public static SalePostResponseDTO ToPostResponseDTO(this Sale entity)
    {
        return entity is not null ? _mapper.Map<SalePostResponseDTO>(entity) : new SalePostResponseDTO();
    }

    public static SalePutResponseDTO ToPutResponseDTO(this Sale entity)
    {
        return entity is not null ? _mapper.Map<SalePutResponseDTO>(entity) : new SalePutResponseDTO();
    }

    public static Sale ToEntity(this SalePostRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Sale>(dto) : new Sale();
    }

    public static Sale ToEntity(this SalePutRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Sale>(dto) : new Sale();
    }

    public static SaleItemGetDetailDTO ToDetailDTO(this SaleItem entity)
    {
        return _mapper.Map<SaleItemGetDetailDTO>(entity);
    }
}