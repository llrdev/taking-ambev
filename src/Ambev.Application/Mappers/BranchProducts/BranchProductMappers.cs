﻿using Ambev.Application.DTOs.BranchProducts;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.BranchProducts;

[ExcludeFromCodeCoverage]
public static class BranchProductMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(cfg =>
        cfg.AddProfile<BranchProductMapperProfile>()).CreateMapper();

    public static List<BranchProductGetResponseDTO> ToDTO(this List<BranchProduct> entities)
    {
        return _mapper.Map<List<BranchProductGetResponseDTO>>(entities);
    }

    public static BranchProductGetDetailResponseDTO ToDetailDTO(this BranchProduct entity)
    {
        return _mapper.Map<BranchProductGetDetailResponseDTO>(entity);
    }

    public static BranchProductPostResponseDTO ToPostResponseDTO(this BranchProduct entity)
    {
        return entity is not null ? _mapper.Map<BranchProductPostResponseDTO>(entity) : new BranchProductPostResponseDTO();
    }

    public static BranchProductPutResponseDTO ToPutResponseDTO(this BranchProduct entity)
    {
        return entity is not null ? _mapper.Map<BranchProductPutResponseDTO>(entity) : new BranchProductPutResponseDTO();
    }

    public static BranchProduct ToEntity(this BranchProductPostRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<BranchProduct>(dto) : new BranchProduct();
    }

    public static BranchProduct ToEntity(this BranchProductPutRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<BranchProduct>(dto) : new BranchProduct();
    }
}