using Ambev.Domain.Entities;

namespace Ambev.Domain.Interfaces.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(int? id, bool? isActive, string? name, DateTime? startDate, DateTime? endDate, int page = 1, int maxResults = 10);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product request);
    Task<Product> UpdateAsync(int id, Product request);
    Task DeleteAsync(int id);
}