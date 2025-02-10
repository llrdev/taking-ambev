using Ambev.Domain.Entities;
using Ambev.Domain.Enums;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Infrastructure.Contexts;
using Ambev.Infrastructure.Scripts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class BranchProductRepository : BaseRepository<BranchProduct>, IBranchProductRepository
{
    public BranchProductRepository(SqlDbContext dbContext) : base(dbContext)
    {
    }

    public async Task UpdateByProductIdAsync(int productId, string productName, ProductCategory productCategory)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(SqlScripts.UpdateBranchProductsByProductId,
                                                     productName,
                                                     (int)productCategory,
                                                     productId);
    }
}