﻿using Ambev.Domain.Entities;
using Ambev.Domain.Enums;
using Bogus;

namespace Ambev.Tests.Mocks.Entities;

public class BranchProductMock : Faker<BranchProduct>
{
    public BranchProductMock()
    {
        RuleFor(bp => bp.Id, f => f.IndexFaker + 1)
        .RuleFor(bp => bp.BranchId, f => f.Random.Int(1, 100))
        .RuleFor(bp => bp.ProductId, f => f.Random.Int(1, 1000))
        .RuleFor(bp => bp.ProductName, f => f.Commerce.ProductName())
        .RuleFor(bp => bp.ProductCategory, f => f.PickRandom<ProductCategory>())
        .RuleFor(bp => bp.Price, f => f.Finance.Amount(1, 1000, 2))
        .RuleFor(bp => bp.StockQuantity, f => f.Random.Int(0, 100))
        .RuleFor(bp => bp.IsActive, f => f.Random.Bool());
    }
}