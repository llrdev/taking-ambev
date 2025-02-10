using Ambev.Domain.Base;
using Ambev.Domain.Base.Interfaces;
using Ambev.Domain.Exceptions;
using Ambev.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Ambev.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
{
    protected readonly SqlDbContext _dbContext;

    public BaseRepository(SqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<T>> GetAsync(int page = 1, int maxResults = 10, Expression<Func<T, bool>>? criteria = default)
    {
        page = page == 0 ? 1 : page;
        int count = (page - 1) * maxResults;

        IQueryable<T> query = _dbContext.Set<T>().AsQueryable();

        if (criteria is not null)
            query = query.Where(criteria);

        var totalRecords = await query.CountAsync();
        var items = await query.Skip(count).Take(maxResults).ToListAsync();

        return new PagedResult<T>(totalRecords, items);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        if (entity.IsDeleted)
            throw new EntityAlreadyDeletedException("The entity is already deleted.");

        entity.IsDeleted = true;

        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entity;
    }
}