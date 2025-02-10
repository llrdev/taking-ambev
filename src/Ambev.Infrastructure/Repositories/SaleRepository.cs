using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class SaleRepository : BaseRepository<Sale>, ISaleRepository
{
    public SaleRepository(SqlDbContext dbContext) : base(dbContext)
    {
    }

    public new async Task<Sale?> GetWithItemsByIdAsync(int id)
    {
        return await _dbContext.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}