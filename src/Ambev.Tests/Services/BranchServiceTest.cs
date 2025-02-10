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
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;
using Xunit;

namespace Ambev.Tests.Services;

[Trait("Branch", "Service")]
public class BranchServiceTest
{
    [Fact(DisplayName = "CreateAsync should create a branch successfully")]
    public async Task CreateAsync_ValidBranch_ShouldReturnCreatedBranch()
    {
        // Arrange
        var (repository, validator, _, service) = CreateDependencies();
        var branch = new BranchMock().Generate();

        validator.ValidateAsync(branch).Returns(new ValidationResult());
        repository.AddAsync(branch).Returns(branch);

        // Act
        var result = await service.CreateAsync(branch);

        // Assert
        result.Should().Be(branch);
        await repository.Received(1).AddAsync(branch);
    }

    [Fact(DisplayName = "CreateAsync should throw ServiceException on repository error")]
    public async Task CreateAsync_RepositoryError_ShouldThrowServiceException()
    {
        // Arrange
        var (repository, validator, _, service) = CreateDependencies();
        var branch = new BranchMock().Generate();

        validator.ValidateAsync(branch).Returns(new ValidationResult());
        repository.AddAsync(branch).Throws(new Exception("Repository error"));

        // Act
        Func<Task> act = () => service.CreateAsync(branch);

        // Assert
        await act.Should().ThrowAsync<ServiceException>()
            .WithMessage("An error occurred while creating a branch.");
    }

    [Fact(DisplayName = "DeleteAsync should delete a branch successfully")]
    public async Task DeleteAsync_ValidBranchId_ShouldDeleteBranch()
    {
        // Arrange
        var (repository, _, _, service) = CreateDependencies();
        var branch = new BranchMock().Generate();

        repository.GetByIdAsync(branch.Id).Returns(branch);

        // Act
        await service.DeleteAsync(branch.Id);

        // Assert
        await repository.Received(1).DeleteAsync(branch);
    }

    [Fact(DisplayName = "DeleteAsync should throw NotFoundException if branch does not exist")]
    public async Task DeleteAsync_BranchNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var (repository, _, _, service) = CreateDependencies();
        int branchId = 1;
        repository.GetByIdAsync(branchId).Returns(default(Branch));

        // Act
        Func<Task> act = () => service.DeleteAsync(branchId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Branch with ID 1 not found.");
    }

    [Fact(DisplayName = "GetAllAsync should return branches based on criteria")]
    public async Task GetAllAsync_ValidCriteria_ShouldReturnBranches()
    {
        // Arrange
        var (repository, _, _, service) = CreateDependencies();
        var branches = new BranchMock().Generate(2);

        repository.GetAsync(1, 10, Arg.Any<Expression<Func<Branch, bool>>>()).Returns(new PagedResult<Branch>(branches.Count(), branches));

        // Act
        var result = await service.GetAllAsync(null, true, null, null, null, 1, 10);

        // Assert
        result.Should().BeEquivalentTo(branches);
    }

    [Fact(DisplayName = "GetByIdAsync should return the branch if exists")]
    public async Task GetByIdAsync_ExistingId_ShouldReturnBranch()
    {
        // Arrange
        var (repository, _, _, service) = CreateDependencies();
        var branch = new BranchMock().Generate();
        repository.GetByIdAsync(branch.Id).Returns(branch);

        // Act
        var result = await service.GetByIdAsync(branch.Id);

        // Assert
        result.Should().Be(branch);
    }

    [Fact(DisplayName = "GetByIdAsync should return null if branch not found")]
    public async Task GetByIdAsync_BranchNotFound_ShouldReturnNull()
    {
        // Arrange
        var (repository, _, _, service) = CreateDependencies();
        int branchId = 1;
        repository.GetByIdAsync(branchId).Returns(default(Branch));

        // Act
        var result = await service.GetByIdAsync(branchId);

        // Assert
        result.Should().BeNull();
    }

    [Fact(DisplayName = "UpdateAsync should update the branch successfully")]
    public async Task UpdateAsync_ValidBranch_ShouldUpdateBranch()
    {
        // Arrange
        var (repository, validator, _, service) = CreateDependencies();
        var existingBranch = new BranchMock().Generate();
        var updateBranch = new BranchMock().Generate();

        repository.GetByIdAsync(existingBranch.Id).Returns(existingBranch);
        validator.ValidateAsync(existingBranch).Returns(Task.FromResult(new ValidationResult()));
        repository.UpdateAsync(existingBranch).Returns(Task.FromResult(existingBranch));

        // Act
        var result = await service.UpdateAsync(existingBranch.Id, updateBranch);

        // Assert
        result.Should().BeEquivalentTo(existingBranch);
        await repository.Received(1).UpdateAsync(existingBranch);
    }

    [Fact(DisplayName = "UpdateAsync should throw ServiceException on validation failure")]
    public async Task UpdateAsync_InvalidBranch_ShouldThrowServiceException()
    {
        // Arrange
        var (repository, validator, _, service) = CreateDependencies();
        var existingBranch = new BranchMock().Generate();
        var updateBranch = new BranchMock().Generate();
        repository.GetByIdAsync(existingBranch.Id).Returns(existingBranch);
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name cannot be empty.")
        });
        validator.ValidateAsync(existingBranch).Returns(validationResult);

        // Act
        Func<Task> act = () => service.UpdateAsync(existingBranch.Id, updateBranch);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "UpdateAsync should throw NotFoundException if branch does not exist")]
    public async Task UpdateAsync_BranchNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var (repository, _, _, service) = CreateDependencies();
        int branchId = 1;
        var updateBranch = new Branch { Name = "Updated Branch" };
        repository.GetByIdAsync(branchId).Returns(default(Branch));

        // Act
        Func<Task> act = () => service.UpdateAsync(branchId, updateBranch);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Branch with ID 1 not found.");
    }

    private (IBranchRepository repository, IValidator<Branch> validator, ILogger<BranchService> logger, BranchService service) CreateDependencies()
    {
        var repository = Substitute.For<IBranchRepository>();
        var validator = Substitute.For<IValidator<Branch>>();
        var logger = Substitute.For<ILogger<BranchService>>();
        var service = new BranchService(repository, validator, logger);
        return (repository, validator, logger, service);
    }
}