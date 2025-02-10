﻿using Ambev.Application.Services;
using Ambev.Domain.Base;
using Ambev.Domain.Entities;
using Ambev.Domain.Exceptions;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Tests.Mocks.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Ambev.Tests.Services;

public class BranchProductServiceTest
{
    [Fact(DisplayName = "CreateAsync should create a branch product successfully")]
    [Trait("BranchProduct", "Service")]
    public async Task CreateAsync_ValidBranchProduct_ShouldCreateBranchProduct()
    {
        // Arrange
        var (repository, productRepository, validator, logger, service) = CreateDependencies();
        var branchProduct = new BranchProductMock().Generate();
        var product = new ProductMock().Generate();

        validator.ValidateAsync(branchProduct).Returns(Task.FromResult(new ValidationResult()));
        productRepository.GetByIdAsync(branchProduct.ProductId).Returns(product);
        repository.AddAsync(branchProduct).Returns(branchProduct);

        // Act
        var result = await service.CreateAsync(branchProduct);

        // Assert
        result.Should().BeEquivalentTo(branchProduct);
        await repository.Received(1).AddAsync(branchProduct);
    }

    [Fact(DisplayName = "CreateAsync should throw NotFoundException if product does not exist")]
    [Trait("BranchProduct", "Service")]
    public async Task CreateAsync_ProductNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var (repository, productRepository, validator, logger, service) = CreateDependencies();
        var branchProduct = new BranchProductMock().Generate();
        validator.ValidateAsync(branchProduct).Returns(Task.FromResult(new ValidationResult()));
        productRepository.GetByIdAsync(branchProduct.ProductId).Returns(default(Product));

        // Act
        Func<Task> act = () => service.CreateAsync(branchProduct);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Product with ID {branchProduct.ProductId} not found.");
    }

    [Fact(DisplayName = "DeleteAsync should delete the branch product successfully")]
    [Trait("BranchProduct", "Service")]
    public async Task DeleteAsync_ValidId_ShouldDeleteBranchProduct()
    {
        // Arrange
        var (repository, productRepository, validator, logger, service) = CreateDependencies();
        var branchProduct = new BranchProductMock().Generate();
        repository.GetByIdAsync(branchProduct.Id).Returns(branchProduct);

        // Act
        await service.DeleteAsync(branchProduct.Id);

        // Assert
        await repository.Received(1).DeleteAsync(branchProduct);
    }

    [Fact(DisplayName = "DeleteAsync should throw NotFoundException if branch product does not exist")]
    [Trait("BranchProduct", "Service")]
    public async Task DeleteAsync_BranchProductNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var (repository, productRepository, validator, logger, service) = CreateDependencies();
        int invalidId = 999;
        repository.GetByIdAsync(invalidId).Returns(default(BranchProduct));

        // Act
        Func<Task> act = () => service.DeleteAsync(invalidId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"BranchProduct with ID {invalidId} not found.");
    }

    [Fact(DisplayName = "GetAllAsync should return a list of branch products")]
    [Trait("BranchProduct", "Service")]
    public async Task GetAllAsync_ValidCriteria_ShouldReturnBranchProducts()
    {
        // Arrange
        var (repository, productRepository, validator, logger, service) = CreateDependencies();
        var branchProducts = new BranchProductMock().Generate(1);
        repository.GetAsync(1, 10, Arg.Any<Expression<Func<BranchProduct, bool>>?>()).Returns(new PagedResult<BranchProduct>(1, branchProducts));

        // Act
        var result = await service.GetAllAsync(default, default, default, default, default, default);

        // Assert
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(branchProducts);
    }

    [Fact(DisplayName = "GetByIdAsync should return the branch product if found")]
    [Trait("BranchProduct", "Service")]
    public async Task GetByIdAsync_ValidId_ShouldReturnBranchProduct()
    {
        // Arrange
        var (repository, productRepository, validator, logger, service) = CreateDependencies();
        var branchProduct = new BranchProductMock().Generate();
        repository.GetByIdAsync(branchProduct.Id).Returns(branchProduct);

        // Act
        var result = await service.GetByIdAsync(branchProduct.Id);

        // Assert
        result.Should().BeEquivalentTo(branchProduct);
    }

    [Fact(DisplayName = "UpdateAsync should update the branch product successfully")]
    [Trait("BranchProduct", "Service")]
    public async Task UpdateAsync_ValidBranchProduct_ShouldUpdateBranchProduct()
    {
        // Arrange
        var (repository, productRepository, validator, logger, service) = CreateDependencies();
        var existingBranchProduct = new BranchProductMock().Generate();
        var updateBranchProduct = new BranchProductMock().Generate();

        repository.GetByIdAsync(existingBranchProduct.Id).Returns(existingBranchProduct);
        validator.ValidateAsync(existingBranchProduct).Returns(Task.FromResult(new ValidationResult()));
        repository.UpdateAsync(existingBranchProduct).Returns(existingBranchProduct);

        // Act
        var result = await service.UpdateAsync(existingBranchProduct.Id, updateBranchProduct);

        // Assert
        result.Should().BeEquivalentTo(existingBranchProduct);
        await repository.Received(1).UpdateAsync(existingBranchProduct);
    }

    private (IBranchProductRepository repository, IProductRepository productRepository, IValidator<BranchProduct> validator, ILogger<BranchProductService> logger, BranchProductService service) CreateDependencies()
    {
        var repository = Substitute.For<IBranchProductRepository>();
        var productRepository = Substitute.For<IProductRepository>();
        var validator = Substitute.For<IValidator<BranchProduct>>();
        var logger = Substitute.For<ILogger<BranchProductService>>();
        var service = new BranchProductService(repository, productRepository, validator, logger);

        return (repository, productRepository, validator, logger, service);
    }
}