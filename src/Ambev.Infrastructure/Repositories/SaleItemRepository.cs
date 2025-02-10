using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Infrastructure.Contexts;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class SaleItemRepository : BaseRepository<SaleItem>, ISaleItemRepository
{
    public SaleItemRepository(SqlDbContext dbContext) : base(dbContext)
    {
    }
}