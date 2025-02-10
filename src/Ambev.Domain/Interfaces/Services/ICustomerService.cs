﻿using Ambev.Domain.Entities;

namespace Ambev.Domain.Interfaces.Services;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync(int? id, string? name, string? document, string? phone, string? email, bool? isActive, DateTime? startDate, DateTime? endDate, int page = 1, int maxResults = 10);
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer> CreateAsync(Customer request);
    Task<Customer> UpdateAsync(int id, Customer request);
    Task DeleteAsync(int id);
}