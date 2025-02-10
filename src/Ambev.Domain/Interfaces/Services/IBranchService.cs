﻿using Ambev.Domain.Entities;

namespace Ambev.Domain.Interfaces.Services;

public interface IBranchService
{
    Task<List<Branch>> GetAllAsync(int? id, bool? isActive, string? name, DateTime? startDate, DateTime? endDate, int page = 1, int maxResults = 10);
    Task<Branch?> GetByIdAsync(int id);
    Task<Branch> CreateAsync(Branch request);
    Task<Branch> UpdateAsync(int id, Branch request);
    Task DeleteAsync(int id);
}