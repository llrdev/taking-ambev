﻿using Ambev.Application.DTOs.Branches;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.Branches;

[ExcludeFromCodeCoverage]
public static class BranchMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(cfg =>
        cfg.AddProfile<BranchMapperProfile>()).CreateMapper();

    public static List<BranchGetResponseDTO> ToDTO(this List<Branch> entities)
    {
        return _mapper.Map<List<BranchGetResponseDTO>>(entities);
    }

    public static BranchGetDetailResponseDTO ToDetailDTO(this Branch entity)
    {
        return _mapper.Map<BranchGetDetailResponseDTO>(entity);
    }

    public static BranchPostResponseDTO ToPostResponseDTO(this Branch entity)
    {
        return entity is not null ? _mapper.Map<BranchPostResponseDTO>(entity) : new BranchPostResponseDTO();
    }

    public static BranchPutResponseDTO ToPutResponseDTO(this Branch entity)
    {
        return entity is not null ? _mapper.Map<BranchPutResponseDTO>(entity) : new BranchPutResponseDTO();
    }

    public static Branch ToEntity(this BranchPostRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Branch>(dto) : new Branch();
    }

    public static Branch ToEntity(this BranchPutRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Branch>(dto) : new Branch();
    }
}