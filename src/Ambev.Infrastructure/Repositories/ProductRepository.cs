using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Infrastructure.Contexts;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(SqlDbContext dbContext) : base(dbContext)
    {
    }
}