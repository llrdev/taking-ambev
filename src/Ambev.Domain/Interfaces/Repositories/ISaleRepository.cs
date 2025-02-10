using Ambev.Domain.Base.Interfaces;
using Ambev.Domain.Entities;

namespace Ambev.Domain.Interfaces.Repositories;

public interface ISaleRepository : IBaseRepository<Sale>
{
    new Task<Sale?> GetWithItemsByIdAsync(int id);
}