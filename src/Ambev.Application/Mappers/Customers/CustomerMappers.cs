﻿using Ambev.Application.DTOs.Customers;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.Customers;

[ExcludeFromCodeCoverage]
public static class CustomerMappers
{
    private static readonly IMapper _mapper = new MapperConfiguration(cfg =>
        cfg.AddProfile<CustomerMapperProfile>()).CreateMapper();

    public static List<CustomerGetResponseDTO> ToDTO(this List<Customer> entities)
    {
        return _mapper.Map<List<CustomerGetResponseDTO>>(entities);
    }

    public static CustomerGetDetailResponseDTO ToDetailDTO(this Customer entity)
    {
        return _mapper.Map<CustomerGetDetailResponseDTO>(entity);
    }

    public static CustomerPostResponseDTO ToPostResponseDTO(this Customer entity)
    {
        return entity is not null ? _mapper.Map<CustomerPostResponseDTO>(entity) : new CustomerPostResponseDTO();
    }

    public static CustomerPutResponseDTO ToPutResponseDTO(this Customer entity)
    {
        return entity is not null ? _mapper.Map<CustomerPutResponseDTO>(entity) : new CustomerPutResponseDTO();
    }

    public static Customer ToEntity(this CustomerPostRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Customer>(dto) : new Customer();
    }

    public static Customer ToEntity(this CustomerPutRequestDTO dto)
    {
        return dto is not null ? _mapper.Map<Customer>(dto) : new Customer();
    }
}