using Ambev.Domain.Entities;
using Ambev.Domain.Enums;
using Bogus;

namespace Ambev.Tests.Mocks.Entities;

public class ProductMock : Faker<Product>
{
    public ProductMock()
    {
        RuleFor(p => p.Id, f => f.Random.Short())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.PickRandom<ProductCategory>())
        .RuleFor(p => p.BasePrice, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.IsActive, f => f.Random.Bool());
    }
}