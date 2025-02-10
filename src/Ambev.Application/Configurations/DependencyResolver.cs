﻿using Ambev.Application.Mappers.Branches;
using Ambev.Application.Mappers.BranchProducts;
using Ambev.Application.Mappers.Customers;
using Ambev.Application.Mappers.Products;
using Ambev.Application.Mappers.Sales;
using Ambev.Application.Services;
using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces.Integrations;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Domain.Interfaces.Services;
using Ambev.Domain.Validators;
using Ambev.Infrastructure.Contexts;
using Ambev.Infrastructure.Integrations;
using Ambev.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Application.Configurations;

[ExcludeFromCodeCoverage]
public static class DependencyResolver
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.ResolveAutoMapper();
        services.ResolveFluentValidators();
        services.ResolveRepositories();
        services.ResolveServices();

        services.AddSingleton<IRabbitMQIntegration, RabbitMQIntegration>();

        return services;
    }

    private static void ResolveRepositories(this IServiceCollection services)
    {
        services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("CONNECTIONSTRING"))
            .EnableSensitiveDataLogging(true));

        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IBranchProductRepository, BranchProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ISaleItemRepository, SaleItemRepository>();
    }

    private static void ResolveServices(this IServiceCollection services)
    {
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IBranchProductService, BranchProductService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISaleService, SaleService>();
    }

    private static void ResolveAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BranchMapperProfile));
        services.AddAutoMapper(typeof(ProductMapperProfile));
        services.AddAutoMapper(typeof(BranchProductMapperProfile));
        services.AddAutoMapper(typeof(CustomerMapperProfile));
        services.AddAutoMapper(typeof(SaleMapperProfile));
    }

    private static void ResolveFluentValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Branch>, BranchValidator>();
        services.AddScoped<IValidator<BranchProduct>, BranchProductValidator>();
        services.AddScoped<IValidator<Product>, ProductValidator>();
        services.AddScoped<IValidator<Customer>, CustomerValidator>();
        services.AddScoped<IValidator<Sale>, SaleValidator>();
        services.AddScoped<IValidator<SaleItem>, SaleItemValidator>();
    }
}