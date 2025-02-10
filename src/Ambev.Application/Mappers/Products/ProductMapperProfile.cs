﻿using Ambev.Application.DTOs.Products;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.Products;

[ExcludeFromCodeCoverage]
public class ProductMapperProfile : Profile
{
    public ProductMapperProfile()
    {
        CreateMap<ProductGetDetailResponseDTO, Product>().ReverseMap();

        CreateMap<ProductGetResponseDTO, Product>().ReverseMap();

        CreateMap<ProductPostRequestDTO, Product>().ReverseMap();

        CreateMap<ProductPostResponseDTO, Product>().ReverseMap();

        CreateMap<ProductPutRequestDTO, Product>().ReverseMap();

        CreateMap<ProductPutResponseDTO, Product>().ReverseMap();
    }
}
