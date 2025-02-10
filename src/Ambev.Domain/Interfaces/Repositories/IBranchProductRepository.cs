using Ambev.Domain.Base.Interfaces;
using Ambev.Domain.Entities;
using Ambev.Domain.Enums;

namespace Ambev.Domain.Interfaces.Repositories;

public interface IBranchProductRepository : IBaseRepository<BranchProduct>
{
    Task UpdateByProductIdAsync(int productId, string productName, ProductCategory productCategory);
}