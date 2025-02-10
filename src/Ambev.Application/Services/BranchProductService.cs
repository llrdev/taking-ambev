﻿using Ambev.Domain.Entities;
using Ambev.Domain.Exceptions;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ambev.Application.Services;

public class BranchProductService : IBranchProductService
{
    private readonly IBranchProductRepository _repository;
    private readonly IProductRepository _productRepository;
    private readonly IValidator<BranchProduct> _validator;
    private readonly ILogger<BranchProductService> _logger;

    public BranchProductService(
        IBranchProductRepository repository,
        IProductRepository productRepository,
        IValidator<BranchProduct> validator,
        ILogger<BranchProductService> logger)
    {
        _repository = repository;
        _productRepository = productRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<BranchProduct> CreateAsync(BranchProduct request)
    {
        await ValidateBranchProductAsync(request);

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");

        MapProductDetailsToBranchProduct(request, product);

        return await _repository.AddAsync(request);
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var branchProduct = await FindBranchProductOrThrowAsync(id);
            await _repository.DeleteAsync(branchProduct);
        }
        catch (Exception ex) when (ex is NotFoundException || ex is EntityAlreadyDeletedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ServiceException("An error occurred while deleting the branch product.", ex);
        }
    }

    public async Task<List<BranchProduct>> GetAllAsync(
        int? id,
        int? branchId,
        int? productId,
        bool? isActive,
        DateTime? startDate,
        DateTime? endDate,
        int page = 1,
        int maxResults = 10)
    {
        try
        {
            if (page <= 0 || maxResults <= 0)
                throw new InvalidPaginationParametersException("Page number and max results must be greater than zero.");

            var criteria = BuildCriteria(id, branchId, productId, isActive, startDate, endDate);
            var result = await _repository.GetAsync(page, maxResults, criteria);
            return result.Items;
        }
        catch (Exception ex) when (ex is InvalidPaginationParametersException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ServiceException("An error occurred while retrieving branch products.", ex);
        }
    }

    public async Task<BranchProduct?> GetByIdAsync(int id)
    {
        try
        {
            var branchProduct = await _repository.GetByIdAsync(id);

            return branchProduct;
        }
        catch (Exception ex)
        {
            throw new ServiceException("An error occurred while retrieving the branch product.", ex);
        }
    }

    public async Task<BranchProduct> UpdateAsync(int id, BranchProduct request)
    {
        var branchProduct = await UpdateBranchProductAsync(id, request);

        await ValidateBranchProductAsync(branchProduct);

        return await _repository.UpdateAsync(branchProduct);
    }

    private async Task ValidateBranchProductAsync(BranchProduct request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
    }

    private async Task<BranchProduct> UpdateBranchProductAsync(int id, BranchProduct request)
    {
        var existingBranchProduct = await FindBranchProductOrThrowAsync(id);

        existingBranchProduct.Price = request.Price;
        existingBranchProduct.StockQuantity = request.StockQuantity;
        existingBranchProduct.IsActive = request.IsActive;

        return existingBranchProduct;
    }

    private void MapProductDetailsToBranchProduct(BranchProduct branchProduct, Product product)
    {
        branchProduct.ProductName = product.Name;
        branchProduct.ProductCategory = product.Category;
    }

    private Expression<Func<BranchProduct, bool>> BuildCriteria(
        int? id,
        int? branchId,
        int? productId,
        bool? isActive,
        DateTime? startDate,
        DateTime? endDate)
    {
        return b =>
            (!id.HasValue || b.Id == id.Value) &&
            (!branchId.HasValue || b.BranchId == branchId.Value) &&
            (!productId.HasValue || b.ProductId == productId.Value) &&
            (!isActive.HasValue || b.IsActive == isActive.Value) &&
            (!startDate.HasValue || b.CreatedAt >= startDate.Value) &&
            (!endDate.HasValue || b.CreatedAt <= endDate.Value);
    }

    private async Task<BranchProduct> FindBranchProductOrThrowAsync(int id)
    {
        var branchProduct = await _repository.GetByIdAsync(id);
        if (branchProduct is null)
            throw new NotFoundException($"BranchProduct with ID {id} not found.");

        return branchProduct;
    }
}