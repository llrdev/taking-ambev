﻿using Ambev.Application.Events.Sales;
using Ambev.Application.Services;
using Ambev.Domain.Base;
using Ambev.Domain.Entities;
using Ambev.Domain.Enums;
using Ambev.Domain.Exceptions;
using Ambev.Domain.Interfaces.Integrations;
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

public class SaleServiceTest
{
    [Fact(DisplayName = "Create Sale - Valid Input")]
    [Trait("Sale", "Service")]
    public async Task CreateAsync_ShouldCreateSale_WhenValidRequest()
    {
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var saleMock = new SaleMock().Generate();
        saleMock.Status = SaleStatus.Created;
        saleMock.Items = new List<SaleItem> { new SaleItem { ProductId = 1, Quantity = 1 } };

        var branchProduct = new BranchProduct { ProductId = saleMock.Items[0].ProductId, Price = 100, StockQuantity = 10, BranchId = saleMock.BranchId };

        repository.AddAsync(Arg.Any<Sale>()).Returns(saleMock);
        branchProductRepository.GetAsync(1, 1, Arg.Any<Expression<Func<BranchProduct, bool>>>()).Returns(new PagedResult<BranchProduct>(1, new List<BranchProduct> { branchProduct }));
        validator.ValidateAsync(Arg.Any<Sale>()).Returns(new ValidationResult());

        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);

        // Act
        var result = await saleService.CreateAsync(saleMock);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(SaleStatus.Created);

        saleMock.Items.Count.Should().Be(1, "a sale should have exactly 1 item");
        saleMock.Items[0].Quantity.Should().Be(1, "the item quantity should be 1");

        await repository.Received(1).AddAsync(Arg.Is<Sale>(s => s.CustomerId == saleMock.CustomerId));
        await branchProductRepository.Received(1).UpdateAsync(Arg.Is<BranchProduct>(bp => bp.StockQuantity == 9));
        await rabbitMQIntegration.Received(1).PublishMessageAsync(Arg.Is<SaleCreatedEvent>(e => e.Id == result.Id));
    }

    [Fact(DisplayName = "Create Sale - Validation Failed")]
    [Trait("Sale", "Service")]
    public async Task CreateAsync_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();
        var saleMock = new SaleMock().Generate();
        var branchProducts = new BranchProductMock().Generate(1);

        branchProductRepository.GetAsync(1, 1, Arg.Any<Expression<Func<BranchProduct, bool>>>()).Returns(Task.FromResult(new PagedResult<BranchProduct>(1, branchProducts)));
        validator.ValidateAsync(Arg.Any<Sale>()).Returns(new ValidationResult(new List<ValidationFailure> {
            new ValidationFailure("CustomerId", "Customer is required") }));

        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);

        // Act
        Func<Task> act = async () => await saleService.CreateAsync(saleMock);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Create Sale - Product Out Of Stock")]
    [Trait("Sale", "Service")]
    public async Task CreateAsync_ShouldThrowItemOutOfStockException_WhenProductIsOutOfStock()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();
        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);
        var saleMock = new SaleMock().Generate();
        var branchProduct = new BranchProduct { ProductId = saleMock.Items[0].ProductId, Price = 100, StockQuantity = 0 };

        branchProductRepository.GetAsync(1, 1, Arg.Any<Expression<Func<BranchProduct, bool>>>()).Returns(new PagedResult<BranchProduct>(1, new List<BranchProduct> { branchProduct }));
        validator.ValidateAsync(saleMock).Returns(new ValidationResult());

        // Act
        Func<Task> act = async () => await saleService.CreateAsync(saleMock);

        // Assert
        await act.Should().ThrowAsync<ItemOutOfStockException>();
    }

    [Fact(DisplayName = "Create Sale - BranchProduct Not Found")]
    [Trait("Sale", "Service")]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenBranchProductNotFound()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();
        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);
        var saleMock = new SaleMock().Generate();

        branchProductRepository.GetAsync(1, 1, Arg.Any<Expression<Func<BranchProduct, bool>>>()).Returns(new PagedResult<BranchProduct>(0, new List<BranchProduct>()));
        validator.ValidateAsync(saleMock).Returns(new ValidationResult());

        // Act
        Func<Task> act = async () => await saleService.CreateAsync(saleMock);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact(DisplayName = "Create Sale - Validate Discount on Items with Branch Product Price")]
    [Trait("Sale", "Service")]
    public async Task CreateSaleAsync_ShouldApplyDiscount_WhenValidDiscountProvided()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var sale = new SaleMock().Generate();
        sale.Items = new List<SaleItem>()
        {
            new SaleItem
            {
                ProductId = 1,
                Quantity = 1,
                Discount = 25,
                Sequence = 2
            }
        };

        var branchProduct = new BranchProduct
        {
            ProductId = sale.Items[0].ProductId,
            BranchId = 1,
            Price = 100,
            StockQuantity = 100,
        };

        validator.ValidateAsync(Arg.Any<Sale>()).Returns(new ValidationResult());

        branchProductRepository.GetAsync(1, 1, Arg.Any<Expression<Func<BranchProduct, bool>>>())
            .Returns(Task.FromResult(new PagedResult<BranchProduct>(1, new List<BranchProduct>() { branchProduct })));

        var capturedSale = new Sale();
        repository.AddAsync(Arg.Do<Sale>(s => capturedSale = s)).Returns(sale);

        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);

        // Act
        await saleService.CreateAsync(sale);

        // Assert
        capturedSale.Should().NotBeNull();
        capturedSale.Items.Should().NotBeEmpty();

        capturedSale.Items[0].Discount.Should().Be(25);
        capturedSale.Items[0].Price.Should().Be(75);
        capturedSale.TotalAmount.Should().Be(75);

        await repository.Received(1).AddAsync(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "Cancel Sale Item - Valid Request")]
    [Trait("Sale", "Service")]
    public async Task CancelItemAsync_ShouldCancelSaleItem_WhenValidRequest()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var sale = new SaleMock().Generate();
        sale.Status = SaleStatus.Created;
        var oldSaleAmount = sale.TotalAmount;

        repository.GetWithItemsByIdAsync(sale.Id).Returns(sale);
        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);

        // Act
        var result = await saleService.CancelItemAsync(sale.Id, sale.Items[0].Sequence);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().Contain(i => i.IsCancelled);
        await repository.Received(1).UpdateAsync(Arg.Is<Sale>(s => oldSaleAmount > s.TotalAmount));
        await rabbitMQIntegration.Received(1).PublishMessageAsync(Arg.Is<SaleItemCancelledEvent>(e => e.Sequence == sale.Items[0].Sequence));
    }

    [Fact(DisplayName = "Cancel Sale Item - Sale Already Canceled")]
    [Trait("Sale", "Service")]
    public async Task CancelItemAsync_ShouldThrowException_WhenSaleAlreadyCanceled()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var sale = new SaleMock().Generate();
        sale.Status = SaleStatus.Canceled;

        repository.GetWithItemsByIdAsync(sale.Id).Returns(sale);
        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);

        // Act
        Func<Task> act = () => saleService.CancelItemAsync(sale.Id, sale.Items[0].Sequence);

        // Assert
        await act.Should().ThrowAsync<SaleAlreadyCanceledException>()
            .WithMessage("Cannot cancel an item from a sale that is already canceled.");
    }

    [Fact(DisplayName = "Cancel Sale Item - Item Already Canceled")]
    [Trait("Sale", "Service")]
    public async Task CancelItemAsync_ShouldThrowException_WhenItemAlreadyCanceled()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var sale = new SaleMock().Generate();
        sale.Status = SaleStatus.Created;
        sale.Items[0].IsCancelled = true;

        repository.GetWithItemsByIdAsync(sale.Id).Returns(sale);
        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);

        // Act
        Func<Task> act = () => saleService.CancelItemAsync(sale.Id, sale.Items[0].Sequence);

        // Assert
        await act.Should().ThrowAsync<SaleItemAlreadyCanceledException>()
            .WithMessage("This item is already cancelled.");
    }

    [Fact(DisplayName = "Cancel Sale - Valid Request")]
    [Trait("Sale", "Service")]
    public async Task CancelAsync_ShouldCancelSale_WhenValidRequest()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);
        var saleMock = new SaleMock().Generate();
        saleMock.Status = SaleStatus.Created;
        repository.GetByIdAsync(saleMock.Id).Returns(saleMock);

        // Act
        var result = await saleService.CancelAsync(saleMock.Id);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(SaleStatus.Canceled);
        await repository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.CancelledAt.HasValue));
        await rabbitMQIntegration.Received(1).PublishMessageAsync(Arg.Is<SaleCancelledEvent>(e => e.Id == result.Id));
    }

    [Fact(DisplayName = "Get All Sales - Valid Parameters")]
    [Trait("Sale", "Service")]
    public async Task GetAllAsync_ShouldReturnSales_WhenValidParameters()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);
        var sales = new SaleMock().Generate(1);
        repository.GetAsync(1, 10, Arg.Any<Expression<Func<Sale, bool>>>()).Returns(new PagedResult<Sale>(1, sales));

        // Act
        var result = await saleService.GetAllAsync(null, null, null, null, null, null);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Should().BeEquivalentTo(sales.First());
    }

    [Fact(DisplayName = "Update Sale - Valid Request")]
    [Trait("Sale", "Service")]
    public async Task UpdateAsync_ShouldUpdateSale_WhenValidRequest()
    {
        // Arrange
        var (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger) = CreateDependencies();

        var saleMock = new SaleMock().Generate();
        saleMock.Status = SaleStatus.Created;
        repository.GetWithItemsByIdAsync(saleMock.Id).Returns(saleMock);
        repository.UpdateAsync(Arg.Any<Sale>()).Returns(saleMock);
        validator.ValidateAsync(Arg.Is<Sale>(s => s.Id == saleMock.Id)).Returns(new ValidationResult());

        var saleService = new SaleService(repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);

        // Act
        var result = await saleService.UpdateAsync(saleMock.Id, saleMock);

        // Assert
        result.Should().NotBeNull();
        await repository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.Id == saleMock.Id));
        await rabbitMQIntegration.Received(1).PublishMessageAsync(Arg.Is<SaleUpdatedEvent>(e => e.Id == result.Id));
    }

    private (ISaleRepository, ISaleItemRepository, IBranchProductRepository, IValidator<Sale>, IRabbitMQIntegration, ILogger<SaleService>) CreateDependencies()
    {
        var repository = Substitute.For<ISaleRepository>();
        var saleItemRepository = Substitute.For<ISaleItemRepository>();
        var branchProductRepository = Substitute.For<IBranchProductRepository>();
        var validator = Substitute.For<IValidator<Sale>>();
        var rabbitMQIntegration = Substitute.For<IRabbitMQIntegration>();
        var logger = Substitute.For<ILogger<SaleService>>();

        return (repository, saleItemRepository, branchProductRepository, validator, rabbitMQIntegration, logger);
    }
}
