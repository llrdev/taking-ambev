using Ambev.Domain.Entities;
using Ambev.Domain.Interfaces.Repositories;
using Ambev.Infrastructure.Contexts;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(SqlDbContext dbContext) : base(dbContext)
    {
    }
}