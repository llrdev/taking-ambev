using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Infrastructure.Contexts;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class BranchRepository : BaseRepository<Branch>, IBranchRepository
{
    public BranchRepository(SqlDbContext dbContext) : base(dbContext)
    {
    }
}