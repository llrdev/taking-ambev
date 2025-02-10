using Ambev.Application.DTOs.Customers;
using Ambev.Domain.Entities;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Mappers.Customers;

[ExcludeFromCodeCoverage]
public class CustomerMapperProfile : Profile
{
    public CustomerMapperProfile()
    {
        CreateMap<CustomerGetDetailResponseDTO, Customer>().ReverseMap();

        CreateMap<CustomerGetResponseDTO, Customer>().ReverseMap();

        CreateMap<CustomerPostRequestDTO, Customer>().ReverseMap();

        CreateMap<CustomerPostResponseDTO, Customer>().ReverseMap();

        CreateMap<CustomerPutRequestDTO, Customer>().ReverseMap();

        CreateMap<CustomerPutResponseDTO, Customer>().ReverseMap();
    }
}
