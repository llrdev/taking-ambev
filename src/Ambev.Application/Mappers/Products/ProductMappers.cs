﻿using Ambev.Application.DTOs.Products;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.Products;

[ExcludeFromCodeCoverage]
public static class ProductMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(cfg =>
        cfg.AddProfile<ProductMapperProfile>()).CreateMapper();

    public static List<ProductGetResponseDTO> ToDTO(this List<Product> entities)
    {
        return _mapper.Map<List<ProductGetResponseDTO>>(entities);
    }

    public static ProductGetDetailResponseDTO ToDetailDTO(this Product entity)
    {
        return _mapper.Map<ProductGetDetailResponseDTO>(entity);
    }

    public static ProductPostResponseDTO ToPostResponseDTO(this Product entity)
    {
        return entity is not null ? _mapper.Map<ProductPostResponseDTO>(entity) : new ProductPostResponseDTO();
    }

    public static ProductPutResponseDTO ToPutResponseDTO(this Product entity)
    {
        return entity is not null ? _mapper.Map<ProductPutResponseDTO>(entity) : new ProductPutResponseDTO();
    }

    public static Product ToEntity(this ProductPostRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Product>(dto) : new Product();
    }

    public static Product ToEntity(this ProductPutRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Product>(dto) : new Product();
    }
}
